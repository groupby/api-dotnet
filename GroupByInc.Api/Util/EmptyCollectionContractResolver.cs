using System;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GroupByInc.Api.Util
{
    internal class EmptyCollectionContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            Predicate<object> shouldSerialize = property.ShouldSerialize;
            property.ShouldSerialize =
                obj => (shouldSerialize == null || shouldSerialize(obj)) && !IsEmptyCollection(property, obj);
            return property;
        }

        private bool IsEmptyCollection(JsonProperty property, object target)
        {
            object value = property.ValueProvider.GetValue(target);
            ICollection collection = value as ICollection;
            if (collection != null && collection.Count == 0)
                return true;

            if (!typeof (IEnumerable).IsAssignableFrom(property.PropertyType))
                return false;

            PropertyInfo countProp = property.PropertyType.GetProperty("Count");
            if (countProp == null)
                return false;

            int count = (int) countProp.GetValue(value, null);
            return count == 0;
        }
    }
}