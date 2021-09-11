using System.ComponentModel;
using FluentValidation;
using Spectre.Console.Cli;

namespace MDIT.Kafka.CLI.Commands
{
    internal sealed partial class ProducerCommand
    {
        public class Settings : CommandSettings
        {
            [CommandOption("-t|--topic <TOPIC>")]
            [Description("The name of the topic to publish events to.")]
            public string Topic { get; set; }

            [CommandOption("-c|--count <MESSAGE_COUNT>")]
            [Description("The number of messages to publish.")]
            public int MessageCount { get; set; }

            public class Validator : AbstractValidator<Settings>
            {
                public Validator()
                {
                    RuleFor(x => x.MessageCount).GreaterThan(0).WithMessage("Message Count must be greater than zero");
                    RuleFor(x => x.Topic).NotEmpty().WithMessage("Topic must be specified");
                }
            }
        }
    }
}