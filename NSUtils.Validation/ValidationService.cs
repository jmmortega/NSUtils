using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSUtils
{
    public class ValidationService
    {
        public List<ValidationResult> Validate(object objectToValidate, List<string> excludeFields = null)
        {
            if(excludeFields == null)
            {
                excludeFields = new List<string>();
            }
            
            var validationResults = new List<ValidationResult>();          
            foreach(var property in objectToValidate.GetType().GetRuntimeProperties().Where(x => x.GetCustomAttributes(typeof(ValidateAttribute), true).Any()))
            {
                var attribute = (ValidateAttribute)property.GetCustomAttributes(typeof(ValidateAttribute), true).FirstOrDefault();

                if (!excludeFields.Contains(attribute.RealName))
                {
                    if (property.GetCustomAttributes(typeof(ValidationAttributeBool), false).Any())
                    {
                        validationResults.Add(ValidateBool(property, objectToValidate));
                    }

                    if (property.GetCustomAttributes(typeof(ValidationAttributeRequired), false).Any())
                    {
                        validationResults.Add(ValidateRequired(property, objectToValidate));
                    }

                    if (property.GetCustomAttributes(typeof(ValidationAttributeRange), false).Any())
                    {
                        validationResults.Add(ValidateRange(property, objectToValidate));
                    }

                    if (property.GetCustomAttributes(typeof(ValidationAttributeGreater), false).Any())
                    {
                        validationResults.Add(ValidationGreater(property, objectToValidate));
                    }

                    if (property.GetCustomAttributes(typeof(ValidationAttributeLess), false).Any())
                    {
                        validationResults.Add(ValidationLess(property, objectToValidate));
                    }

                    if(property.GetCustomAttributes(typeof(ValidationAttributeCheckRequired),false).Any())
                    {
                        validationResults.Add(ValidateCheck(property, objectToValidate));
                    }
                }
            }

            return validationResults.Where(x => x != null).ToList();
        }

        private ValidationResult ValidateBool(PropertyInfo property, object objectToValidate)
        {
            var attr = (ValidationAttributeBool)property.GetCustomAttributes(typeof(ValidationAttributeBool), false).First();
            bool value = (bool)property.GetValue(objectToValidate, null);

            var element = attr.DefaultValue;
                        
            if(value == element)
            {
                return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeBool) , LiteralToValidate = attr.LiteralValidationError};
            }

            return null;
        }

        private ValidationResult ValidateRequired(PropertyInfo property , object objectToValidate)
        {            
            var value = property.GetValue(objectToValidate, null);
            var attr = (ValidationAttributeRequired)property.GetCustomAttributes(typeof(ValidationAttributeRequired), false).First();

            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeRequired), LiteralToValidate = attr.LiteralValidationError };
            }

            return null;
        }

        private ValidationResult ValidateRange(PropertyInfo property , object objectToValidate)
        {
            var attr = (ValidationAttributeRange)property.GetCustomAttributes(typeof(ValidationAttributeRange), false).First();
            double value = Convert.ToDouble(property.GetValue(objectToValidate, null));

            double minValue = attr.MinValue;
            double maxValue = attr.MaxValue;

            if(attr.GreaterEqualOrNot == true && attr.LessEqualOrNot == true)
            {
                if (value <= minValue|| value >= maxValue)
                {
                    return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeRange), LiteralToValidate = attr.LiteralValidationError };
                }
            }
            else if(attr.GreaterEqualOrNot == false && attr.LessEqualOrNot == true)
            {
                if (value <= minValue || value > maxValue)
                {
                    return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeRange), LiteralToValidate = attr.LiteralValidationError };
                }
            }
            else if (attr.GreaterEqualOrNot == true && attr.LessEqualOrNot == false)
            {
                if (value < minValue || value >= maxValue)
                {
                    return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeRange), LiteralToValidate = attr.LiteralValidationError };
                }
            }
            else
            {
                if (value < minValue || value > maxValue)
                {
                    return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeRange), LiteralToValidate = attr.LiteralValidationError };
                }
            }
            return null;
        }

        private ValidationResult ValidationGreater(PropertyInfo property, object objectToValidate)
        {
            var attr = (ValidationAttributeGreater)property.GetCustomAttributes(typeof(ValidationAttributeGreater), false).First();
            double value = Convert.ToDouble(property.GetValue(objectToValidate, null));

            if(attr.EqualsOrNot == true)
            {
                if(value >= attr.MaxValue)
                {
                    return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeGreater), LiteralToValidate = attr.LiteralValidationError };
                }
            }
            else
            {
                if(value > attr.MaxValue)
                {
                    return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeGreater), LiteralToValidate = attr.LiteralValidationError };
                }
            }

            return null;
        }

        private ValidationResult ValidationLess(PropertyInfo property , object objectToValidate)
        {
            var attr = (ValidationAttributeLess)property.GetCustomAttributes(typeof(ValidationAttributeLess), false).First();
            double value = Convert.ToDouble(property.GetValue(objectToValidate, null));

            if (attr.EqualsOrNot == true)
            {
                if (value <= attr.MinValue)
                {
                    return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeLess), LiteralToValidate = attr.LiteralValidationError };
                }
            }
            else
            {
                if (value < attr.MinValue)
                {
                    return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeLess), LiteralToValidate = attr.LiteralValidationError };
                }
            }

            return null;
        }

        private ValidationResult ValidateCheck(PropertyInfo property, object objectToValidate)
        {
            var attr = (ValidationAttributeCheckRequired)property.GetCustomAttributes(typeof(ValidationAttributeCheckRequired), false).First();
            int? value = (int?)property.GetValue(objectToValidate, null);

            if(value == null || value.TryValue() <= -1)
            {
                return new ValidationResult() { FieldName = attr.RealName, ValidationType = typeof(ValidationAttributeCheckRequired), LiteralToValidate = attr.LiteralValidationError };
            }

            return null;
        }
    }
}
