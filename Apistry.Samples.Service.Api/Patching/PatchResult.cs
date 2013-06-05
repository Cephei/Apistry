namespace Apistry.Samples.Service.Api.Patching
{
    using System.Collections.Generic;

    public class PatchResult<TDto>
    {
        private readonly TDto _PatchedObject;

        private readonly IEnumerable<PatchOperation<TDto>> _Operations;

        public PatchResult(TDto patchedObject, IEnumerable<PatchOperation<TDto>> operations)
        {
            _PatchedObject = patchedObject;
            _Operations = operations;
        }

        public TDto PatchedObject
        {
            get
            {
                return _PatchedObject;
            }
        }

        public IEnumerable<PatchOperation<TDto>> Operations
        {
            get
            {
                return _Operations;
            }
        }
    }
}