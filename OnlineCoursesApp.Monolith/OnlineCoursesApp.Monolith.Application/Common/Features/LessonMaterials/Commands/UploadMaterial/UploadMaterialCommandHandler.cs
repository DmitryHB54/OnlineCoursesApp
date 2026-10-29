using AutoMapper;
using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.UploadMaterial;

public class UploadMaterialCommandHandler
    : IRequestHandler<UploadMaterialCommand, LessonMaterialDto>
{
    private readonly ILessonMaterialRepository _lessonMaterialRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UploadMaterialCommandHandler(
        ILessonMaterialRepository lessonMaterialRepository,
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _lessonMaterialRepository = lessonMaterialRepository;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<LessonMaterialDto> Handle(UploadMaterialCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Course not found");

        if(_currentUserService is { Role: RoleName.Teacher } && _currentUserService.UserId != course.AuthorId)
        {
            throw new ForbiddenException("It's prohibited to add content to someone else's course.");
        }

        var stored = await _fileStorageService.SaveAsync(
            request.CourseId, request.FileContent, request.FileName, cancellationToken);

        uint orderNumber = await _lessonMaterialRepository.GetNextOrderNumberAsync(request.CourseId, cancellationToken);

        var material = new LessonMaterial
        {
            Id = Guid.NewGuid(),
            CourseId = request.CourseId,
            Title = request.Title.Trim(),
            OrderNumber = orderNumber,
            MaterialType = request.MaterialType,
            CreatedAt = DateTime.UtcNow,
            FileInfo = new Domain.Entities.FileInfo(
                stored.RelativePath,
                request.FileName,
                stored.SizeBytes,
                stored.Extension)
        };

        await _lessonMaterialRepository.AddAsync(material, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LessonMaterialDto>(material);
    }
}