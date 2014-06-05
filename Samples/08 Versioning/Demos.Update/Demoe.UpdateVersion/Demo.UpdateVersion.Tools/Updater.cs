using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Activities.DynamicUpdate;
using System.Activities.Statements;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Demo.UpdateVersion.Tools
{
    public static class Updater
    {
        #region Load/unload definitions

        /// <summary>
        /// Load Workflow definition from file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static ActivityBuilder LoadActivityBuilder(String fileName)
        {
            ActivityBuilder builder;
            using (var xamlReader = new XamlXmlReader(fileName))
            {
                var builderReader = ActivityXamlServices.CreateBuilderReader(xamlReader);
                builder = (ActivityBuilder) XamlServices.Load(builderReader);
                xamlReader.Close();
            }
            return builder;
        }

        /// <summary>
        /// Save workflow definition to file
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        private static void SaveActivityBuilder(ActivityBuilder builder, String path)
        {
            using (var writer = File.CreateText(path))
            {
                var xmlWriter = new XamlXmlWriter(writer, new XamlSchemaContext());
                using (var xamlWriter = ActivityXamlServices.CreateBuilderWriter(xmlWriter))
                {
                    XamlServices.Save(xamlWriter, builder);
                }
            }
        }

        #endregion

        /// <summary>
        /// Mark workflow definition file to updtae it latter
        /// </summary>
        /// <param name="fileName"></param>
        public static void MarkToUpdate(String fileName)
        {
            ActivityBuilder wf = LoadActivityBuilder(fileName);
            DynamicUpdateServices.PrepareForUpdate(wf);
            SaveActivityBuilder(wf, fileName);
        }

        /// <summary>
        /// Load update map from updated defintion and save it to file *.map
        /// </summary>
        /// <param name="fileName"></param>
        public static void SaveUpdateMap(String fileName)
        {
            ActivityBuilder wf= LoadActivityBuilder(fileName);
            DynamicUpdateMap map = DynamicUpdateServices.CreateUpdateMap(wf);

            String path = System.IO.Path.ChangeExtension(fileName, "map");
            DataContractSerializer serialize = new DataContractSerializer(typeof(DynamicUpdateMap));
            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                serialize.WriteObject(fs, map);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="definitionPath"></param>
        /// <param name="mapPath"></param>
        /// <param name="connectionString"></param>
        /// <param name="instanceId"></param>
        public static void Update(String definitionPath,String mapPath, String connectionString,Guid instanceId)
        {
            // Store
            SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore
                {
                    ConnectionString = connectionString
                };

            // Instance to update
            WorkflowApplicationInstance wfInstance = WorkflowApplication.GetInstance(instanceId, instanceStore);

            // Load update map
            DynamicUpdateMap updateMap;
            DataContractSerializer s = new DataContractSerializer(typeof(DynamicUpdateMap));
            using (FileStream fs = File.Open(mapPath, FileMode.Open))
            {
                updateMap = s.ReadObject(fs) as DynamicUpdateMap;
            }
            // if (updateMap == null) return; // Throw

            WorkflowApplication wfApp =
                new WorkflowApplication(LoadActivityBuilder(definitionPath).Implementation);
            
            IList<ActivityBlockingUpdate> act;
            if (wfInstance.CanApplyUpdate(updateMap, out act))
            {
                wfApp.Load(wfInstance, updateMap);
                wfApp.Unload();
            }
        }
    }
}
