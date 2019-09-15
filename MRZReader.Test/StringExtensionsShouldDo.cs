using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MRZReader.Core;

namespace MRZReader.Test
{
    [TestClass]
    public class StringExtensionsShouldDo
    {
        [TestMethod]
        public void ConvertToBoolSafelyTest()
        {
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely("<"));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely(null));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely(" "));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely(string.Empty));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely("0"));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely("1"));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely("false"));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely("&lt;"));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely(@"  <field type=""GivenName"">"));
            Assert.IsFalse(StringExtensions.ConvertToBoolSafely("5334013720G8E88120402303286&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;00"));

            Assert.IsTrue(StringExtensions.ConvertToBoolSafely("true"));
        }
        [TestMethod]
        public void ConvertToIntSafelyTest()
        {
            Assert.AreEqual(0, StringExtensions.ConvertToIntSafely("0"));
            Assert.AreEqual(1, StringExtensions.ConvertToIntSafely("1"));
            Assert.AreEqual(533401372, StringExtensions.ConvertToIntSafely("533401372"));
            Assert.AreEqual(-1, StringExtensions.ConvertToIntSafely(""));
            Assert.AreEqual(-1, StringExtensions.ConvertToIntSafely(" "));
            Assert.AreEqual(-1, StringExtensions.ConvertToIntSafely(string.Empty));
            Assert.AreEqual(-1, StringExtensions.ConvertToIntSafely("text1"));
            Assert.AreEqual(-1, StringExtensions.ConvertToIntSafely("&lt;"));
            Assert.AreEqual(-1, StringExtensions.ConvertToIntSafely(@"  <field type=""GivenName"">"));
            Assert.AreEqual(-1, StringExtensions.ConvertToIntSafely("5334013720G8E88120402303286&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;00"));
        }
        [TestMethod]
        public void ConvertToDateTimeSafelyTest()
        {
            Assert.AreEqual(DateTime.Parse("04/12/1988 00:00:00"), StringExtensions.ConvertToDateTimeSafely("881204"));
            Assert.AreEqual(DateTime.Parse("04/12/2005 00:00:00"), StringExtensions.ConvertToDateTimeSafely("051204"));
            Assert.AreEqual(DateTime.Parse("04/12/1968 00:00:00"), StringExtensions.ConvertToDateTimeSafely("681204"));
            Assert.AreEqual(DateTime.Parse("04/12/2011 00:00:00"), StringExtensions.ConvertToDateTimeSafely("111204"));
            Assert.AreEqual(DateTime.Parse("04/12/2000 00:00:00"), StringExtensions.ConvertToDateTimeSafely("001204"));
            Assert.AreEqual(DateTime.Parse("04/12/1999 00:00:00"), StringExtensions.ConvertToDateTimeSafely("991204"));
            Assert.IsNull(StringExtensions.ConvertToDateTimeSafely("995004"));
            Assert.IsNull(StringExtensions.ConvertToDateTimeSafely("990004"));
            Assert.IsNull(StringExtensions.ConvertToDateTimeSafely("990000"));
            Assert.IsNull(StringExtensions.ConvertToDateTimeSafely("5334013720G8E88120402303286&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;&lt;00"));


        }

    }
}
