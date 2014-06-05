using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demos.Persitance.Extensions;

namespace Demos.Persitance.Activities
{
    public sealed class GetExtensionValues : CodeActivity<MyExtension>
    {
        protected override MyExtension Execute(CodeActivityContext context)
        {
            // Get extension
            MyExtension extension = context.GetExtension<MyExtension>();
            
            return extension;
        }
    }
}
