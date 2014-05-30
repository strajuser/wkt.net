﻿#region License
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
        }

        [TestMethod]
        public void ToString_Simple()
        {
            var array = new WktArray("String", 1, 2.0);
            Assert.AreEqual(array.ToString(), "\"String\",1,2.0");
        }

        [TestMethod]
        public void ToString_With_Culture()
        {
            throw new NotImplementedException();
        }
    }
}