using System;
using System.Activities.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Demos.VersioningService.Extensions
{
    /// <summary>
    /// Extension to store id as promoted participant
    /// </summary>
    public sealed class IdParticipant : PersistenceParticipant
    {
        #region Déclarations

        private const String NamspaceName = "http://IdParticipent";
        private const String IdName = "Id";
        private static readonly XName IdXName;

        private Guid _id;

        #endregion

        #region Constructeur

        /// <summary>
        /// Static constructor
        /// </summary>
        static IdParticipant()
        {
            IdXName = XName.Get(IdName, NamspaceName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get { return _id; } set { _id = value; } }

        #endregion

        #region PersistenceParticipant

        protected override void CollectValues(out IDictionary<System.Xml.Linq.XName, object> readWriteValues, out IDictionary<System.Xml.Linq.XName, object> writeOnlyValues)
        {
            readWriteValues = new Dictionary<XName, Object>(){
                { IdXName, _id}
            };
            writeOnlyValues = null;
        }

        protected override void PublishValues(IDictionary<System.Xml.Linq.XName, object> readWriteValues)
        {
            Object value;
            if (readWriteValues.TryGetValue(IdXName, out value)
                && value != null)
            {
                _id = (Guid)value;
            }                
        }

        public static IEnumerable<XName> GetPropotionsParticipants()
        {
            yield return IdXName;
        }

        #endregion
    }
}