using System;

namespace NSUtils
{
    public abstract class ValidateAttribute : Attribute
    {
        public ValidateAttribute(string realName)
        {
            RealName = realName;
        }

        public string RealName { get; private set; }

        public virtual string LiteralValidationError 
        {
            get
            {
                throw new NotImplementedException();
            }
        }
            
    }
}
