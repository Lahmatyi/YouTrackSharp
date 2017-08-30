using System.Diagnostics;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    [DebuggerDisplay("{Name}: {OldValue} -> {NewValue}")]
    public class ChangeField
    {
        [JsonProperty("color")]
        public YouTrackColor Color;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("newValue")]
        public object[] NewValue;

        [JsonProperty("oldValue")]
        public object[] OldValue;

        [JsonProperty("valueId")]
        public object ValueId;

        public Field OldValueField => new Field
        {
            Name = Name,
            Value = OldValue[0],
            ValueId = ValueId,
            Color = Color
        };

        public Field NewValueField => new Field
        {
            Name = Name,
            Value = NewValue[0],
            ValueId = ValueId,
            Color = Color
        };
    }
}