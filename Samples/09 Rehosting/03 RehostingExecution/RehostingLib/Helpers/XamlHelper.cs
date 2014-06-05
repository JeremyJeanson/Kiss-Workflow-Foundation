using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RehostingLib
{
    internal static class XamlHelper
    {
        /// <summary>
        /// Retourne le workflow représenté par la string contenant le xaml
        /// </summary>
        /// <param name="xaml"></param>
        /// <returns></returns>
        internal static Activity GetActivity(String xaml)
        {
            if (!String.IsNullOrEmpty(xaml))
            {
                // Création du XmlReader destiné à permettre la restitution du Workflow
                StringReader stringReader = new StringReader(xaml);
                // XmlReader xmlReader = XmlReader.Create(stringReader);

                try
                {
                    // Création du workflow
                    return ActivityXamlServices.Load(stringReader);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
