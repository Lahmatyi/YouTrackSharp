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
        public ICollection<Change> ChangesCollection { get; set; }
    }
    
    [DebuggerDisplay("{UpdaterName} {Updated}")]
    public class Change
        : DynamicObject
    {
        private readonly IDictionary<string, Field> _fields = new Dictionary<string, Field>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, ChangeField> _changeFields = new Dictionary<string, ChangeField>(StringComparer.OrdinalIgnoreCase);

        public string UpdaterName
        {
            get
            {
                var field = GetField("updaterName");
                return field?.Value.ToString();
            }
        }

        public DateTime Updated
        {
            get
            {
                var field = GetField("updated");
                return field.AsDateTime();
            }
        }

        public IEnumerable<Field> Fields => _fields.Values;
        public IEnumerable<ChangeField> ChangedFields => _changeFields.Values;

        public Field GetField(string fieldName)
        {
            Field field;
            _fields.TryGetValue(fieldName, out field);
            return field;
        }

        public ChangeField GetChangeField(string fieldName)
        {
            ChangeField field;
            _changeFields.TryGetValue(fieldName, out field);
            return field;
        }

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // "field" setter when deserializing JSON into Issue object
            if (string.Equals(binder.Name, "field", StringComparison.OrdinalIgnoreCase) && value is JArray)
            {   
                var changeFieldElements = ((JArray)value).ToObject<List<ChangeField>>();
                foreach (var fieldElement in changeFieldElements)
                {
                    //if(string.IsNullOrEmpty(fieldElement.NewValueField.AsString()) && string.IsNullOrEmpty(fieldElement.OldValueField.AsString()))
                    if(IsArrayEmpty(fieldElement.NewValue) && IsArrayEmpty(fieldElement.OldValue))
                        continue;
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
                    //File.AppendAllText("result.txt", $"Field updated {fieldElement.Name}");
                    _changeFields[fieldElement.Name] = fieldElement;
                }

                var fieldElements = ((JArray)value).ToObject<List<Field>>();
                foreach (var fieldElement in fieldElements)
                {
                    //if(string.IsNullOrEmpty(fieldElement.NewValueField.AsString()) && string.IsNullOrEmpty(fieldElement.OldValueField.AsString()))
                    if (fieldElement.Value == null)
                        continue;
                    _fields[fieldElement.Name] = fieldElement;
                }
                return true;
            }


            // Regular setter
            /*ChangeField field;
            if (_changeFields.TryGetValue(binder.Name, out field) || _changeFields.TryGetValue(binder.Name.Replace("_", " "), out field))
            {
                field.Value = value;
            }
            else
            {
                _changeFields.Add(binder.Name, new Field { Name = binder.Name, Value = value });
            }*/
            
            return true;
        }

        private bool IsArrayEmpty(object[] array)
        {
            return array == null || array.Length == 0;
        }
    }
}