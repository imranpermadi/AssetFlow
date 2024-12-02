using FAMS_Data;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using FAMS_Models.Utilities;

namespace FAMS_API.Utilities
{
    public  static class UUtilsApi
    {

        public static List<Dictionary<string, object>> GetObjectRelations(Type type)
        {
            var relations = new List<Dictionary<string, object>>();

            var metadata = ((IObjectContextAdapter)new DataEntities()).ObjectContext.MetadataWorkspace;

            var fk_all = metadata.GetItems<AssociationType>(DataSpace.CSpace).Where(a => a.IsForeignKey);

            var fk_out = fk_all.Where(x => x.ReferentialConstraints[0].FromRole.Name == type.Name).ToList(); // relations going out 

            foreach (var fk in fk_out)
            {
                var relation = new Dictionary<string, object>();

                var fk_ref = fk.ReferentialConstraints[0]; //How can a foreign key relation have more than one column?

                var objectName = fk_ref.ToRole.Name;

                var attributeName = fk_ref.ToProperties[0].Name;

                relation.Add(objectName, attributeName);

                relations.Add(relation);
            }

            return relations;
        }

        public static bool HasTransaction(Type type, long Id, out string usedTable)
        {
            usedTable = "";
            var detailTable = type.Name + "_Detail";
            var allRelation = UUtilsApi.GetObjectRelations(type);

            foreach (var relation in allRelation)
            {
                var tableName = relation.Keys.FirstOrDefault();
                var columnName = relation.Values.FirstOrDefault();

                if (detailTable != tableName && tableName != "GroupMenu")
                {
                    var query = string.Format("select * from {0} where {1} = {2}", tableName, columnName, Id);
                    DataTable q = new DataTable();
                    string sError = SQL.GetDataTable(query, new Dictionary<string, object>(), out q);

                    if (sError == "OK")
                    {
                        usedTable = tableName;
                        if (q.Rows.Count > 0) return true;
                    }
                }
            }

            return false;
        }
    
    }
}
