using Spectre.Console;

namespace MDIT.Kafka.CLI.Commands
{
    public static class AbstractValidatorExtensions
    {
        public static ValidationResult AsValidationResult(this FluentValidation.Results.ValidationResult result)
        {
            return result.IsValid ? ValidationResult.Success() : ValidationResult.Error(result.ToString());
        }
    }
}