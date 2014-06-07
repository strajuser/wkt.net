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
using System.IO;
using Wkt.NET.Exceptions;
using Wkt.NET.IO;

namespace Wkt.NET.Serialization
{
    /// <summary>
    /// Serialize and deserialize data in WKT format
    /// </summary>
    public class WktSerializer
    {
        // Wrapper for WktSerializerInternal and WktDeserializerInternal

        /// <summary>
        /// Deserializes data from stream <paramref name="data"/>
        /// </summary>
        /// <param name="data">Input data to deserialize</param>
        /// <returns>Deserialized data</returns>
        /// <exception cref="WktException">Thrown on non valid data for serialization</exception>
        public object Deserialize(Stream data)
        {
            using (var serializer = new WktDeserializerInternal(new WktTextReader(data)))
            {
                return serializer.Deserialize();
            }
        }

        /// <summary>
        /// Deserializes data from string <paramref name="data"/>
        /// </summary>
        /// <param name="data">Input data to deserialize</param>
        /// <returns></returns>
        /// <exception cref="WktException">Thrown on non valid data for serialization</exception>
        public object Deserialize(string data)
        {
            using (var serializer = new WktDeserializerInternal(new WktTextReader(data)))
            {
                return serializer.Deserialize();
            }
        }

        /// <summary>
        /// Serializes object <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown if object type is not supported for serialization</exception>
        public string Serialize(object obj)
        {
            var serializer = new WktSerializerInternal();
            return serializer.Serialize(obj);
        }

        /// <summary>
        /// Serializes object <paramref name="obj"/> with <paramref name="settings"/>
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <param name="settings">Settings for serialization</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown if object type is not supported for serialization</exception>
        public string Serialize(object obj, WktSerializationSettings settings)
        {
            var serializer = new WktSerializerInternal(settings);
            return serializer.Serialize(obj);
        }
    }
}