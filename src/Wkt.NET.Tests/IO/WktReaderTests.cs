using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wkt.NET.Enum;
using Wkt.NET.IO;

namespace Wkt.NET.Tests.IO
{
    [TestClass]
    public class WktReaderTests
    {
        [TestMethod]
        public void ParseInt32()
        {
            var reader = new WktReaderMock();

            reader.SetValue("0");
            Assert.IsTrue(reader.Value is int);
            Assert.AreEqual(reader.Value, 0);

            reader.SetValue("1");
            Assert.IsTrue(reader.Value is int);
            Assert.AreEqual(reader.Value, 1);

            reader.SetValue("+1");
            Assert.IsTrue(reader.Value is int);
            Assert.AreEqual(reader.Value, 1);

            reader.SetValue("-1");
            Assert.IsTrue(reader.Value is int);
            Assert.AreEqual(reader.Value, -1);

            reader.SetValue(int.MaxValue.ToString(CultureInfo.InvariantCulture));
            Assert.IsTrue(reader.Value is int);
            Assert.AreEqual(reader.Value, int.MaxValue);

            reader.SetValue(int.MinValue.ToString(CultureInfo.InvariantCulture));
            Assert.IsTrue(reader.Value is int);
            Assert.AreEqual(reader.Value, int.MinValue);
        }

        [TestMethod]
        public void ParseDouble()
        {
            var reader = new WktReaderMock();

            reader.SetValue("0.0");
            Assert.IsTrue(reader.Value is double);
            Assert.AreEqual(reader.Value, 0.0);

            reader.SetValue("1.234");
            Assert.IsTrue(reader.Value is double);
            Assert.AreEqual(reader.Value, 1.234);

            reader.SetValue("-1.234");
            Assert.IsTrue(reader.Value is double);
            Assert.AreEqual(reader.Value, -1.234);

            reader.SetValue("+1.234E+02");
            Assert.IsTrue(reader.Value is double);
            Assert.AreEqual(reader.Value, +1.234E+02);

            reader.SetValue("-1.234E+02");
            Assert.IsTrue(reader.Value is double);
            Assert.AreEqual(reader.Value, -1.234E+02);

            reader.SetValue(double.MaxValue.ToString("r", CultureInfo.InvariantCulture));
            Assert.IsTrue(reader.Value is double);
            Assert.IsTrue(Math.Abs((double)reader.Value - double.MaxValue) < double.Epsilon);

            reader.SetValue(double.MinValue.ToString("r", CultureInfo.InvariantCulture));
            Assert.IsTrue(reader.Value is double);
            Assert.IsTrue(Math.Abs((double)reader.Value - double.MinValue) < double.Epsilon);
        }

        /// <summary>
        /// Helper mock class for testing WktReader
        /// </summary>
        private class WktReaderMock : WktReader
        {
            public override bool Read()
            {
                return true;
            }

            public void SetValue(string value)
            {
                SetState(ReaderState.Value, value);
            }
        }
    }
}