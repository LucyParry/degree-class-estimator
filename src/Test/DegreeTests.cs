using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DegreeClassEstimator.Model;
using System.IO;
using HonoursClassEstimator.Test;
using System.Linq;

namespace DegreeClassEstimator.Tests
{
    [TestClass]
    public class DegreeTests
    {
        private string _dataPath;
        private List<ClassThresholds> _thresholds;
        private DegreeTestData _testData;
        private TestHelpers _testHelpers;

        [TestInitialize]
        public void TestInitialise()
        {
            _testData = new DegreeTestData();
            _dataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "thresholds.json");
            _testHelpers = new TestHelpers(_dataPath);

            IList<ClassThresholds> thresholdsResult = _testHelpers.GetCreditClassThresholds();
            _thresholds = (List<ClassThresholds>)thresholdsResult;
           
            var mods = ModuleTestData.GetTestData();
        }


        [TestMethod]
        public void Test_Standard_AllModulePoints()
        {
            Degree degree = _testData.OUDocWorkedExample1;
            Assert.AreEqual(240, degree.AllModulePoints);
        }


        [TestMethod]
        public void Test_Classify_OUWorkedExample1()
        {
            var degree = _testData.OUDocWorkedExample1;
            Assert.IsFalse(degree.IsCalculated);

            IClassifier classifier = new StandardClassifier(_thresholds.ToArray());
            degree.Classify(classifier);

            Assert.IsTrue(degree.IsCalculated);
            Assert.AreEqual(960, degree.DegreeWeightedPoints);
            Assert.AreEqual(120, degree.QualityAssuranceWeightedPoints);

            Assert.AreEqual(HonoursClass.LowerSecond, degree.InitialClass);
            Assert.AreEqual(HonoursClass.UpperSecond, degree.QualityAssuranceClass);
            Assert.AreEqual(HonoursClass.LowerSecond, degree.FinalClass);
        }


        [TestMethod]
        public void Test_Classify_OUWorkedExample2()
        {
            var degree = _testData.OUDocWorkedExample2;
            
            IClassifier classifier = new StandardClassifier(_thresholds.ToArray());
            degree.Classify(classifier);

            Assert.AreEqual(750, degree.DegreeWeightedPoints);
            Assert.AreEqual(90, degree.QualityAssuranceWeightedPoints);

            Assert.AreEqual(HonoursClass.UpperSecond, degree.InitialClass);
            Assert.AreEqual(HonoursClass.UpperSecond, degree.QualityAssuranceClass);
            Assert.AreEqual(HonoursClass.UpperSecond, degree.FinalClass);
        }

        [TestMethod]
        public void Test_Classify_WorkedExample3()
        {
            var degree = _testData.WorkedExample3;
            
            IClassifier classifier = new StandardClassifier(_thresholds.ToArray());
            degree.Classify(classifier);

            Assert.AreEqual(930, degree.DegreeWeightedPoints);
            Assert.AreEqual(150, degree.QualityAssuranceWeightedPoints);

            Assert.AreEqual(HonoursClass.LowerSecond, degree.InitialClass);
            Assert.AreEqual(HonoursClass.LowerSecond, degree.QualityAssuranceClass);
            Assert.AreEqual(HonoursClass.LowerSecond, degree.FinalClass);
        }

        [TestMethod]
        public void Test_GetEncodedString()
        {
            var degree = _testData.WorkedExample3;
            string encodedString = degree.GetModuleCodeString();
        }


        [TestMethod]
        public void Test_GetModules_ValidDescriptionStrings()
        {
            List<Module> modules = Degree.GetModules("tu100_P60L2G5C0&s104_P30L3G1C1");

            Assert.IsTrue(modules.Count() == 2);

            Assert.IsTrue(modules[0].Description == "tu100");
            Assert.IsTrue(modules[0].Points == 60);
            Assert.IsTrue(modules[0].Level == Level.Two);
            Assert.IsTrue(modules[0].Grade == Grade.Transferred);
            Assert.IsFalse(modules[0].Compulsary);

            Assert.IsTrue(modules[1].Description == "s104");
            Assert.IsTrue(modules[1].Points == 30);
            Assert.IsTrue(modules[1].Level == Level.Three);
            Assert.IsTrue(modules[1].Grade == Grade.Distinction);
            Assert.IsTrue(modules[1].Compulsary);
        }


        [TestMethod]
        public void Test_GetModules_InvalidDescriptionString()
        {
            List<Module> modules = Degree.GetModules("tu100_P60L2G5C0&invalidstring");
            Assert.IsTrue(modules.Count() == 1);
            Assert.IsTrue(modules[0].Description == "tu100");
        }
    }
}
