using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Common.Helper
{
    public class MappingAttribute : Attribute
    {
        public MappingAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}
