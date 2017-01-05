using System;

namespace NSUtils.Database.Attributes
{
    public class DatabaseFieldAttribute : Attribute
    {
        public DatabaseFieldAttribute()
        {
            Name = string.Empty;
            Size = 0;
            IsPrimaryKey = false;
            IsUnique = false;
            IsNull = true;
        }

        public string Name { get; set; }

        public int Size { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsUnique { get; set; }

        public bool IsNull { get; set; }        
    }
}
