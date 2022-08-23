using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Teriyaki.GraphFlow.NodeHandlers;
using Teriyaki.GraphFlow.Runtime;

namespace Teriyaki.GraphFlow
{
    public class HandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NodesFactory>().AsImplementedInterfaces();
            builder.RegisterType<LinksFactory>().AsImplementedInterfaces();
            //
            //builder.RegisterGeneric(typeof(NodeRuntime<>));
            //builder.RegisterType<LinkRuntime>().AsImplementedInterfaces();
            //
            builder.RegisterType<ConstStringNodeHandler>().AsImplementedInterfaces();
            builder.RegisterType<ConstNumberNodeHandler>().AsImplementedInterfaces();
            builder.RegisterType<InputValueNodeHandler>().AsImplementedInterfaces();
            builder.RegisterType<ButtonNodeHandler>().AsImplementedInterfaces();
            builder.RegisterType<IfNodeHandler>().AsImplementedInterfaces();
        }
    }

    public class NodesFactory : INodesFactory
    {
        public INodeRuntime CreateNodeRuntime(IGraphNodeModel node)
        {
            var runtimeType = node.GetNodeRuntimeType();
            return (INodeRuntime)Activator.CreateInstance(runtimeType);
        }
    }

    public class LinksFactory : ILinksFactory
    {
        public ILinkRuntime CreateLinkRuntime(GraphLinkModel link)
        {
            //var runtimeType = typeof(ILinkRuntime);
            //return (ILinkRuntime)Activator.CreateInstance(typeof(LinkRuntime));
            return new LinkRuntime();
        }
    }
}
