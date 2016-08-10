namespace NSUtils
{
    public class ValidationAttributeGreater : ValidateAttribute
    {
        public ValidationAttributeGreater(string realName , double maxValue , bool equalsOrNot = false , bool showOnlyRequired = false ,string literalToValidate = "") : base(realName)
        {
            MaxValue = maxValue;
            EqualsOrNot = equalsOrNot;
            _literalToValidate = literalToValidate;
            ShowOnlyRequired = showOnlyRequired;
        }

        public double MaxValue { get; set; }
        public bool EqualsOrNot { get; set; }
        public bool ShowOnlyRequired { get; set; }
        private string _literalToValidate;

        public override string LiteralValidationError
        {
            get
            {
                if(ShowOnlyRequired == true)
                {
                    return string.Empty;
                }
                else if(string.IsNullOrEmpty(_literalToValidate))
                {
                    return string.Format("Default_Literal_Greater", RealName, MaxValue);
                }
                else
                {
                    return _literalToValidate;
                }
                    
            }
        }

    }
}
