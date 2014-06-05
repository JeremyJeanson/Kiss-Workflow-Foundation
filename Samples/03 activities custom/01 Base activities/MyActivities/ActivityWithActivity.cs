using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace MyActivities
{

    public sealed class ActivityWithActivity : NativeActivity
    {
        public Activity Body { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            context.ScheduleActivity(
                this.Body, 
                this.CompletionCallback,
                this.FaultCallback);
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            metadata.AddChild(Body);
        }

        private void CompletionCallback(NativeActivityContext context, ActivityInstance completedInstance)
        {
        }

        private void FaultCallback(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
        }
    }
}
