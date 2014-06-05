using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using UseBookmarks.Activities;
using System.Threading;

namespace UseBookmarks
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity definition = new WorkflowDeBase();
            
            // Lancer le premier host
            Application app1 = new Application(definition);
            app1.Run();
            app1.Resume(1);
            Console.Read();
        }
    }
}
