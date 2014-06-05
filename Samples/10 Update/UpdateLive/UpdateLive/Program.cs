using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using UpdateLive.Activities;
using System.Threading;

namespace UpdateLive
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity definition = new BookmarkActivity();
            AutoResetEvent idle1;
            
            // Lancer le premier host
            Application app1 = new Application(definition);
            app1.Run();
            app1.Resume(1);
        }

        private static WorkflowApplication GetHost(Activity definition, AutoResetEvent idleEvent)
        {
            // Instanciation d'un hôte
            WorkflowApplication host = new WorkflowApplication(definition);
            if (idleEvent != null)
            {
                host.Idle = e => {
                    idleEvent.Set();
                };
            }

            return host;
        }
    }
}
