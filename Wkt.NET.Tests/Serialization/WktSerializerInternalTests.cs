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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wkt.NET.IO;
using Wkt.NET.Linq;
using Wkt.NET.Serialization;

namespace Wkt.NET.Tests.Serialization
{
    [TestClass]
    public class WktSerializerInternalTests
    {
        [TestMethod]
        public void Deserialize_WktNode()
        {
            const string data = "PROJCS[\"Name\"]";
            using (var serializer = new WktSerializerInternal(new WktTextReader(data)))
            {
                var rez = serializer.Deserialize();
                Assert.IsTrue(rez is WktNode);
            }
        }

        [TestMethod]
        public void Deserialize_WktArray()
        {
            const string data = "PROJCS[\"Name\", \"Name2\"]";
            using (var serializer = new WktSerializerInternal(new WktTextReader(data)))
            {
                var rez = serializer.Deserialize() as WktNode;
                Assert.IsTrue(rez != null);
                Assert.IsTrue(rez.Value is WktArray);
            }
        }

        [TestMethod]
        public void Deserialize_ComplexWkt()
        {
            throw new NotImplementedException();
            //const string data = "PROJCS[\"WGS_1984_Web_Mercator_Auxiliary_Sphere\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Mercator_Auxiliary_Sphere\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",0.0],PARAMETER[\"Standard_Parallel_1\",0.0],PARAMETER[\"Auxiliary_Sphere_Type\",0.0],UNIT[\"Meter\",1.0]]";
            //using (var serializer = new WktSerializerInternal(new WktTextReader(data)))
            //{
            //    var rez = serializer.Deserialize() as WktNode;
            //    Assert.IsTrue(rez != null);
            //    Assert.IsTrue(rez.Value is WktArray);
            //}
        }
    }
}