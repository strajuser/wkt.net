#region License
// Copyright (c) 2014 Dmitri Strajnikov
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wkt.NET.Linq;

namespace Wkt.NET.Tests.Linq
{
    [TestClass]
    public class WktValueTests
    {
        public enum DefaultEnum { VALUE, Value  };

        [TestMethod]
        public void Linq_Simple_Ctor()
        {
            var value = new WktValue("str");
            Assert.IsTrue(value.Value is string);
            Assert.AreEqual(value.Value, "str");

            value = new WktValue(1);
            Assert.IsTrue(value.Value is int);
            Assert.AreEqual(value.Value, 1);

            value = new WktValue(2.0);
            Assert.IsTrue(value.Value is double);
            Assert.AreEqual(value.Value, 2.0);
        }

        [TestMethod]
        public void ToString_DefaultFormats()
        {
            var val = new WktValue(1);
            Assert.AreEqual(val.ToString(), "1");

            val = new WktValue(2.0);
            Assert.AreEqual(val.ToString(), "2.0");

            val = new WktValue((decimal)3.0);
            Assert.AreEqual(val.ToString(), "3.0");

            val = new WktValue((float)4.0);
            Assert.AreEqual(val.ToString(), "4.0");

            val = new WktValue(5.123456789);
            Assert.AreEqual(val.ToString(), "5.123456789");

            val = new WktValue(5.123456789012345678901234567890);
            // NOTE: Support only 24 floating point numbers
            Assert.AreEqual(val.ToString(), "5.12345678901235");

            val = new WktValue("str");
            Assert.AreEqual(val.ToString(), "\"str\"");

            val = new WktValue(DefaultEnum.VALUE);
            Assert.AreEqual(val.ToString(), "VALUE");

            val = new WktValue(DefaultEnum.Value);
            Assert.AreEqual(val.ToString(), "Value");
        }

        //[TestMethod]
        public void ToString_With_Culture()
        {
            throw new NotImplementedException();
        }
    }
}