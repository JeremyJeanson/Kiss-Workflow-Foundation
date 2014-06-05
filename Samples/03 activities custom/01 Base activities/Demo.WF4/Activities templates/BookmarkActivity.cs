using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Demo.WF4
{

    public sealed class MyValidationActivity : NativeActivity<Boolean>
    {
        protected override void Execute(NativeActivityContext context)
        {
            context.CreateBookmark("ValideMyWorkflow", new BookmarkCallback(this.MyCallBack));
        }

        protected override Boolean CanInduceIdle
        {
            get { return true; }
        }

        private void MyCallBack(NativeActivityContext context, Bookmark bookmark, Object o)
        {
            Boolean value = (Boolean) o;
            this.Result.Set(context, value);
        }
    }
}
