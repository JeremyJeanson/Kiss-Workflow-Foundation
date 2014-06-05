using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demos.Persitance.Extensions;

namespace Demos.Persitance.Activities
{
    public sealed class SetExtensionValues:CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            // Get extension
            MyExtension extension = context.GetExtension<MyExtension>();
            // Test
            if (extension == null) return;

            //Use
            extension.Name = "Set in workflow";
            extension.Id = Guid.NewGuid();
            extension.Date = DateTime.Now;
        }
    }
}
