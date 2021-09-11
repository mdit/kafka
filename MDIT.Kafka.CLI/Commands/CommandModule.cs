// Copyright (c) AMR GP Limited 2021.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace MDIT.Kafka.CLI.Commands
{
    public class CommandModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProducerCommand>().AsSelf();
        }
    }
}
