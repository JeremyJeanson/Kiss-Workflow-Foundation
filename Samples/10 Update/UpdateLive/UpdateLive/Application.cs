using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpdateLive.Activities;

namespace UpdateLive
{
    public sealed class Application
    {
        private AutoResetEvent _waitIdle;
        private readonly WorkflowApplication _host;

        public Application(Activity definition)
        {
            _host = new WorkflowApplication(definition);
        }

        public void Run()
        {
            _waitIdle = new AutoResetEvent(false);
            _host.Idle = e=>_waitIdle.Set();
            _host.Run();
            _waitIdle.WaitOne();
        }

        public WorkflowApplication Host { get { return _host; } }

        public void Resume(Object value)
        {
            _host.ResumeBookmark(BookmarkActivity.BookmarkName, value);
        }
    }
}
