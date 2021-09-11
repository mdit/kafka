// Copyright (c) AMR GP Limited 2021.

using System;
using System.Threading;
using System.Threading.Tasks;
using MDIT.Kafka.CLI.Support;
using MDIT.Kafka.Producers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MDIT.Kafka.CLI.Commands
{
    internal sealed partial class ProducerCommand : AsyncCommand<ProducerCommand.Settings>
    {
        private readonly ILogger<ProducerCommand> _logger;
        private readonly MessageProducer<int> _producer;

        public ProducerCommand(
            ILogger<ProducerCommand> logger,
            MessageProducer<int> producer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        public static void Register(IConfigurator configurator)
        {
            configurator.AddCommand<ProducerCommand>("produce")
                        .WithAlias("p")
                        .WithDescription("Run Producer")
                        .WithExample(new[] { "-t", "topic-name", "-c", "5" });
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            try
            {
                await _producer.Produce(settings.Topic, settings.MessageCount, CancellationToken.None);
                return ExitCode.Success;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not execute ProducerCommand.");
                return ExitCode.Error;
            }
        }

        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            return new Settings.Validator()
                   .Validate(settings)
                   .AsValidationResult();
        }
    }
}
