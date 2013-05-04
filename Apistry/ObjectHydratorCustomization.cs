namespace Apistry
{
    using System.Linq;
    using Foundation.ObjectHydrator;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    public class ObjectHydratorCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var builders = new DefaultTypeMap().Select(map => new HydratorAdapter(map));

            fixture.Customizations.Add(new CompositeSpecimenBuilder(builders));
        }
    }
}