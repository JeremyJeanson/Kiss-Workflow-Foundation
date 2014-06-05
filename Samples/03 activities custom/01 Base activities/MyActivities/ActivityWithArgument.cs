using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace MyActivities
{

    public sealed class ActivityWithArgument : CodeActivity
    {
        public InArgument<string> MyArgument { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string text = context.GetValue(this.MyArgument);
        }

        /// <summary>
        /// Optimisation de la metadata
        /// </summary>
        /// <param name="metadata"></param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            RuntimeArgument arg = new RuntimeArgument("MyArgument", typeof(String), ArgumentDirection.In);
            metadata.Bind(this.MyArgument, arg);
            metadata.AddArgument(arg);
        }
    }
}
