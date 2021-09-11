using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using MDIT.Kafka.CLI.Commands;
using MDIT.Kafka.CLI.Options;
using MDIT.Kafka.CLI.Support;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

try
{
    var registrar = GetTypeRegistrar();
    var app = new CommandApp(registrar);
    app.Configure(ConfigureCommands);
    await app.RunAsync(args);
}
catch (Exception exception)
{
    AnsiConsole.WriteException(exception);
}
finally
{
    Console.ReadLine();
}

static ITypeRegistrar GetTypeRegistrar()
{
    var configuration = GetConfiguration();
    var serviceCollection = CreateServices(configuration);
    var containerBuilder = CreateContainerBuilder(serviceCollection);
    return new AutoFacRegistrar(containerBuilder);
}

static IServiceCollection CreateServices(IConfiguration configuration)
{
    return new ServiceCollection().AddLogging(builder => builder.AddConsole())
                                  .AddOptions()
                                  .ConfigureOptions<ConfigureProducerConfig>()
                                  .Configure<ProducerConfig>(configuration.GetSection(nameof(ProducerConfig)))
                                  .Configure<AvroSerializerConfig>(configuration.GetSection(nameof(AvroSerializerConfig)));
}

static IConfiguration GetConfiguration()
{
    return new ConfigurationBuilder()
           .SetBasePath(Environment.CurrentDirectory)
           .AddJsonFile("appsettings.json", optional: false)
           .Build();
}

static ContainerBuilder CreateContainerBuilder(IServiceCollection services)
{
    var builder = new ContainerBuilder();
    builder.Populate(services);

    var assemblies = GetAssemblies("MDIT*.dll").ToArray();
    builder.RegisterAssemblyModules(assemblies);
    return builder;
}

static IEnumerable<Assembly> GetAssemblies(string pattern)
{
    foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory, pattern).OrderBy(x => x))
    {
        yield return Assembly.LoadFrom(file);
    }
}

static void ConfigureCommands(IConfigurator configurator)
{
    ProducerCommand.Register(configurator);
}