using System;

namespace NSUtils
{
    public class ValidationResult
    {
        public Type ValidationType { get; set; }
        public string FieldName { get; set; }
        public string LiteralToValidate { get; set; }

        public override string ToString()
        {
            string literal = GetLiteral(ValidationType); 
     
            if(string.IsNullOrEmpty(LiteralToValidate))
            {
                return literal;
            }
            var localLiteralToValidate = LiteralToValidate;

            //Resources don't exits or I call local previously
            if(localLiteralToValidate.Contains("Appinux") == true)
            {
                return string.Format("{0}" + System.Environment.NewLine + "{1}", literal, LiteralToValidate);    
            }
            else
            {
                return string.Format("{0}" + System.Environment.NewLine + "{1}" , literal, localLiteralToValidate);
            }

            
        }

        private string GetLiteral(Type validationType)
        {
            return string.Format("IsRequired", FieldName);                    
        }
    }
}
