using System;
using System.Activities.Presentation.Toolbox;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehostingLib
{
    internal static class ToolboxHelper
    {
        internal static void AddAll(ToolboxControl control)
        {
            var categories = GetAll();
            foreach (var category in categories)
            {
                control.Categories.Add(category);
            }
        }

        internal static ToolboxCategory[] GetAll()
        {
            return new ToolboxCategory[]{
                GetToolboxControlFlow(),
                GetToolboxFlowchart(),
                GetToolboxStateMachine(),
                GetToolboxMessaging(),
                GetToolboxRuntime(),
                GetToolboxPrimitives(),
                GetToolboxTransaction(),
                GetToolboxCollection(),
                GetToolboxErrorHandling()
            };
        }

        /// <summary>
        /// ToolboxControlFlow
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxControlFlow()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxControlFlow);
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.DoWhile)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.ForEach<>)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.If)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Parallel)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.ParallelForEach<>)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Pick)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.PickBranch)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Sequence)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Switch<>)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.While)));
            return category;
        }

        /// <summary>
        /// ToolboxFlowchart
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxFlowchart()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxFlowchart);
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Flowchart)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.FlowDecision)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.FlowSwitch<>)));
            return category;
        }

        /// <summary>
        /// ToolboxStateMachine
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxStateMachine()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxStateMachine);
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.StateMachine)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.State)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Core.Presentation.FinalState)));
            return category;
        }

        /// <summary>
        /// ToolboxMessaging
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxMessaging()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxMessaging);
            category.Add(new ToolboxItemWrapper(typeof(System.ServiceModel.Activities.CorrelationScope)));
            category.Add(new ToolboxItemWrapper(typeof(System.ServiceModel.Activities.InitializeCorrelation)));
            category.Add(new ToolboxItemWrapper(typeof(System.ServiceModel.Activities.Receive)));
            category.Add(new ToolboxItemWrapper(typeof(System.ServiceModel.Activities.Presentation.Factories.ReceiveAndSendReplyFactory)));
            category.Add(new ToolboxItemWrapper(typeof(System.ServiceModel.Activities.Send)));
            category.Add(new ToolboxItemWrapper(typeof(System.ServiceModel.Activities.Presentation.Factories.SendAndReceiveReplyFactory)));
            category.Add(new ToolboxItemWrapper(typeof(System.ServiceModel.Activities.TransactedReceiveScope)));
            return category;
        }

        /// <summary>
        /// ToolboxRuntime
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxRuntime()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxRuntime);
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Persist)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.TerminateWorkflow)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.NoPersistScope)));
            return category;
        }

        /// <summary>
        /// ToolboxPrimitives
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxPrimitives()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxPrimitives);
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Assign)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Delay)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.InvokeDelegate)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.InvokeMethod)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.WriteLine)));
            return category;
        }

        /// <summary>
        /// ToolboxTransaction
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxTransaction()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxTransaction);
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.CancellationScope)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.CompensableActivity)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Compensate)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Confirm)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.TransactionScope)));
            return category;
        }

        /// <summary>
        /// ToolboxCollection
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxCollection()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxCollection);
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.AddToCollection<>)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.ClearCollection<>)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.ExistsInCollection<>)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.RemoveFromCollection<>)));
            return category;
        }

        /// <summary>
        /// ErrorHandling
        /// </summary>
        /// <returns></returns>
        internal static ToolboxCategory GetToolboxErrorHandling()
        {
            ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxErrorHandling);
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Rethrow)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Throw)));
            category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.TryCatch)));
            return category;
        }
        
        // Pour WF3
        //internal static ToolboxCategory GetToolboxMigration()
        //{
        //    ToolboxCategory category = new ToolboxCategory(Properties.Resources.ToolboxMigration);
        //    category.Add(new ToolboxItemWrapper(typeof(System.Activities.Statements.Interop)));
        //    return category;
        //}
    }
}
