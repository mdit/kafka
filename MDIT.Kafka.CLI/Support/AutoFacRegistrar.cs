// Copyright (c) AMR GP Limited 2021.

using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace MDIT.Kafka.CLI.Support
{
    internal class AutoFacRegistrar : ITypeRegistrar
    {
        private readonly ContainerBuilder _containerBuilder;

        public AutoFacRegistrar(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public void Register(Type service, Type implementation)
        {
            _containerBuilder.RegisterType(implementation).As(service);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _containerBuilder.RegisterInstance(implementation).As(service);
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            _containerBuilder.Register(_ => factory()).As(service);
        }

        public ITypeResolver Build()
        {
            var container = _containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);
            return new TypeResolver(serviceProvider);
        }
    }
}