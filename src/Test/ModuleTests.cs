using DegreeClassEstimator.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DegreeClassEstimator.Tests
{
    [TestClass]
    public class ModuleTests
    {
        public List<Module> TestModules { get; set; }


        public ModuleTests()
        {
            this.TestModules = ModuleTestData.GetTestData();
        }


        [TestMethod]
        public void Test_ValidModule_Standard_GradeWeightedCredits()
        {
            Module module = this.TestModules.FirstOrDefault(x => x.Code == "P60L2G3C0");
            Assert.AreEqual(60 * 3, module.GradeWeightedCredits);
        }


        [TestMethod]
        public void Test_ValidModule_Standard_FinalWeightedCredits()
        {
            Module module = this.TestModules.FirstOrDefault(x => x.Code == "P30L3G2C0");
            module.DoubleWeight = true;
            Assert.AreEqual((30 * 2) * 2, module.FinalWeightedCredits);
        }


        [TestMethod]
        public void Test_InvalidModule_GradeWeightedCredits()
        {
            Module module = this.TestModules.FirstOrDefault(x => x.Code == "InvalidCredits");
            Assert.AreEqual(-1, module.GradeWeightedCredits);
        }


        [TestMethod]
        public void Test_InvalidModule_FinalWeightedCredits()
        {
            Module module = this.TestModules.FirstOrDefault(x => x.Code == "InvalidCredits");
            Assert.AreEqual(-1, module.FinalWeightedCredits);
        }


        [TestMethod]
        public void Test_Regex()
        {
            Assert.IsTrue(Regex.IsMatch("tu100_P60L2G5C0", Module.ModuleDescriptionCodePattern));
            Assert.IsTrue(Regex.IsMatch("some description_P120L3G1C1", Module.ModuleDescriptionCodePattern));
            
            Assert.IsFalse(Regex.IsMatch("thing_P120L4G1", Module.ModuleDescriptionCodePattern));
            Assert.IsFalse(Regex.IsMatch("thing", Module.ModuleDescriptionCodePattern));

            MatchCollection matches = Regex.Matches("tu100_P60L2G5C0&s104_P30L3G2C1", Module.ModuleDescriptionCodePattern);
            Assert.IsTrue(matches.Count == 2);
        }


        [TestMethod]
        public void Test_GetModules_ValidDescriptionStrings()
        {
            List<Module> modules = Degree.GetModules("tu100_P60L2G5C0&s104_P30L3G1C1");

            Assert.IsTrue(modules.Count() == 2);

            Assert.IsTrue(modules[0].Code == "tu100");
            Assert.IsTrue(modules[0].Credits == 60);
            Assert.IsTrue(modules[0].Level == Level.Two);
            Assert.IsTrue(modules[0].Grade == Grade.Transferred);
            Assert.IsFalse(modules[0].Compulsory);
            
            Assert.IsTrue(modules[1].Code == "s104");
            Assert.IsTrue(modules[1].Credits == 30);
            Assert.IsTrue(modules[1].Level == Level.Three);
            Assert.IsTrue(modules[1].Grade == Grade.Distinction);
            Assert.IsTrue(modules[1].Compulsory);
        }


        [TestMethod]
        public void Test_GetModules_InvalidDescriptionString()
        {
            List<Module> modules = Degree.GetModules("tu100_P60L2G5C0&invalidstring");
            Assert.IsTrue(modules.Count() == 1);
            Assert.IsTrue(modules[0].Code == "tu100");
        }
    }
}
