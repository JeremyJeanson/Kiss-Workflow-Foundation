using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Expressions;
using Microsoft.VisualBasic.Activities;

namespace Demo.WF4
{
    class Invoker
    {
        public void Test()
        {
            //Int32 i = 6;
            //MyWorkflow workflow = new MyWorkflow() { Argument1 = i };
            //WorkflowInvoker.Invoke(workflow);


            //Int32 i = 6;
            //MyWorkflow workflow = new MyWorkflow() { Argument1 = new Literal<Int32>(i) };
            //WorkflowInvoker.Invoke(workflow);

            //String expression = "6";
            //MyWorkflow workflow = new MyWorkflow() { Argument1 = new VisualBasicValue<Int32>(expression) };
            //WorkflowInvoker.Invoke(workflow);
            User u = new User() { Name = "Doe", FirstName = "John" };
            MyWorkflow workflow = new MyWorkflow() { Argument1 = new LambdaValue<User>(c => u) };
            WorkflowInvoker.Invoke(workflow);
        }

        internal class User
        {
            public String Name { get; set; }
            public String FirstName { get; set; }
        }
    }
}
