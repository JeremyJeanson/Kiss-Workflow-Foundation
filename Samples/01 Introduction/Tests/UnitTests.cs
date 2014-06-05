using System;
using System.Activities;
using System.Collections.Generic;
using Demos.Introduction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestSequenceType()
        {
            const string name = "Toto";

            Activity workflow1 = new SequenceType();
            // Create arguments In ->
            IDictionary<String, Object> argsIn = new Dictionary<string, object>
                {
                    {"Name", name}
                };

            // Invoke the workflow
            IDictionary<String, Object> argsOut = WorkflowInvoker.Invoke(workflow1, argsIn);

            // Test teh result
            Assert.IsTrue(argsOut["Result"].ToString().StartsWith("Hello " + name));
        }

        [TestMethod]
        public void TestFlowchartType()
        {
            const string name = "Toto";

            Activity workflow1 = new FlowchartType();
            // Create arguments In ->
            IDictionary<String, Object> argsIn = new Dictionary<string, object>
                {
                    {"Name", name}
                };

            // Invoke the workflow
            IDictionary<String, Object> argsOut = WorkflowInvoker.Invoke(workflow1, argsIn);

            // Test teh result
            Assert.IsTrue(argsOut["Result"].ToString().StartsWith("Hello " + name));
        }

        [TestMethod]
        public void TestSequenceTypesWithoutDictonary()
        {
            const string name = "Toto";

            Activity workflow1 = new SequenceType() { Name = new InArgument<string>(name) };

            // Invoke the workflow
            IDictionary<String, Object> argsOut = WorkflowInvoker.Invoke(workflow1);

            // Test teh result
            Assert.IsTrue(argsOut["Result"].ToString().StartsWith("Hello " + name));
        }

        [TestMethod]
        public void TestSequenceTypesImplicit()
        {
            const string name = "Toto";

            Activity workflow1 = new SequenceType() { Name = name };

            // Invoke the workflow
            IDictionary<String, Object> argsOut = WorkflowInvoker.Invoke(workflow1);

            // Test teh result
            Assert.IsTrue(argsOut["Result"].ToString().StartsWith("Hello " + name));
        }
    }
}
