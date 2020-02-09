using System;

namespace Mytheme.Data.Dto
{
    public class Option : IComparable<Option>
    {
        public Guid Id { get; set; }
        public string Label { get; set; }

        public int CompareTo(Option other)
        {
            return string.Compare(other.Label, Label, StringComparison.Ordinal);
        }
    }
}
