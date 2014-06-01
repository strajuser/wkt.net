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
using Wkt.NET.Enum;
using Wkt.NET.IO;

namespace Wkt.NET.Tests.IO
{
    [TestClass]
    public class WktTextReaderTests
    {
        [TestMethod]
        public void Read_SimpleNumberValue()
        {
            const string data = "1";

            using (var reader = new WktTextReader(data))
            {
                var rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is int);

                rez = reader.Read();
                Assert.IsFalse(rez);
                Assert.IsTrue(reader.State == ReaderState.Finished);
                Assert.IsNull(reader.Value);
            }
        }

        [TestMethod]
        public void Read_SimpleStringValue()
        {
            const string data = "\"str\"";

            using (var reader = new WktTextReader(data))
            {
                var rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is string);

                rez = reader.Read();
                Assert.IsFalse(rez);
                Assert.IsTrue(reader.State == ReaderState.Finished);
                Assert.IsNull(reader.Value);
            }
        }

        [TestMethod]
        public void Read_SimpleStringNumberValue()
        {
            const string data = "\"1\"";

            using (var reader = new WktTextReader(data))
            {
                var rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is string);

                rez = reader.Read();
                Assert.IsFalse(rez);
                Assert.IsTrue(reader.State == ReaderState.Finished);
                Assert.IsNull(reader.Value);
            }
        }

        [TestMethod]
        public void Read_SimpleArray()
        {
            const string data = "1, \"str\", 2.0";
            
            using (var reader = new WktTextReader(data))
            {
                var rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is int);

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is string);

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is double);

                rez = reader.Read();
                Assert.IsFalse(rez);
                Assert.IsTrue(reader.State == ReaderState.Finished);
                Assert.IsNull(reader.Value);
            }
        }

        [TestMethod]
        public void Read_SimpleNumberNode()
        {
            const string data = "KEY[1]";

            using (var reader = new WktTextReader(data))
            {
                var rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Key);
                Assert.AreEqual(reader.Value, "KEY");

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is int);

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Node);
                Assert.IsNull(reader.Value);

                rez = reader.Read();
                Assert.IsFalse(rez);
                Assert.IsTrue(reader.State == ReaderState.Finished);
                Assert.IsNull(reader.Value);
            }
        }

        [TestMethod]
        public void Read_SimpleStringNode()
        {
            const string data = "KEY[\"1\"]";

            using (var reader = new WktTextReader(data))
            {
                var rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Key);
                Assert.AreEqual(reader.Value, "KEY");

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is string);

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Node);
                Assert.IsNull(reader.Value);

                rez = reader.Read();
                Assert.IsFalse(rez);
                Assert.IsTrue(reader.State == ReaderState.Finished);
                Assert.IsNull(reader.Value);
            }
        }

        [TestMethod]
        public void Read_ArrayNode()
        {
            const string data = "KEY[\"1\", 2]";

            using (var reader = new WktTextReader(data))
            {
                var rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Key);
                Assert.AreEqual(reader.Value, "KEY");

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is string);

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is int);

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Node);
                Assert.IsNull(reader.Value);

                rez = reader.Read();
                Assert.IsFalse(rez);
                Assert.IsTrue(reader.State == ReaderState.Finished);
                Assert.IsNull(reader.Value);
            }
        }

        [TestMethod]
        public void ParseLineBreakData()
        {
            const string data = @"KEY[1,
2]";

            using (var reader = new WktTextReader(data))
            {
                var rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Key);
                Assert.AreEqual(reader.Value, "KEY");

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is int);

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Value);
                Assert.IsTrue(reader.Value is int);

                rez = reader.Read();
                Assert.IsTrue(rez);
                Assert.IsTrue(reader.State == ReaderState.Node);
                Assert.IsNull(reader.Value);

                rez = reader.Read();
                Assert.IsFalse(rez);
                Assert.IsTrue(reader.State == ReaderState.Finished);
                Assert.IsNull(reader.Value);
            }
        }
    }
}