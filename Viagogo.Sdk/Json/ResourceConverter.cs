using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Viagogo.Sdk.Models;
using Viagogo.Sdk.Resources;

namespace Viagogo.Sdk.Json
{
    public class ResourceConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var json = JToken.ReadFrom(reader);
            var resource = (Resource)JsonConvert.DeserializeObject(json.ToString(), objectType);

            resource.Links = new LinkCollection(DeserializeLinks(json));

            DeserializeAndAssignEmbeddedProperties(json, objectType, ref resource);

            return resource;
        }

        IEnumerable<Link> DeserializeLinks(JToken json)
        {
            if (json["_links"] == null || !json["_links"].HasValues)
            {
                yield break;
            }

            var enumerator = ((JObject)json["_links"]).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var link = JsonConvert.DeserializeObject<Link>(enumerator.Current.Value.ToString());
                link.Rel = enumerator.Current.Key;
                yield return link;
            }
        }

        void DeserializeAndAssignEmbeddedProperties(JToken json, Type objectType, ref Resource resource)
        {
            if (json["_embedded"] == null || !json["_embedded"].HasValues)
            {
                return;
            }

            var embeddedPropertiesMap = new Dictionary<string, PropertyInfo>();
            foreach (var property in objectType.GetTypeInfo().DeclaredProperties)
            {
                var embeddedAttribute = property.GetCustomAttribute<EmbeddedAttribute>(true);
                if (embeddedAttribute == null)
                {
                    // This property doesn't have the EmbeddedAttribute
                    continue;
                }

                embeddedPropertiesMap.Add(embeddedAttribute.Rel, property);
            }

            if (embeddedPropertiesMap.Count == 0)
            {
                // No properties in this object are mapped to anything
                // in _embedded so bail
                return;
            }

            var enumerator = ((JObject)json["_embedded"]).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var rel = enumerator.Current.Key;
                PropertyInfo property;
                if (!embeddedPropertiesMap.TryGetValue(rel, out property))
                {
                    continue;
                }

                var deserializedPropertyValue = JsonConvert.DeserializeObject(
                                                    enumerator.Current.Value.ToString(),
                                                    property.PropertyType,
                                                    new ResourceConverter());
                property.SetValue(resource, deserializedPropertyValue);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Resource).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }
    }
}
