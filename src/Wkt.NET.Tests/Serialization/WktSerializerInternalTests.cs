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
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wkt.NET.Enum;
using Wkt.NET.Linq;
using Wkt.NET.Serialization;

namespace Wkt.NET.Tests.Serialization
{
    [TestClass]
    public class WktSerializerInternalTests
    {
        public enum Direction { NORTH, SOUTH, EAST, WEST, UP };

        [TestMethod]
        public void Serialize_WktValue_DefaultSettings()
        {
            var val = new WktValue(0);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), "0");

            val = new WktValue(1);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), "1");

            val = new WktValue(-1);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), "-1");

            val = new WktValue(int.MaxValue);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), int.MaxValue.ToString(CultureInfo.InvariantCulture));

            val = new WktValue("str");
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), "\"str\"");

            val = new WktValue(1.0);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), "1.0");

            val = new WktValue(-1.0);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), "-1.0");

            val = new WktValue(1.123456789);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), "1.123456789");

            val = new WktValue(Direction.NORTH);
            // Enum should be serialized without ""
            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), "NORTH");
        }

        [TestMethod]
        public void Serialize_WktArray_DefaultSettings()
        {
            var arr = new WktArray();
            Assert.AreEqual((new WktSerializerInternal()).Serialize(arr), "[]");

            arr = new WktArray(1);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(arr), "[1]");

            arr = new WktArray(1, 2);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(arr), "[1,2]");

            arr = new WktArray("str");
            Assert.AreEqual((new WktSerializerInternal()).Serialize(arr), "[\"str\"]");

            arr = new WktArray(1, "str");
            Assert.AreEqual((new WktSerializerInternal()).Serialize(arr), "[1,\"str\"]");

            arr = new WktArray(1.0);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(arr), "[1.0]");

            arr = new WktArray(Direction.WEST);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(arr), "[WEST]");
        }

        [TestMethod]
        public void Serialize_WktNode_DefaultSettings()
        {
            var node = new WktNode("Key");
            Assert.AreEqual((new WktSerializerInternal()).Serialize(node), "Key[]");

            node = new WktNode("Key", 1);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(node), "Key[1]");

            node = new WktNode("Key", 1, 2);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(node), "Key[1,2]");

            node = new WktNode("Key", "str");
            Assert.AreEqual((new WktSerializerInternal()).Serialize(node), "Key[\"str\"]");

            node = new WktNode("Key", 1, "str");
            Assert.AreEqual((new WktSerializerInternal()).Serialize(node), "Key[1,\"str\"]");

            node = new WktNode("Key", 1.0);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(node), "Key[1.0]");

            node = new WktNode("Key", 1.0, "str");
            Assert.AreEqual((new WktSerializerInternal()).Serialize(node), "Key[1.0,\"str\"]");

            node = new WktNode("Key", Direction.SOUTH);
            Assert.AreEqual((new WktSerializerInternal()).Serialize(node), "Key[SOUTH]");
        }

        [TestMethod]
        public void Serialize_WktValue_WithSettings()
        {

        }

        [TestMethod]
        public void Serialize_WktArray_WithSettings()
        {

        }

        [TestMethod]
        public void Serialize_WktNode_WithSettings()
        {

        }

        [TestMethod]
        public void Serialize_RealDataExample()
        {
            var val = new WktNode("COMPD_CS", @"OSGB36 / British National Grid + ODN\",
                new WktNode("PROJCS", @"OSGB 1936 / British National Grid",
                    new WktNode("GEOGCS", "OSGB 1936",
                        new WktNode("DATUM", "OSGB_1936",
                            new WktNode("SPHEROID", "Airy 1830", 6377563.396, 299.3249646, new WktNode("AUTHORITY", "EPSG", "7001")),
                            new WktNode("TOWGS84", 375, -111, 431, 0, 0, 0, 0),
                            new WktNode("AUTHORITY", "EPSG", "6277")),
                        new WktNode("PRIMEM", "Greenwich", 0, new WktNode("AUTHORITY", "EPSG", "8901")),
                        new WktNode("UNIT", "DMSH", 0.0174532925199433, new WktNode("AUTHORITY", "EPSG", "9108")),
                        new WktNode("AXIS", "Lat", Direction.NORTH),
                        new WktNode("AXIS", "Long", Direction.EAST),
                        new WktNode("AUTHORITY", "EPSG", "4277")),
                    new WktNode("PROJECTION", "Transverse_Mercator"),
                    new WktNode("PARAMETER", "latitude_of_origin", 49),
                    new WktNode("PARAMETER", "central_meridian", -2),
                    new WktNode("PARAMETER", "scale_factor", 0.999601272),
                    new WktNode("PARAMETER", "false_easting", 400000),
                    new WktNode("PARAMETER", "false_northing", -100000),
                    new WktNode("UNIT", "metre", 1, new WktNode("AUTHORITY", "EPSG", "9001")),
                    new WktNode("AXIS", "E", Direction.EAST),
                    new WktNode("AXIS", "N", Direction.NORTH),
                    new WktNode("AUTHORITY", "EPSG", "27700")),
                new WktNode("VERT_CS", "Newlyn",
                    new WktNode("VERT_DATUM", "Ordnance Datum Newlyn", 2005, new WktNode("AUTHORITY", "EPSG", "5101")),
                    new WktNode("UNIT", "metre", 1, new WktNode("AUTHORITY", "EPSG", "9001")),
                    new WktNode("AXIS", "Up", Direction.UP),
                    new WktNode("AUTHORITY", "EPSG", "5701")),
                new WktNode("AUTHORITY", "EPSG", "7405"));

            var rez = @"COMPD_CS[""OSGB36 / British National Grid + ODN\"",
    PROJCS[""OSGB 1936 / British National Grid"",
        GEOGCS[""OSGB 1936"",
            DATUM[""OSGB_1936"",
                SPHEROID[""Airy 1830"",6377563.396,299.3249646,AUTHORITY[""EPSG"",""7001""]],
                TOWGS84[375,-111,431,0,0,0,0],
                AUTHORITY[""EPSG"",""6277""]],
            PRIMEM[""Greenwich"",0,AUTHORITY[""EPSG"",""8901""]],
            UNIT[""DMSH"",0.0174532925199433,AUTHORITY[""EPSG"",""9108""]],
            AXIS[""Lat"",NORTH],
            AXIS[""Long"",EAST],
            AUTHORITY[""EPSG"",""4277""]],
        PROJECTION[""Transverse_Mercator""],
        PARAMETER[""latitude_of_origin"",49],
        PARAMETER[""central_meridian"",-2],
        PARAMETER[""scale_factor"",0.999601272],
        PARAMETER[""false_easting"",400000],
        PARAMETER[""false_northing"",-100000],
        UNIT[""metre"",1,AUTHORITY[""EPSG"",""9001""]],
        AXIS[""E"",EAST],
        AXIS[""N"",NORTH],
        AUTHORITY[""EPSG"",""27700""]],
    VERT_CS[""Newlyn"",
        VERT_DATUM[""Ordnance Datum Newlyn"",2005,AUTHORITY[""EPSG"",""5101""]],
        UNIT[""metre"",1,AUTHORITY[""EPSG"",""9001""]],
        AXIS[""Up"",UP],
        AUTHORITY[""EPSG"",""5701""]],
    AUTHORITY[""EPSG"",""7405""]]";

            Assert.AreEqual((new WktSerializerInternal()).Serialize(val), RemoveLineBreakData(rez));

            val = new WktNode("MULTIPOINT",
                new [] {10, 40},
                new [] {40, 30},
                new [] {20, 20},
                new [] {30, 10});

            var settings = new WktSerializationSettings
            {
                ArraySeparator = " ",
                ArraySerializeType = ArraySerializeType.Parentheses,
            };
            var serializer = new WktSerializerInternal(settings);
            Assert.AreEqual(serializer.Serialize(val), "MULTIPOINT((10 40),(40 30),(20 20),(30 10))");
        }

        private string RemoveLineBreakData(string value)
        {
            return value.Replace(Environment.NewLine, "").Replace("    ", "");
        }
    }
}