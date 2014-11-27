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

            var links = new List<Link>();
            if (json["_links"] != null && json["_links"].HasValues)
            {
                var enumerator = ((JObject) json["_links"]).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var link = JsonConvert.DeserializeObject<Link>(enumerator.Current.Value.ToString());
                    link.Rel = enumerator.Current.Key;
                    links.Add(link);
                }
            }
            resource.Links = new LinkCollection(links);

            // TODO: Read _embedded objects into properties

            return resource;
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
