using System;
using System.Activities;
using System.Activities.Debugger;
using System.Activities.Presentation;
using System.Activities.Presentation.Debug;
using System.Activities.Presentation.Services;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace RehostingLib.Extensions
{
    public sealed class VisualTracking : TrackingParticipant
    {
        #region Declarations

        private const String all = "*";

        private readonly WorkflowDesigner _designer;
        private readonly Dictionary<object, SourceLocation> _wfElementToSourceLocationMap;
        private readonly Dictionary<string, Activity> _activityIdToWfElementMap;
        private readonly Boolean _slowTrack;
        private readonly ObservableCollection<String> _tracks;
        private static Object _tracksLock;

        #endregion

        #region Contructors

        static VisualTracking()
        {
            _tracksLock = new Object();
        }

        /// <summary>
        /// Contructeur
        /// </summary>
        /// <param name="designer"></param>
        internal VisualTracking(WorkflowDesigner designer,Boolean slowTrack)
        {
            _designer = designer;
            _slowTrack = slowTrack;
            _tracks = new ObservableCollection<String>();
            BindingOperations.EnableCollectionSynchronization(_tracks, _tracksLock);
            VisualMapping.GetMaps(_designer, out _wfElementToSourceLocationMap, out _activityIdToWfElementMap);
        }

        #endregion

        #region Properties

        /// <summary>
        /// TrackingProfile pour capturer le schangement d'activité
        /// </summary>
        public override TrackingProfile TrackingProfile
        {
            get
            {
                return new TrackingProfile()
                    {
                        Name = "CustomTrackingProfile",
                        Queries = 
                        {
                            new CustomTrackingQuery() 
                            {
                                Name = all,
                                ActivityName = all
                            },
                            new WorkflowInstanceQuery()
                            {
                                // Limit workflow instance tracking records for started and completed workflow states
                                States = { WorkflowInstanceStates.Started, WorkflowInstanceStates.Completed },
                            },
                            new ActivityStateQuery()
                            {
                                // Subscribe for track records from all activities for all states
                                ActivityName = all,
                                States = { all },

                                // Extract workflow variables and arguments as a part of the activity tracking record
                                // VariableName = "*" allows for extraction of all variables in the scope
                                // of the activity
                                Variables = 
                                {                                
                                    { all }   
                                }
                            }   
                        }
                    };
            }
            set
            {
                // Ne s'applique pas ici
            }
        }

        /// <summary>
        /// Tracks
        /// </summary>
        public ObservableCollection<String> Tracks { get { return _tracks; } }

        #endregion

        #region Methodes de tracking

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            _tracks.Add(
                String.Format("Tracking Record Received: {0} with timeout: {1} seconds.", record, timeout.TotalSeconds));

            ActivityStateRecord activityStateRecord = record as ActivityStateRecord;

            if ((activityStateRecord != null) && (!activityStateRecord.Activity.TypeName.Contains("System.Activities.Expressions")))
            {
                if (_activityIdToWfElementMap.ContainsKey(activityStateRecord.Activity.Id))
                {
                    var activity = _activityIdToWfElementMap[activityStateRecord.Activity.Id];

                    _tracks.Add(
                        String.Format("<Activity> Activity Tracking Record Received for ActivityId: {0}, record: {1} ",
                        activity.Id,
                        activityStateRecord
                        )
                    );

                    ShowDebug(_wfElementToSourceLocationMap[activity]);

                    // Test si on doit ralentir le tracking
                    if (_slowTrack)
                    {
                        Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                        {
                            //Textbox Updates
                            //tx.AppendText(activityStateRecord.Activity.DisplayName + " " + activityStateRecord.State + "\n");
                            //tx.AppendText("******************\n");
                            //textLineToSourceLocationMap.Add(i, wfElementToSourceLocationMap[activityStateRecord.Activity]);
                            //i = i + 2;

                            //Add a sleep so that the debug adornments are visible to the user
                            System.Threading.Thread.Sleep(1000);
                        }));
                    }
                }

            }
            else
            {
                // TrackingRecordReceived(this, new TrackingEventArgs(record, timeout, null));
            }
        }

        private void ShowDebug(SourceLocation srcLoc)
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, new Action(() =>
                    _designer.DebugManagerView.CurrentLocation = srcLoc));
        }

        #endregion

        #region Methodes
        #endregion
    }
}
