using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using Demos.UseExtensions.Extensions;

namespace Demos.UseExtensions
{

    class Program
    {
        static void Main(string[] args)
        {
            var workflow = new Workflow1();

            // Creat an host
            WorkflowInvoker invoker = new WorkflowInvoker(workflow);

            // Add extension
            invoker.Extensions.Add(new MyExtension());

            //Invoke
            invoker.Invoke();

            Console.Read();
        }
    }
}
