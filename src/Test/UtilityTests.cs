using DegreeClassEstimator.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void Test_GetEnumDescription()
        {
            Grade grade = Grade.Grade2;
            string enumDescr = grade.GetEnumDescription();
            Assert.IsNotNull(enumDescr);    
        }

        [TestMethod]
        public void Test_GetEnumDescription_NoValue()
        {
            Level level = Level.Two;
            string enumDescr = level.GetEnumDescription();
            Assert.IsNull(enumDescr);
        }

        [TestMethod]
        public void Test_GetEnumDisplayName()
        {
            Grade grade = Grade.Grade2;
            string enumDescr = grade.GetDisplayName();
            Assert.AreEqual(enumDescr, "Grade 2");
        }
    }
}
