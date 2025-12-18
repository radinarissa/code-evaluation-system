using CodeEvaluator.Application.DTOs;
using FluentValidation;

namespace CodeEvaluator.API.Validators
{
    public class SubmissionRequestDtoValidator : AbstractValidator<SubmissionRequestDto>
    {
        public SubmissionRequestDtoValidator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("TaskId must be greater than 0.");

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required.");

            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Language is required.");

            RuleFor(x => x.SourceCode)
                .NotEmpty().WithMessage("SourceCode is required.")
                .MinimumLength(10).WithMessage("SourceCode must be at least 10 characters long.");

            // Moodle fields
            RuleFor(x => x.MoodleUserId)
                .NotEmpty().WithMessage("MoodleUserId is required.");

            RuleFor(x => x.MoodleCourseId)
                .NotEmpty().WithMessage("MoodleCourseId is required.");

            RuleFor(x => x.MoodleSubmissionId)
                .NotEmpty().WithMessage("MoodleSubmissionId is required.");
        }
    }
}
