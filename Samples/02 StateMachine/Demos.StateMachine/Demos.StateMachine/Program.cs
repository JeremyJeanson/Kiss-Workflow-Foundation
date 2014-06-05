using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;

namespace Demos.StateMachine
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity workflow1 = new Pouet();
            WorkflowInvoker.Invoke(workflow1);
            Console.Read();
        }
    }
}
