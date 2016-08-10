namespace NSUtils
{
    public class ValidationAttributeRequired : ValidateAttribute
    {
        public ValidationAttributeRequired(string realName, string literalValidationError = "") : base (realName)
        {
            _literalValidationError = literalValidationError;
        }

        private string _literalValidationError;
        public override string LiteralValidationError
        {
            get { return _literalValidationError; }            
        }
    }
}
