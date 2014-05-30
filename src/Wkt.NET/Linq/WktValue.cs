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

namespace Wkt.NET.Linq
{
    /// <summary>
    /// Base class for simple WKT objects
    /// </summary>
    public class WktValue
    {
        internal static IFormatProvider DefaultFormatter = CultureInfo.InvariantCulture;

        public WktValue(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Value of WKT data
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Returns WKT Object as formated string with InvariantCulture
        /// </summary>
        /// <remarks>
        /// Method sealed, use <see cref="ToString(IFormatProvider)"/> for derived classes
        /// </remarks>
        public sealed override string ToString()
        {
            return ToString(DefaultFormatter);
        }

        /// <summary>
        /// Returns WKT Object as formated string 
        /// </summary>
        /// <param name="provider">FormatProvider for Value</param>
        /// <returns></returns>
        public virtual string ToString(IFormatProvider provider)
        {
            if (Value == null)
                return String.Empty;

            if (Value is string)
                return String.Format(provider, "\"{0}\"",  Value);

            if (Value is double ||
                Value is float ||
                Value is decimal)
                return String.Format(provider, "{0:0.0}", Value);

            return String.Format(provider, "{0}", Value);
        }
    }
}