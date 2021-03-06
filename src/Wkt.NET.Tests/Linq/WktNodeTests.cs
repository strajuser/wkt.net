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
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wkt.NET.Enum;
using Wkt.NET.Linq;

namespace Wkt.NET.Tests.Linq
{
    [TestClass]
    public class WktNodeTests
    {
        /*
         * var obj = new WktNode(
                "PROJCS",
                "WGS_1984_Web_Mercator_Auxiliary_Sphere",
                new WktNode("GEOGCS",
                    "GCS_WGS_1984",
                    new WktNode("DATUM",
                        "D_WGS_1984",
                        new WktNode("SPHEROID", "WGS_1984", 6378137.0, 298.257223563)),
                    new WktNode("PRIMEM", "Greenwich", 0.0),
                    new WktNode("UNIT", "Degree", 0.0174532925199433)),
                new WktNode("PROJECTION", "Mercator_Auxiliary_Sphere"),
                new WktNode("PARAMETER", "False_Easting", 0.0),
                new WktNode("PARAMETER", "False_Northing", 0.0),
                new WktNode("PARAMETER", "Central_Meridian", 0.0),
                new WktNode("PARAMETER", "Standard_Parallel_1", 0.0),
                new WktNode("PARAMETER", "Auxiliary_Sphere_Type", 0.0),
                new WktNode("UNIT", "Meter", 1.0),
                new WktNode("AUTHORITY", "ESRI", "102100"));
         */

        public enum DefaultEnum { VALUE, Value };

        [TestMethod]
        public void Linq_Simple_Ctor()
        {
            var node = new WktNode("PROJECTION", "Mercator_Auxiliary_Sphere");

            Assert.AreEqual(node.Key, "PROJECTION");
            Assert.IsTrue(node.Count == 1);
            Assert.AreEqual(node[0].Value, "Mercator_Auxiliary_Sphere");

            // KEY[]
            node = new WktNode("KEY");
            Assert.IsTrue(node.Count == 0);
            Assert.AreEqual(node.Key, "KEY");

            // KEY["str"]
            node = new WktNode("KEY", "str");
            Assert.IsTrue(node.Count == 1);

            // KEY[1]
            node = new WktNode("KEY", 1);
            Assert.IsTrue(node.Count == 1);

            // KEY[1,2]
            node = new WktNode("KEY", 1, 2);
            Assert.IsTrue(node.Count == 2);

            // KEY[[1,2]]
            node = new WktNode("KEY", new WktArray(1, 2));
            Assert.IsTrue(node.Count == 1);
            Assert.IsTrue(node[0] is WktArray);
            Assert.IsTrue(node[0].As<WktArray>().Count == 2);

            // KEY[KEY[1,2]]
            node = new WktNode("KEY", new WktNode("KEY", 1, 2));
            Assert.IsTrue(node.Count == 1);

            // KEY[1,[2,3]]
            node = new WktNode("KEY", 1, new WktArray(2, 3));
            Assert.IsTrue(node.Count == 2);

            // KEY[1,KEY[2,3]]
            node = new WktNode("KEY", 1, new WktNode("KEY", 2, 3));
            Assert.IsTrue(node.Count == 2);

            // KEY[[1,2],["1","2"]]
            node = new WktNode("KEY", new[] {1, 2}, new[] {"1", "2"});
            Assert.IsTrue(node.Count == 2);
            Assert.IsTrue(node[0] is WktArray);
            Assert.IsTrue(node[1] is WktArray);
            Assert.IsTrue(node[0].As<WktArray>().Count == 2);
            Assert.IsTrue(node[1].As<WktArray>().Count == 2);

            // KEY[[1,2],KEY[],["1","2"]]
            node = new WktNode("KEY", new[] { 1, 2 }, new WktNode("KEY"), new[] { "1", "2" });
            Assert.IsTrue(node.Count == 3);

            // KEY[[1,2],3,KEY[],["1","2"]]
            node = new WktNode("KEY", new[] { 1, 2 }, 3, new WktNode("KEY"), new[] { "1", "2" });
            Assert.IsTrue(node.Count == 4);
        }

        //[TestMethod]
        //public void Linq_List_AddMembers()
        //{
        //    var node = new WktNode("KEY");
        //    Assert.IsTrue(node.Count == 0);

        //    node.Add(new WktValue(1));
        //    Assert.IsTrue(node[0].Value is int);
        //    Assert.AreEqual(node[0].Value, 1);

        //    node.Add(1);
        //    Assert.IsTrue(node.Count == 2);
        //    Assert.IsTrue(node[1].Value is int);
        //    Assert.AreEqual(node[1].Value, 1);
        //}

        [TestMethod]
        public void Linq_Params_Ctor()
        {
            var node = new WktNode("PROJCS",
                "WGS_1984_Web_Mercator_Auxiliary_Sphere",
                new WktNode("PROJECTION", "Mercator_Auxiliary_Sphere"),
                new WktNode("PARAMETER", "False_Easting", 0.0),
                new WktNode("PARAMETER", "False_Northing", 0.0),
                new WktNode("PARAMETER", "Central_Meridian", 0.0),
                new WktNode("PARAMETER", "Standard_Parallel_1", 0.0),
                new WktNode("PARAMETER", "Auxiliary_Sphere_Type", 0.0));

            Assert.AreEqual(node[0].Value, "WGS_1984_Web_Mercator_Auxiliary_Sphere");
            Assert.IsTrue(node.Count == 7);
            Assert.IsTrue(node.OfType<WktNode>().Count() == 6);
            Assert.IsTrue(node.OfType<WktNode>().Count(x => x.Key == "PARAMETER") == 5);
        }

        [TestMethod]
        public void Linq_Enumerable_Ctor()
        {
            var list = new List<object> { "str", 1, 2.0 };

            var node = new WktNode("Key", list);

            Assert.IsTrue(node.Count == 3);
            Assert.AreEqual(node[1].Value, 1);
        }

        [TestMethod]
        public void ToString_Simple()
        {
            var node = new WktNode("KEY", "Value");
            Assert.AreEqual(node.ToString(), "KEY[\"Value\"]");

            node = new WktNode("KEY", "Value", 1, 2.0);
            Assert.AreEqual(node.ToString(), "KEY[\"Value\",1,2.0]");

            node = new WktNode("PROJCS", 
                "WGS_1984_Web_Mercator_Auxiliary_Sphere",
                new WktNode("PROJECTION", "Mercator_Auxiliary_Sphere"),
                new WktNode("PARAMETER", "False_Easting", 0.0));
            Assert.AreEqual(node.ToString(), "PROJCS[\"WGS_1984_Web_Mercator_Auxiliary_Sphere\",PROJECTION[\"Mercator_Auxiliary_Sphere\"],PARAMETER[\"False_Easting\",0.0]]");

            node = new WktNode("KEY", DefaultEnum.VALUE, DefaultEnum.Value);
            Assert.AreEqual(node.ToString(), "KEY[VALUE,Value]");
        }

        //[TestMethod]
        public void ToString_With_Culture()
        {
            throw new NotImplementedException();
        }
    }
}