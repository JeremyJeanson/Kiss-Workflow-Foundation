using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using Demos.ConsoleTracking.Extensions;

namespace Demos.ConsoleTracking
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity workflow1 = new Workflow1();
            WorkflowInvoker host = new WorkflowInvoker(workflow1);
            host.Extensions.Add(new TraceTracking());
            host.Invoke();
        }
    }
}
