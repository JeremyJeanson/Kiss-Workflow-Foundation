using System;
using System.Activities.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demos.Persitance.Extensions
{
    public sealed class MyExtension : PersistenceParticipant
    {
        #region Declarations

        private const String NamespaceName = "http://MyExtension";
        private const String NameName = "Name";
        private const String IdName = "Id";
        private const String DateName = "Date";
        public static readonly XNamespace ExtensionNamespace;

        private String _name;
        private Guid _id;
        private DateTime _date;

        #endregion

        #region Consrtructors

        static MyExtension()
        {
            ExtensionNamespace = XNamespace.Get(NamespaceName);
        }

        #endregion

        #region Properties

        public String Name {get { return _name; }set { _name = value; }}
        public Guid Id { get { return _id; } set { _id = value; } }
        public DateTime Date { get { return _date; } set { _date = value; } }

        #endregion

        #region PersistenceParticipant

        /// <summary>
        /// Write inner values to persitance store
        /// </summary>
        /// <param name="readWriteValues"></param>
        /// <param name="writeOnlyValues"></param>
        protected override void CollectValues(out IDictionary<System.Xml.Linq.XName, object> readWriteValues, out IDictionary<System.Xml.Linq.XName, object> writeOnlyValues)
        {
            readWriteValues = new Dictionary<XName, object>
                {
                    {ExtensionNamespace.GetName(NameName), _name},
                    {ExtensionNamespace.GetName(IdName), _id},
                    {ExtensionNamespace.GetName(DateName), _date}
                };
            writeOnlyValues = null;
        }

        /// <summary>
        /// Read inner values from persitance store
        /// </summary>
        /// <param name="readWriteValues"></param>
        protected override void PublishValues(IDictionary<System.Xml.Linq.XName, object> readWriteValues)
        {
            Object value;

            if (readWriteValues.TryGetValue(ExtensionNamespace.GetName(NameName), out value)
                && value != null)
                _name = value.ToString();

            if (readWriteValues.TryGetValue(ExtensionNamespace.GetName(IdName), out value)
                && value != null)
                _id = (Guid) value;

            if (readWriteValues.TryGetValue(ExtensionNamespace.GetName(DateName), out value)
                && value != null)
                _date = (DateTime)value;
        } 

        #endregion

        /// <summary>
        /// Return values to promote in SQL Store
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<XName> GetValuesToPromote()
        {
            yield return ExtensionNamespace.GetName(NameName);
            yield return ExtensionNamespace.GetName(IdName);
            yield return ExtensionNamespace.GetName(DateName);
        }

        public override string ToString()
        {
            return "Name=" + _name + ", ID=" + _id.ToString() + ", Date=" + _date.ToString();
        }
    }
}
