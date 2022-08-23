using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Teriyaki.GraphFlow.NodeHandlers;
using Teriyaki.GraphFlow.Runtime;
using Teriyaki.GraphFlow.Tests.Runtime;
using Xunit;
using Xunit.Abstractions;

namespace Teriyaki.GraphFlow.Tests
{
    public abstract class GraphTestsBase : XunitContextBase
    {
        ILogger _output;
        protected GraphModel _graph;
        protected readonly ContainerBuilder _servicesBuilder;

        public GraphTestsBase(ITestOutputHelper output, [CallerFilePath] string sourceFile = "")
            : base(output, sourceFile)
        {
            _output = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output, LogEventLevel.Verbose)
                .CreateLogger()
                .ForContext<SimpleGraph>();
            //
            Log.Logger = _output;
            Log.Information("Start test.");
            //
            _graph = new GraphModel();
            //
            _servicesBuilder = new ContainerBuilder();
            //
            _servicesBuilder.RegisterModule<HandlersModule>();
        }

        protected GraphRuntime CreateGraph()
        {
            var container = _servicesBuilder.Build();
            var provider = new AutofacServiceProvider(container);
            var runtime = new GraphRuntime(provider);
            //
            runtime.Load(_graph).GetAwaiter().GetResult();
            //
            return runtime;
        }
    }
}