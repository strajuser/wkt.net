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

using System.IO;
using Wkt.NET.Serialization;

namespace Wkt.NET
{
    /// <summary> 
    /// Provides several methods to convert data from/to WKT format
    /// </summary>
    public static class WktConvert
    {
        /// <summary>
        /// Deserialize data from stream <paramref name="data"/> to WktObjects
        /// </summary>
        /// <param name="data">Stream with data to deserialize</param>
        /// <returns></returns>
        public static object Deserialize(Stream data)
        {
            var serializer = new WktSerializer();
            return serializer.Deserialize(data);
        }

        /// <summary>
        /// Deserialize data from string <paramref name="data"/> to WktObjects
        /// </summary>
        /// <param name="data">String with data to deserialize</param>
        /// <returns></returns>
        public static object Deserialize(string data)
        {
            var serializer = new WktSerializer();
            return serializer.Deserialize(data);
        }
    }
}