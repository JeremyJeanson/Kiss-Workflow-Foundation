using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.Metadata;
using System.ComponentModel;

namespace MyActivities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();

            builder.AddCustomAttributes(typeof(ActivityWithActivities),
                new DesignerAttribute(typeof(ActivityWithActivitiesDesigner)));
            builder.AddCustomAttributes(typeof(ActivityWithActivity),
                new DesignerAttribute(typeof(ActivityWithActivityDesigner)));
            builder.AddCustomAttributes(typeof(ActivityWithArgument),
                new DesignerAttribute(typeof(ActivityWithArgumentDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
