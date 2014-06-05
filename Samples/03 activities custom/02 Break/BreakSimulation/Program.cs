using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;

namespace BreakSimulation
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity workflow1 = new Simulation();
            WorkflowInvoker.Invoke(workflow1);
            Console.Read();
        }
    }
}
