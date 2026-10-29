using AutoMapper;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.CreateCourse;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourses;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.MappingProfile;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<GetCoursesQuery, CourseFilter>();

        CreateMap<CreateCourseCommand, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}