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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wkt.NET.Linq
{
    /// <summary>
    /// Array of WKT Objects with signature [value {, value}]
    /// </summary>
    /// <remarks>
    /// Can contains <see cref="WktNode">nodes</see>
    /// </remarks>
    public class WktArray : WktValue
    {
        private readonly IEnumerable<WktValue> _values;

        /// <summary>
        /// Creates array of values for WKT objects structure
        /// </summary>
        /// <param name="values">Array of values of WKT Array</param>
        public WktArray(params object[] values)
            : base(values.Select(x => new WktValue(x)).ToArray())
        {
            // TODO: Check if value
            _values = (IEnumerable<WktValue>) Value;
        }

        /// <summary>
        /// Creates array of values for WKT objects structure
        /// </summary>
        /// <param name="values">Enumerable of values of WKT Array</param>
        public WktArray(IEnumerable values) : this(values.Cast<object>().ToArray())
        {
            
        }

        //public virtual object this[string key]
        //{
        //    get
        //    {
        //        // TODO: ForFuture
        //        throw new NotImplementedException();
        //    }
        //}

        /// <summary>
        /// Returns WKT Array as formated string (ex. ""WGS_1984", 6378137.0, 298.257223563")
        /// </summary>
        /// <param name="provider">FormatProvider for Value</param>
        /// <returns></returns>
        public override string ToString(IFormatProvider provider)
        {
            var objects = (IEnumerable<WktValue>) Value;

            return String.Join(",", objects.Select(x => x.ToString(provider)));
        }
    }
}