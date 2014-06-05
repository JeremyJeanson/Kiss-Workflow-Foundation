using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Diagnostics;
using System.IO;

namespace UseBookmarks.Activities
{
    public sealed class BookmarkActivity : NativeActivity<Object>
    {
        public const String BookmarkName = "MonBookmark";

        protected override void Execute(NativeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            context.CreateBookmark(BookmarkName, BookmarkCallBack);
        }

        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }

        private void BookmarkCallBack(NativeActivityContext context, Bookmark bookmark, object value)
        {
            var writer = context.GetExtension<TextWriter>() ?? Console.Out;
            writer.WriteLine("Bookmark resumed = " + bookmark.Name + " With value = " + value);
            context.SetValue(Result, value);
        }
    }
}
