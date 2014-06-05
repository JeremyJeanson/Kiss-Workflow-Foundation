using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.ConsoleTracking.Extensions
{
    public sealed class TraceTracking : TrackingParticipant
    {
        private readonly TrackingProfile _trackingProfile;

        public TraceTracking()
        {
            _trackingProfile = new TrackingProfile
                {
                    ImplementationVisibility = ImplementationVisibility.All,
                    Name = "TraceTracking",
                    Queries =
                        {
                            new WorkflowInstanceQuery {States = {"*"}},
                            new ActivityStateQuery {States = {"*"}},
                        }
                };
        }

        #region Properties

        /// <summary>
        /// Profile
        /// </summary>
        public override TrackingProfile TrackingProfile
        {
            get { return _trackingProfile; }
            set
            {
                // Not used
            }
        }

        #endregion

        #region Track

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            switch (record.Level)
            {
                case TraceLevel.Error:
                    {
                        Trace.TraceError(FormatMessage(record));
                        break;
                    }
                case TraceLevel.Warning:
                    {
                        Trace.TraceWarning(FormatMessage(record));
                        break;
                    }
                case TraceLevel.Info:
                    {
                        {
                            Trace.TraceInformation(FormatMessage(record));
                            break;
                        }
                    }
                default:
                    {
                        Trace.WriteLine(FormatMessage(record));
                        break;
                    }
            }
        }

        private static String FormatMessage(TrackingRecord record)
        {
            ActivityStateRecord a = record as ActivityStateRecord;

            return record.ToString();
        }

        #endregion
    }
}
