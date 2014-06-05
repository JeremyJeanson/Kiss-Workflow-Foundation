using System;
using System.Activities;
using System.Activities.Debugger;
using System.Activities.Presentation;
using System.Activities.Presentation.Debug;
using System.Activities.Presentation.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RehostingLib.Extensions
{
    internal sealed class VisualMapping
    {
        #region Declarations

        private readonly WorkflowDesigner _designer;

        #endregion

        #region Contructors

        public VisualMapping(WorkflowDesigner designer)
        {
            _designer = designer;
            _designer.Flush();
        }

        #endregion

        #region Properties
        #endregion

        #region Methodes

        public static void GetMaps(WorkflowDesigner designer, out Dictionary<object, SourceLocation> wfElementToSourceLocationMap, out Dictionary<string, Activity> activityIdToWfElementMap)
        {
            VisualMapping mapping = new VisualMapping(designer);

            //Mapping between the Object and Line No.
            wfElementToSourceLocationMap = mapping.UpdateSourceLocationMappingInDebuggerService();

            //Mapping between the Object and the Instance Id
            activityIdToWfElementMap = mapping.BuildActivityIdToWfElementMap(wfElementToSourceLocationMap);
        }

        #endregion

        #region Code répris du sample "VisualWorkflowTracking"

        Object GetRootInstance()
        {
            ModelService modelService = _designer.Context.Services.GetService<ModelService>();
            if (modelService != null)
            {
                return modelService.Root.GetCurrentValue();
            }
            else
            {
                return null;
            }
        }

        // Get root WorkflowElement.  Currently only handle when the object is ActivitySchemaType or WorkflowElement.
        // May return null if it does not know how to get the root activity.
        Activity GetRootWorkflowElement(object rootModelObject)
        {
            System.Diagnostics.Debug.Assert(rootModelObject != null, "Cannot pass null as rootModelObject");

            Activity rootWorkflowElement;
            IDebuggableWorkflowTree debuggableWorkflowTree = rootModelObject as IDebuggableWorkflowTree;
            if (debuggableWorkflowTree != null)
            {
                rootWorkflowElement = debuggableWorkflowTree.GetWorkflowRoot();
            }
            else // Loose xaml case.
            {
                rootWorkflowElement = rootModelObject as Activity;
            }
            return rootWorkflowElement;
        }

        Activity GetRootRuntimeWorkflowElement()
        {
            Activity root = XamlHelper.GetActivity(_designer.Text);
            WorkflowInspectionServices.CacheMetadata(root);

            IEnumerator<Activity> enumerator1 = WorkflowInspectionServices.GetActivities(root).GetEnumerator();
            //Get the first child of the x:class
            enumerator1.MoveNext();
            root = enumerator1.Current;
            return root;
        }

        Dictionary<object, SourceLocation> UpdateSourceLocationMappingInDebuggerService()
        {
            object rootInstance = GetRootInstance();
            Dictionary<object, SourceLocation> sourceLocationMapping = new Dictionary<object, SourceLocation>();
            Dictionary<object, SourceLocation> designerSourceLocationMapping = new Dictionary<object, SourceLocation>();

            if (rootInstance != null)
            {
                Activity documentRootElement = GetRootWorkflowElement(rootInstance);
                SourceLocationProvider.CollectMapping(GetRootRuntimeWorkflowElement(), documentRootElement, sourceLocationMapping,
                    _designer.Context.Items.GetValue<WorkflowFileItem>().LoadedFile);
                SourceLocationProvider.CollectMapping(documentRootElement, documentRootElement, designerSourceLocationMapping,
                   _designer.Context.Items.GetValue<WorkflowFileItem>().LoadedFile);

            }

            // Notify the DebuggerService of the new sourceLocationMapping.
            // When rootInstance == null, it'll just reset the mapping.
            //DebuggerService debuggerService = debuggerService as DebuggerService;
            var debugService = _designer.DebugManagerView;
            if (debugService != null)
            {
                ((DebuggerService)debugService).UpdateSourceLocations(designerSourceLocationMapping);
            }

            return sourceLocationMapping;
        }

        Dictionary<string, Activity> BuildActivityIdToWfElementMap(Dictionary<object, SourceLocation> wfElementToSourceLocationMap)
        {
            Dictionary<string, Activity> map = new Dictionary<string, Activity>();

            Activity wfElement;
            foreach (object instance in wfElementToSourceLocationMap.Keys)
            {
                wfElement = instance as Activity;
                if (wfElement != null)
                {
                    map.Add(wfElement.Id, wfElement);
                }
            }

            return map;
        }

        #endregion
    }
}
