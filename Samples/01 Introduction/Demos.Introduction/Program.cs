using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Activities;
using System.Activities.Statements;

namespace Demos.Introduction
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Get a name to send to workflow
            Console.WriteLine("What is your name?");
            String name = Console.ReadLine();

            // Create an instance
            Activity workflow1 = new SequenceType();
            // Create arguments In ->
            IDictionary<String, Object> argsIn = new Dictionary<string, object>
                {
                    {"Name", name}
                };

            // Invoke the workflow
            IDictionary<String, Object> argsOut = WorkflowInvoker.Invoke(workflow1, argsIn);

            // Prompt result
            Console.WriteLine(argsOut["Result"]);
            Console.Read();
        }
    }
}
