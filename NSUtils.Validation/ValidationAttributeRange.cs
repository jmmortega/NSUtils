namespace NSUtils
{
    public class ValidationAttributeRange : ValidateAttribute
    {
        public ValidationAttributeRange(string realName , double minValue , double maxValue , bool lessEqualOrNot = false , bool greatEqualOrNot = false , string literaToValidate = "") : base(realName)
        {
            MinValue = minValue;
            MaxValue = maxValue;

            LessEqualOrNot = lessEqualOrNot;
            GreaterEqualOrNot = greatEqualOrNot;

            _literalToValidate = literaToValidate;
        }

        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public bool LessEqualOrNot { get; set; }
        public bool GreaterEqualOrNot { get; set; }

        private string _literalToValidate;

        public override string LiteralValidationError
        {
            get
            {
                return _literalToValidate;
            }
        }

    }
}
