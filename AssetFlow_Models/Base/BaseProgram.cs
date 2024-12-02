using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace FAMS_Models.Utilities
{
    public class BaseProgram
    {
        private const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;


        public static string GetResourceValue(string key)
        {
            string resString = "";

            try
            {
                ResourceManager myManager = new ResourceManager(typeof(Resources.Resources));
                resString = myManager.GetString(key);
            }
            catch (Exception ex)
            {

            }

            return resString;
        }

        public static Dictionary<string, string> GetFieldValuesAsDict(Type type)
        {
            return type
                      .GetFields(BindingFlags.Public | BindingFlags.Static)
                      .Where(f => f.FieldType == typeof(string))
                      .ToDictionary(f => f.Name,
                                    f => (string)f.GetValue(null));
        }

        public static List<string> GetFieldValuesAsList(Type type)
        {
            Dictionary<string, string> fields = GetFieldValuesAsDict(type);
            List<string> list = fields.Select(r => r.Value).ToList();
            return list;
        }


        public static string GetDisplayNameValue(string key)
        {
            string resString = "";

            try
            {
                ResourceManager myManager = new ResourceManager(typeof(Resources.DisplayNames));
                resString = myManager.GetString(key);
            }
            catch (Exception ex)
            {

            }

            return resString;
        }
        public static void CopyProperties(Type fromType, object from, Type toType, object to)
        {
            if (fromType == null)
                throw new ArgumentNullException("fromType", "The type that you are copying from cannot be null");

            if (from == null)
                throw new ArgumentNullException("from", "The object you are copying from cannot be null");

            if (toType == null)
                throw new ArgumentNullException("toType", "The type that you are copying to cannot be null");

            if (to == null)
                throw new ArgumentNullException("to", "The object you are copying to cannot be null");

            // Don't copy if they are the same object
            if (!ReferenceEquals(from, to))
            {
                // Get all of the public properties in the toType with getters and setters
                Dictionary<string, PropertyInfo> toProperties = new Dictionary<string, PropertyInfo>();
                PropertyInfo[] properties = toType.GetProperties(flags);
                foreach (PropertyInfo property in properties)
                {
                    toProperties.Add(property.Name, property);
                }

                // Now get all of the public properties in the fromType with getters and setters
                properties = fromType.GetProperties(flags);
                foreach (PropertyInfo fromProperty in properties)
                {
                    // If a property matches in name and type, copy across
                    if (toProperties.ContainsKey(fromProperty.Name))
                    {
                        PropertyInfo toProperty = toProperties[fromProperty.Name];
                        if (toProperty.PropertyType == fromProperty.PropertyType)
                        {
                            object value = fromProperty.GetValue(from, null);
                            toProperty.SetValue(to, value, null);
                            if (toProperty.GetValue(to) == null)
                            {
                                if (toProperty.Name.Contains(""))
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        public static void CompareProperties(Type fromType, object from, Type toType, object to, out List<DataComparer> compareResult, List<String> fieldException = null)
        {
            compareResult = new List<DataComparer>();
            if (fromType == null)
                throw new ArgumentNullException("fromType", "The type that you are copying from cannot be null");
            if (from == null)
                throw new ArgumentNullException("from", "The object you are copying from cannot be null");
            if (toType == null)
                throw new ArgumentNullException("toType", "The type that you are copying to cannot be null");
            if (to == null)
                throw new ArgumentNullException("to", "The object you are copying to cannot be null"); // Don't copy if they are the same object
            if (!ReferenceEquals(from, to))
            {
                // Get all of the public properties in the toType with getters and setters
                Dictionary<string, PropertyInfo> toProperties = new Dictionary<string, PropertyInfo>();
                PropertyInfo[] properties = toType.GetProperties(flags);
                foreach (PropertyInfo property in properties)
                {
                    toProperties.Add(property.Name, property);
                } // Now get all of the public properties in the fromType with getters and setters
                properties = fromType.GetProperties(flags);
                foreach (PropertyInfo fromProperty in properties)
                {
                    // If a property matches in name and type, copy across
                    if (toProperties.ContainsKey(fromProperty.Name))
                    {
                        PropertyInfo toProperty = toProperties[fromProperty.Name];
                        var attributes = toProperty.GetCustomAttributes(false);
                        var columnMapping = attributes.FirstOrDefault(a => a.GetType() == typeof(DisplayAttribute));
                        if (toProperty.PropertyType == fromProperty.PropertyType)
                        {
                            object fromValue = fromProperty.GetValue(from, null);
                            object toValue = toProperty.GetValue(to, null); string fromString = fromValue == null ? "" : fromValue.ToString();
                            string toString = toValue == null ? "" : toValue.ToString();
                            string fieldName = fromProperty.Name;
                            bool fieldExc = false;
                            if (fieldException != null)
                            {
                                for (int i = 0; i < fieldException.Count; i++)
                                {
                                    if (fieldException[i] == fieldName)
                                    {
                                        fieldExc = true;
                                        break;
                                    }
                                }
                            }
                            if (fromString != toString && !fieldExc)
                            {
                                fieldName = ((DisplayAttribute)columnMapping) == null ? fieldName : ((DisplayAttribute)columnMapping).Name;
                                compareResult.Add(new DataComparer()
                                {
                                    field = fieldName,
                                    from = fromString,
                                    to = toString
                                });
                            }
                        }
                    }
                }
            }
        }

        public static string CreateRoutingKey(string module,string action)
        {
            return $"{module}.{action}";
        }

        public static string EscapeSymbols(string filter)
        {
            return filter.Replace("[", "\\[").Replace("]", "\\]").Replace("-", "\\-");
        }
    }

    public class IndexParams
    {
        public int start { get; set; }

        public int pageSize { get; set; }

        public List<FilterClass> filters { get; set; }

        public List<SortClass> sorts { get; set; }

        public string category { get; set; }
        public string type { get; set; }
    }

    public class FilterClass
    {
        public string field { get; set; }
        public string value { get; set; }
        public string matchMode { get; set; }
    }

    public class SortClass
    {
        public string field { get; set; }
        public string order { get; set; }
    }

    public class DataComparer
    {
        public string field { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }
}
