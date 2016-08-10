namespace NSUtils
{
    public class ValidationAttributeLess : ValidateAttribute
    {
        public ValidationAttributeLess(string realName, float minValue, bool equalsOrNot = false, bool showOnlyRequired = false, string literateToValidate = "") : base(realName)
        {
            EqualsOrNot = equalsOrNot;
            MinValue = minValue;
            _literaToValidate = literateToValidate;
            ShowOnlyRequired = showOnlyRequired;
        }

        public double MinValue { get; set; }
        public bool EqualsOrNot { get; set; }
        public bool ShowOnlyRequired { get; set; }
        private string _literaToValidate;


        public override string LiteralValidationError
        {
            get
            {
                if(ShowOnlyRequired == true)
                {
                    return string.Empty;
                }
                else if(string.IsNullOrEmpty(_literaToValidate))
                {
                    return string.Format("Default_Literal_Less", RealName, MinValue);
                }
                else
                {
                    return _literaToValidate;
                }

            }
        }
    }
}
