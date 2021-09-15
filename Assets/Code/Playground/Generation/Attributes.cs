using System;

namespace Playground.Generation
{
    public sealed class BlockVariableAttribute : Attribute
    {
    }

    public sealed class BlockMethodAttribute : Attribute
    {
    }

    public sealed class NameAttribute : Attribute
    {
        public string Value { get; private set; }

        public NameAttribute(string value)
        {
            Value = value;
        }
    }

    public sealed class DescriptionAttribute : Attribute
    {
        public string Value { get; private set; }

        public DescriptionAttribute(string value)
        {
            Value = value;
        }
    }
}
