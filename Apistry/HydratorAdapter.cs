namespace Apistry
{
    using System;
    using System.Reflection;

    using Foundation.ObjectHydrator.Interfaces;

    using Ploeh.AutoFixture.Kernel;

    public class HydratorAdapter : ISpecimenBuilder
    {
        private readonly IMap map;

        public HydratorAdapter(IMap map)
        {
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            this.map = map;
        }

        public Object Create(Object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;

            if (pi == null)
            {
                return new NoSpecimen(request);
            }

            if ((!map.Match(pi)) || (map.Type != pi.PropertyType))
            {
                return new NoSpecimen(request);
            }

            return map.Mapping(pi).Generate();
        }
    }
}