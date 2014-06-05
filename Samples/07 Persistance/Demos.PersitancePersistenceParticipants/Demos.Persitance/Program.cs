using System;
using System.Activities.DurableInstancing;
using System.Configuration;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.Threading;
using Demos.Persitance.Extensions;

namespace Demos.Persitance
{
    class Program
    {
        private static Guid _instanceId;
        private static AutoResetEvent _wait;

        static void Main(string[] args)
        {
            _wait = new AutoResetEvent(false); 
            GetApp().Run();

            _wait.WaitOne();
            _wait = new AutoResetEvent(false);
            Console.Read();

            // Add breakpoint here
            var app = GetApp();
            app.Load(_instanceId);
            app.Run();

            _wait.WaitOne();
            Console.Read();
        }

        private static WorkflowApplication GetApp()
        {
            // Create a new workflow
            Activity workflow1 = new Workflow1();

            // Create a new host
            WorkflowApplication app = new WorkflowApplication(workflow1);
            
            SqlWorkflowInstanceStore store =
                new SqlWorkflowInstanceStore(ConfigurationManager.ConnectionStrings["Demos"].ConnectionString);
            
            // Delete all when workflow instance is terminated
            // store.InstanceCompletionAction = InstanceCompletionAction.DeleteAll;

            // Pomote extensions properties (InstancePromotedPropertiesTable)
            store.Promote("MyExtension", MyExtension.GetValuesToPromote(), null);

            app.InstanceStore = store;

            // Add extension
            MyExtension extension = new MyExtension();
            app.Extensions.Add(extension);
            app.PersistableIdle = PersistableIdle;
            app.Unloaded = Unloaded;
            return app;
        }


        private static PersistableIdleAction PersistableIdle(WorkflowApplicationIdleEventArgs arg)
        {
            return PersistableIdleAction.Unload;
        }

        private static void Unloaded(WorkflowApplicationEventArgs obj)
        {
            _instanceId = obj.InstanceId;
            Console.WriteLine("Unloaded, InstanceId=" + _instanceId);
            _wait.Set();
        }
    }
}
