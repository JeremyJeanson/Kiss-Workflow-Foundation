using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UseTemplateFactory.Extensions;

namespace UseTemplateFactory.Activities
{
    public class ParallelForEachBreak<T> : IActivityTemplateFactory
    {
        public Activity Create(DependencyObject target)
        {
            return new TryCatch
            {
                Try = new ParallelForEach<T>
                {
                    Body = new ActivityAction<T>
                    {
                        Argument = new DelegateInArgument<T>("item"),
                        Handler = new Sequence
                        {
                            Activities = { new Break()}
                        }
                    }
                },
                Catches =
                {
                    new Catch<BreackException>()
                }
            };
        }
    }
}
