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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wkt.NET.Exceptions;
using Wkt.NET.IO;
using Wkt.NET.Linq;
using Wkt.NET.Serialization;
using Wkt.NET.Tests.TestUtilities;

namespace Wkt.NET.Tests.Serialization
{
    [TestClass]
    public class WktSerializerInternalTests
    {
        [TestMethod]
        public void Deserialize_WktValue()
        {
            const string data = "str";
            using (var serializer = new WktSerializerInternal(new WktTextReader(data)))
            {
                var rez = serializer.Deserialize();
                Assert.IsTrue(rez is WktValue);
            }
        }

        [TestMethod]
        public void Deserialize_WktNode()
        {
            const string data = "PROJCS[\"Name\"]";
            using (var serializer = new WktSerializerInternal(new WktTextReader(data)))
            {
                var node = serializer.Deserialize();
                Assert.IsTrue(node is WktNode);
            }
        }

        [TestMethod]
        public void Deserialize_WktNodeArray()
        {
            const string data = "PROJCS[\"Name\", \"Name2\"]";
            using (var serializer = new WktSerializerInternal(new WktTextReader(data)))
            {
                var node = serializer.Deserialize() as WktNode;
                Assert.IsTrue(node != null);
                Assert.IsTrue(node.Count == 2);
                Assert.IsTrue(node[0].Value is String);
            }
        }

        [TestMethod]
        public void Deserialize_ComplexWkt()
        {
            const string data = "PROJCS[\"WGS_1984_Web_Mercator_Auxiliary_Sphere\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Mercator_Auxiliary_Sphere\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",0.0],PARAMETER[\"Standard_Parallel_1\",0.0],PARAMETER[\"Auxiliary_Sphere_Type\",0.0],UNIT[\"Meter\",1.0]]";
            using (var serializer = new WktSerializerInternal(new WktTextReader(data)))
            {
                var node = serializer.Deserialize() as WktNode;
                
                Assert.IsNotNull(node);
                Assert.AreEqual(node[0].Value, "WGS_1984_Web_Mercator_Auxiliary_Sphere");
                Assert.IsTrue(node.OfType<WktNode>().Count() == 8);
                Assert.IsTrue(node.OfType<WktNode>().Count(x => x.Key == "PARAMETER") == 5);

                var node2 = node[1] as WktNode;
                Assert.IsNotNull(node2);
                Assert.AreEqual(node2.Key, "GEOGCS");
                Assert.AreEqual(node2[0].Value, "GCS_WGS_1984");
                Assert.IsTrue(node2.Count == 4);
                Assert.IsTrue(node2.Last() is WktNode);
                Assert.AreEqual(((WktNode)node2.Last()).Key, "UNIT");
            }
        }

        [TestMethod]
        public void BrokenDelimiters()
        {
            const string data = "1, 2]";

            using (var serializer = new WktSerializerInternal(new WktTextReader(data)))
            {
                ExceptionAssert.Throws<WktException>("Error parsing input data, position: 5. Bad closing ']', expected ''", () => serializer.Deserialize());
            }
        }
    }
}