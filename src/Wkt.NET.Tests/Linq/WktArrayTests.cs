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
    public class WktArrayTests
    {
        [TestMethod]
        public void Linq_Simple_Ctor()
        {
            var array = new WktArray("String", 1, 2.1);

            Assert.IsTrue(array.Count == 3);
            Assert.IsTrue(array[0].Value is string);
            Assert.IsTrue(array[1].Value is int);
            Assert.IsTrue(array[2].Value is double);

            array = new WktArray();
            Assert.IsTrue(array.Count == 0);

            array = new WktArray("str");
            Assert.IsTrue(array.Count == 1);

            array = new WktArray(1, 2);
            Assert.IsTrue(array.Count == 2);

            array = new WktArray(new WktArray(1, 2));
            Assert.IsTrue(array.Count == 2);

            array = new WktArray(new []{1, 2}, 3);
            Assert.IsTrue(array.Count == 3);

            array = new WktArray(new[] { 1, 2 }, new[] {"s"});
            Assert.IsTrue(array.Count == 3);

            array = new WktArray(new[] { 1, 2 }, new WktArray(), new[] { "s" });
            Assert.IsTrue(array.Count == 3);

            array = new WktArray(new[] { 1, 2 }, new WktArray(1, 2), new[] { "s" });
            Assert.IsTrue(array.Count == 5);
        }

        [TestMethod]
        public void Linq_Complex_Ctor()
        {
            var array = new WktArray(new WktNode("Key", 1));
            Assert.IsTrue(array.Count == 1);
            Assert.IsTrue(array[0] is WktNode);

            array = new WktArray(1, new WktNode("Key", 1));
            Assert.IsTrue(array.Count == 2);

            array = new WktArray(1, new WktNode("Key", 1, 2));
            Assert.IsTrue(array.Count == 2);

            array = new WktArray(1, new WktNode("Key", 1, 2), new [] { 1, 2 });
            Assert.IsTrue(array.Count == 4);
        }

        [TestMethod]
        public void ToString_Simple()
        {
            var array = new WktArray("String", 1, 2.0);
            Assert.AreEqual(array.ToString(), "\"String\",1,2.0");
        }

        [TestMethod]
        public void KeyIndexer_Single()
        {
            var array = new WktArray("String", 1, 2.0);
            Assert.IsNull(array["KEY"]);

            array = new WktArray(new WktNode("KEY", 1));
            Assert.IsNotNull(array["KEY"]);
            Assert.AreSame(array["KEY"], array[0]);

            array = new WktArray(
                new WktNode("KEY", 1),
                new WktNode("KEY", 2));
            Assert.IsNotNull(array["KEY"]);
            Assert.IsTrue(array["KEY"] is WktArray);
            Assert.IsTrue(((WktArray)array["KEY"]).Count == 2);

            array = new WktArray(
                "str",
                new WktNode("KEY", 1),
                new WktNode("KEY", 2));
            Assert.IsNotNull(array["KEY"]);
            Assert.IsTrue(array["KEY"] is WktArray);
            Assert.IsTrue(((WktArray)array["KEY"]).Count == 2);
            Assert.AreSame(array[1], ((WktArray)array["KEY"])[0]);
        }

        [TestMethod]
        public void ToString_With_Culture()
        {
            throw new NotImplementedException();
        }
    }
}