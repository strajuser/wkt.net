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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wkt.NET.Linq;

namespace Wkt.NET.Tests.Linq
{
    [TestClass]
    public class WktNamedNodeTests
    {
        [TestMethod]
        public void Linq_Simple_Ctor()
        {
            var node = new WktNamedNode("PROJECTION", "Mercator_Auxiliary_Sphere");

            Assert.AreEqual(node.Key, "PROJECTION");
            Assert.IsTrue(node.Count == 0);
            Assert.AreEqual(node.Name, "Mercator_Auxiliary_Sphere");

            // KEY["Name"]
            node = new WktNamedNode("KEY", "Name");
            Assert.IsTrue(node.Count == 0);
            Assert.AreEqual(node.Key, "KEY");
            Assert.AreEqual(node.Name, "Name");

            // KEY["Name", "str"]
            node = new WktNamedNode("KEY", "Name", "str");
            Assert.IsTrue(node.Count == 1);

            // KEY["Name", 1]
            node = new WktNamedNode("KEY", "Name", 1);
            Assert.IsTrue(node.Count == 1);

            // KEY["Name", 1,2]
            node = new WktNamedNode("KEY", "Name", 1, 2);
            Assert.IsTrue(node.Count == 2);

            // KEY["Name", 1,2]
            node = new WktNamedNode("KEY", "Name", new WktArray(1, 2));
            Assert.IsTrue(node.Count == 2);

            // KEY["Name", KEY[1,2]]
            node = new WktNamedNode("KEY", "Name", new WktNode("KEY", 1, 2));
            Assert.IsTrue(node.Count == 1);

            // KEY["Name", 1,2,3]
            node = new WktNamedNode("KEY", "Name", 1, new WktArray(2, 3));
            Assert.IsTrue(node.Count == 3);

            // KEY["Name", 1,KEY[2,3]]
            node = new WktNamedNode("KEY", "Name", 1, new WktNode("KEY", 2, 3));
            Assert.IsTrue(node.Count == 2);

            // KEY["Name", 1,2,"1","2"]
            node = new WktNamedNode("KEY", "Name", new[] { 1, 2 }, new[] { "1", "2" });
            Assert.IsTrue(node.Count == 4);

            // KEY["Name", 1,2,KEY[],"1","2"]
            node = new WktNamedNode("KEY", "Name", new[] { 1, 2 }, new WktNode("KEY"), new[] { "1", "2" });
            Assert.IsTrue(node.Count == 5);

            // KEY["Name", 1,2,3,KEY[],"1","2"]
            node = new WktNamedNode("KEY", "Name", new[] { 1, 2 }, 3, new WktNode("KEY"), new[] { "1", "2" });
            Assert.IsTrue(node.Count == 6);
        }

        [TestMethod]
        public void Linq_Params_Ctor()
        {
            var node = new WktNamedNode("PROJCS",
                "WGS_1984_Web_Mercator_Auxiliary_Sphere",
                new WktNamedNode("PROJECTION", "Mercator_Auxiliary_Sphere"),
                new WktNamedNode("PARAMETER", "False_Easting", 0.0),
                new WktNamedNode("PARAMETER", "False_Northing", 0.0),
                new WktNamedNode("PARAMETER", "Central_Meridian", 0.0),
                new WktNamedNode("PARAMETER", "Standard_Parallel_1", 0.0),
                new WktNamedNode("PARAMETER", "Auxiliary_Sphere_Type", 0.0));

            Assert.AreEqual(node.Name, "WGS_1984_Web_Mercator_Auxiliary_Sphere");
            Assert.IsTrue(node.Count == 6);
            Assert.IsTrue(node.OfType<WktNamedNode>().Count() == 6);
            Assert.IsTrue(node.OfType<WktNamedNode>().Count(x => x.Key == "PARAMETER") == 5);
        }

        [TestMethod]
        public void ToString_Simple()
        {
            var node = new WktNamedNode("KEY", "Name");
            Assert.AreEqual(node.ToString(), "KEY[\"Name\"]");

            node = new WktNamedNode("KEY", "Name", 1, 2.0);
            Assert.AreEqual(node.ToString(), "KEY[\"Name\",1,2.0]");

            node = new WktNamedNode("PROJCS",
                "WGS_1984_Web_Mercator_Auxiliary_Sphere",
                new WktNamedNode("PROJECTION", "Mercator_Auxiliary_Sphere"),
                new WktNamedNode("PARAMETER", "False_Easting", 0.0));
            Assert.AreEqual(node.ToString(), "PROJCS[\"WGS_1984_Web_Mercator_Auxiliary_Sphere\",PROJECTION[\"Mercator_Auxiliary_Sphere\"],PARAMETER[\"False_Easting\",0.0]]");
        }
    }
}