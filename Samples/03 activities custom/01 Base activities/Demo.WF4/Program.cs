using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Demo.WF4
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkflowInvoker.Invoke(new MyWorkflow());
            Console.Read();
        }
    }
}
