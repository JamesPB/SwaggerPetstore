using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwaggerPetstore
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestNull()
        {
            Pet pet = new Pet();

            Assert.IsNull(pet.category);
            Assert.IsNull(pet.name);
        }

        [TestMethod]
        public void TestNullDataFix()
        {
            Pet pet = new Pet();

            Program.FixPetData(pet);

            Assert.IsNotNull(pet.category);
            Assert.Equals("Category Missing", pet.category.name);

            Assert.IsNotNull(pet.name);
            Assert.Equals("Name Missing", pet.name);
        }
    }
}
