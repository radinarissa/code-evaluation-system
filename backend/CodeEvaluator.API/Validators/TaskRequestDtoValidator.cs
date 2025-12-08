using CodeEvaluator.API.DTOs;
using FluentValidation;

namespace CodeEvaluator.API.Validators
{
    public class TaskRequestDtoValidator : AbstractValidator<TaskRequestDto>
    {
        public TaskRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Task name is required.")
                .MaximumLength(200).WithMessage("Task name must be at most 200 characters long.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Task description is required.");

            RuleFor(x => x.MaxPoints)
                .GreaterThan(0).WithMessage("MaxPoints must be greater than 0.");

            RuleFor(x => x.MaxExecutionTimeMs)
                .GreaterThan(0).WithMessage("MaxExecutionTimeMs must be greater than 0.");

            RuleFor(x => x.MaxDiskUsageMb)
                .GreaterThan(0).WithMessage("MaxDiskUsageMb must be greater than 0.");

            RuleFor(x => x.TestCases)
                .NotNull().WithMessage("At least one test case is required.")
                .Must(list => list.Count > 0).WithMessage("At least one test case is required.");

            RuleForEach(x => x.TestCases).ChildRules(tc =>
            {
                tc.RuleFor(t => t.Name)
                    .NotEmpty().WithMessage("Test case name is required.");

                tc.RuleFor(t => t.InputFilePath)
                    .NotEmpty().WithMessage("Input file path is required.");
            });
        }
    }
}
