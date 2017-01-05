using System;
using System.Reflection;

namespace NSUtils.Database.Attributes
{
    public class DatabaseTableAttribute : Attribute
    {
        public DatabaseTableAttribute()
        {
            Name = string.Empty;
        }

        public string Name { get; set; }
    }
}
