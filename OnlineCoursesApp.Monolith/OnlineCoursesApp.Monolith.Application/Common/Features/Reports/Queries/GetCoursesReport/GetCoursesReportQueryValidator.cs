using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Queries.GetCoursesReport;
public class GetCoursesReportQueryValidator : AbstractValidator<GetCoursesReportQuery>
{
    public GetCoursesReportQueryValidator()
    {
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.To)
            .GreaterThanOrEqualTo(x => x.From)
            .WithMessage("The date 'To' must be no earlier than 'From'.");
        RuleFor(x => x.To)
            .LessThanOrEqualTo(_ => DateTime.UtcNow.AddDays(1))
            .WithMessage("The 'To' date cannot be in the future.");
    }
}