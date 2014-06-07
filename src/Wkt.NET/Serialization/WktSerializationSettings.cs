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
using Wkt.NET.Enum;

namespace Wkt.NET.Serialization
{
    /// <summary>
    /// Setttings for Wkt Serialization
    /// </summary>
    public class WktSerializationSettings
    {
        /// <summary>
        /// Gets or sets IFormatProvider for data formatting
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets type of array serialization - with SquareBrackets or Parentheses
        /// </summary>
        public ArraySerializeType ArraySerializeType { get; set; }

        /// <summary>
        /// Gets or sets separator for WktArrays
        /// </summary>
        public string ArraySeparator { get; set; }

        /// <summary>
        /// Gets or sets separator for WktNodes
        /// </summary>
        public string NodeSeparator { get; set; }

        /// <summary>
        /// Creates default serialization settings with SqareBrackets array type, Invariant Culure and ',' as array and node separator
        /// </summary>
        public WktSerializationSettings()
        {
            FormatProvider = CultureInfo.InvariantCulture;
            ArraySerializeType = ArraySerializeType.SquareBrackets;
            ArraySeparator = ",";
            NodeSeparator = ",";
        }

        /// <summary>
        /// Creates default serialization settings with SqareBrackets array type, Invariant Culure and ',' as array and node separator
        /// </summary>
        /// <returns></returns>
        public static WktSerializationSettings CreateDefaultSettings()
        {
            return new WktSerializationSettings();
        }
    }
}