using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demos.UseExtensions.Extensions;

namespace Demos.UseExtensions.Activities
{
    public sealed class UseMyExtension:CodeActivity
    {
        public enum Actions
        {
            Action1,
            Action2
        };

        public Actions Action { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // Gest extention
            IMyExtension extension = context.GetExtension<IMyExtension>();

            if (extension != null)
            {
                switch (Action)
                {
                    case Actions.Action1:
                        {
                            extension.DoAction1();
                            break;
                        }
                    case Actions.Action2:
                        {
                            extension.DoAction2();
                            break;
                        }
                }
            }
        }
    }
}
