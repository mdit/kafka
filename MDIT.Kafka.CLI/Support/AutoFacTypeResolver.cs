// Copyright (c) AMR GP Limited 2021.

using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace MDIT.Kafka.CLI.Support
{
    public class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;

        public TypeResolver(
            IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentOutOfRangeException(nameof(provider));
        }

        public object Resolve(Type type)
        {
            try
            {
                return _provider.GetRequiredService(type);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            (_provider as IDisposable)?.Dispose();
        }
    }
}