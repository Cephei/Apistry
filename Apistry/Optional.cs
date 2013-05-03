namespace Apistry
{
    using System;

    public class Optional<T>
    {
        private readonly T _OriginalValue;

        private T _Value;

        private Boolean _IsDefault;

        public Optional(T defaultValue)
        {
            Value = _OriginalValue = defaultValue;
        }

        public Boolean IsDefault
        {
            get { return _IsDefault; }
        }

        public T Value
        {
            get { return _Value; }
            set
            {
                _IsDefault = OriginalValue == null || OriginalValue.Equals(value);
                _Value = value;
            }
        }

        public T OriginalValue
        {
            get { return _OriginalValue; }
        }
    }
}