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
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wkt.NET.Linq;

namespace Wkt.NET.Tests
{
    [TestClass]
    public class PerformanceTests
    {
        [TestMethod]
        public void SimpleArray()
        {
            var array = new double[1000];
            const int iterations = 1000;

            var data = new WktArray(array).ToString();

            CheckPerformance("Array", () =>
            {
                for (int i = 0; i < iterations; i++)
                    WktConvert.Deserialize(data);
            });

            var arrayNode = new WktNode("Key", array);
            data = arrayNode.ToString();

            CheckPerformance("ArrayNode", () =>
            {
                for (int i = 0; i < iterations; i++)
                    WktConvert.Deserialize(data);
            });

            var arrayOfNodes = new WktArray(array.Select(x => new WktNode("Key1", x)));
            data = arrayOfNodes.ToString();

            CheckPerformance("ArrayOfNodes", () =>
            {
                for (int i = 0; i < iterations; i++)
                    WktConvert.Deserialize(data);
            });
        }

        private void CheckPerformance(string name, Action action)
        {
            var watch = new Stopwatch();
            watch.Start();

            action();

            // TODO: Change with Tracing
            Console.WriteLine("{0}: {1} ms", name, watch.ElapsedMilliseconds);
            watch.Stop();
        }
    }
}