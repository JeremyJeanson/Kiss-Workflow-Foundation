using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.Statements;
using System.Collections.ObjectModel;

namespace MyActivities
{

    public sealed class ActivityWithActivities : NativeActivity
    {
        public Collection<Activity> Activities { get; set; }
        private Variable<Int32> _index;

        public ActivityWithActivities()
        {
            Activities = new Collection<Activity>();
        }
        protected override void Execute(NativeActivityContext context)
        {            
            if (this.Activities.Count > 0)
            {
                context.SetValue(_index, 0);
                this.NextActivity(context);
            }
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.AddImplementationVariable(_index);
        }

        private void NextActivity(NativeActivityContext context)
        {
            Int32 index = context.GetValue(_index);
            context.ScheduleActivity(
                this.Activities[index],
                this.CompletionCallback,
                this.FaultCallback);
            context.SetValue(_index, index + 1);
        }

        private void CompletionCallback(NativeActivityContext context, ActivityInstance completedInstance)
        {
            if (context.GetValue(_index) < this.Activities.Count)
            {
                this.NextActivity(context);
            }
        }

        private void FaultCallback(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
        }
    }
}
