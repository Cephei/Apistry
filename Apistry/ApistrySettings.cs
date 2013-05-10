namespace Apistry
{
    using System.Collections.Generic;
    using Apistry.Conventions;

    public class ApistrySettings
    {
        private readonly ISet<IRequestBuilderConvention> _RequestBuilderConventions;

        public ApistrySettings()
        {
            _RequestBuilderConventions = new HashSet<IRequestBuilderConvention>();
        }

        public ISet<IRequestBuilderConvention> RequestBuilderConventions
        {
            get { return _RequestBuilderConventions; }
        }
    }
}