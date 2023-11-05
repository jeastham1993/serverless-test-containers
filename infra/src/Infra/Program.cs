using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new InfraStack(app, "InfraStack", new StackProps
            {
            });
            app.Synth();
        }
    }
}
