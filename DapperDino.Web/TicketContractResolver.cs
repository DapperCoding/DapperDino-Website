using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
namespace DapperDino
{
    public class TicketContractResolver : DefaultContractResolver
    {
        private readonly string[] props;

        public TicketContractResolver(params string[] prop)
        {
            this.props = prop;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {// Create a PropertyDescriptor for "SpouseName" by calling the static GetProperties on TypeDescriptor.
            //try
            //{
            //    var props = TypeDescriptor.GetProperties(type);
            //    if (props != null)
            //    {
            //        PropertyDescriptor descriptor = props.Find(this.props[0], true);
            //        if (descriptor != null)
            //        {
            //            // Fetch the ReadOnlyAttribute from the descriptor. 
            //            JsonIgnoreAttribute attrib = (JsonIgnoreAttribute)descriptor.Attributes[typeof(JsonIgnoreAttribute)];

            //            // Get the internal isReadOnly field from the ReadOnlyAttribute using reflection. 
            //            FieldInfo isIgnored = attrib.GetType().GetField("isIgnored", BindingFlags.NonPublic | BindingFlags.Instance);

            //            // Using Reflection, set the internal isReadOnly field. 
            //            isIgnored.SetValue(attrib, false);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            
            IList<JsonProperty> retval = base.CreateProperties(type, memberSerialization);
            
            


            return retval;
        }
    }
}
