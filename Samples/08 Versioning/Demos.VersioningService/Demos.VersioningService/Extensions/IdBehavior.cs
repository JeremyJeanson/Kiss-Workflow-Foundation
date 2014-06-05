using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activities;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Web;
using System.Activities.DurableInstancing;

namespace Demos.VersioningService.Extensions
{
    public sealed class IdBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // Not used
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            WorkflowServiceHost host = serviceHostBase as WorkflowServiceHost;
            if (host == null) return;

            host.WorkflowExtensions.Add<IdParticipant>(() => new IdParticipant());

            var store = host.DurableInstancingOptions.InstanceStore as SqlWorkflowInstanceStore;
            store.Promote("IdParticipant", IdParticipant.GetPropotionsParticipants(),null);
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            // Not used
        }
    }

    public sealed class IdBehaviorExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(IdBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new IdBehavior();
        }
    }
}