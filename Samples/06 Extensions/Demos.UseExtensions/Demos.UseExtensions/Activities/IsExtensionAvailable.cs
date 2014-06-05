using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demos.UseExtensions.Extensions;

namespace Demos.UseExtensions.Activities
{
    public sealed class IsExtensionAvailable:CodeActivity<Boolean>
    {
        protected override bool Execute(CodeActivityContext context)
        {
            // try to get an extension
            IMyExtension extension = context.GetExtension<IMyExtension>();
            return extension != null;
        }
    }
}
