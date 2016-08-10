namespace NSUtils
{
    public class ValidationAttributeBool : ValidateAttribute
    {
        public ValidationAttributeBool(string realName, string literalValidationError = "") : base (realName)
        {
            _literalValidationError = literalValidationError;
        }

        private string _literalValidationError;
        public override string LiteralValidationError
        {
            get { return _literalValidationError; }            
        }
                
        public bool DefaultValue { get; set; }

    }
}
