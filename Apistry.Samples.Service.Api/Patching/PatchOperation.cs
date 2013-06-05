namespace Apistry.Samples.Service.Api.Patching
{
    using System;
    using System.Linq.Expressions;

    public class PatchOperation<TDto>
    {
        private readonly String _PropertyName;

        private readonly Object _OldValue;

        private readonly Object _NewValue;

        public PatchOperation(String propertyName, Object oldValue, Object newValue)
        {
            _PropertyName = propertyName;
            _OldValue = oldValue;
            _NewValue = newValue;
        }

        public String PropertyName
        {
            get
            {
                return _PropertyName;
            }
        }

        public Object OldValue
        {
            get
            {
                return _OldValue;
            }
        }

        public Object NewValue
        {
            get
            {
                return _NewValue;
            }
        }

        public Boolean HasChangedTo(Object value)
        {
            return NewValue.Equals(value) && !OldValue.Equals(value);
        }

        public Boolean IsProperty<TProperty>(Expression<Func<TDto, TProperty>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression)
                .Member
                .Name
                .Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}