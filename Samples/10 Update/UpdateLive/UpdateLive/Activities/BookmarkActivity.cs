using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Diagnostics;

namespace UpdateLive.Activities
{

    public sealed class BookmarkActivity : NativeActivity
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
            Debug.WriteLine("Bookmark resumed = " + bookmark.Name + " With value = " + value);
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            // Rien à faire
        }
    }
}
