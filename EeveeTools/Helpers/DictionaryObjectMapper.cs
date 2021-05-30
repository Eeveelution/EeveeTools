using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EeveeTools.Helpers {
    public static class DictionaryObjectMapper {
        public static void MapObject<pType>(this pType objectToMap, IReadOnlyDictionary<string, object> mappingDictionary) where pType : class, new() {
            PropertyInfo[] properties = typeof(pType).GetProperties();

            foreach (PropertyInfo property in properties) {
                object value = mappingDictionary.GetValueOrDefault(property.Name, null);

                if(value != null)
                    property.SetValue(objectToMap, value);
            }
        }
    }
}
