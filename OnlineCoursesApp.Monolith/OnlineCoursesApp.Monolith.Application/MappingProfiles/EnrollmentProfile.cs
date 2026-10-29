using AutoMapper;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Dtos;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.MappingProfiles;
public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title));
    }
}