using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YouTrackSharp.Json;

namespace YouTrackSharp.Issues
{
    [DebuggerDisplay("{Issue}")]
    public class Changes
    {
        [JsonProperty("issue")]
        public Issue Issue { get; set; }
        [JsonProperty("change")]
        public IEnumerable<Change> ChangesCollection { get; set; }
    }
    
    [DebuggerDisplay("{UpdaterName} {Updated}")]
    public class Change
        : DynamicObject
    {
        private readonly IDictionary<string, ChangeField> _fields = new Dictionary<string, ChangeField>(StringComparer.OrdinalIgnoreCase);
        
        [JsonProperty("updaterName")]
        public Field UpdaterName { get; set; }
        
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("updated")]
        public Field Updated { get; set; }
        
        public IEnumerable<ChangeField> ChangedFields => _fields.Values;
        
        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // "field" setter when deserializing JSON into Issue object
            if (string.Equals(binder.Name, "field", StringComparison.OrdinalIgnoreCase) && value is JArray)
            {   
                var fieldElements = ((JArray)value).ToObject<List<ChangeField>>();
                foreach (var fieldElement in fieldElements)
                {
                    /*if (fieldElement.Value is JArray fieldElementAsArray)
                    {
                        // Map collection
                        if (string.Equals(fieldElement.Name, "assignee", StringComparison.OrdinalIgnoreCase))
                        {
                            // For assignees, we can do a strong-typed list.
                            fieldElement.Value = fieldElementAsArray.ToObject<List<Assignee>>();
                        }
                        else
                        {
                            if (fieldElementAsArray.First is JValue &&
                                JTokenTypeUtil.IsSimpleType(fieldElementAsArray.First.Type))
                            {
                                // Map simple arrays to a collection of string
                                fieldElement.Value = fieldElementAsArray.ToObject<List<string>>();
                            }
                            else
                            {
                                // Map more complex arrays to JToken[]
                                fieldElement.Value = fieldElementAsArray;
                            }
                        }
                    }*/
                    
                    // Set the actual field
                    File.AppendAllText("result.txt", $"Field updated {fieldElement.Name}");
                    _fields[fieldElement.Name] = fieldElement;
                }
             
                return true;
            }
            // Regular setter
            /*ChangeField field;
            if (_fields.TryGetValue(binder.Name, out field) || _fields.TryGetValue(binder.Name.Replace("_", " "), out field))
            {
                field.Value = value;
            }
            else
            {
                _fields.Add(binder.Name, new Field { Name = binder.Name, Value = value });
            }*/
            
            return true;
        }
    }
}