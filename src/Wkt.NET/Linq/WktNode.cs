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
    /// WKT Node with signature key[value {, value}]
    /// </summary>
    public class WktNode : WktArray
    {
        /// <summary>
        /// Creates simple WKT Node with simple value (ex. "UNIT[1.0]") 
        /// </summary>
        /// <param name="key">Key of WKT Node</param>
        /// <param name="value">Value of WKT Node</param>
        /// <example>
        /// <code lang="cs">
        /// var node = new WktNode("UNIT", 1.0)
        /// </code>
        /// </example>
        //public WktNode(string key, object value) : base(Utilities.CreateWktValue(value))
        //{
        //    Key = key;
        //}

        /// <summary>
        /// Creates simple WKT Node with array of values (ex. "SPHEROID["WGS_1984", 6378137.0, 298.257223563]") 
        /// </summary>
        /// <param name="key">Key of WKT Node</param>
        /// <param name="values">Array of values of WKT Node</param>
        /// <example>
        /// <code lang="cs">
        /// new WktNode("SPHEROID", "WGS_1984", 6378137.0, 298.257223563)
        /// </code>
        /// </example>
        public WktNode(string key, params object[] values) : base(new WktArray(values))
        {
            Key = key;
        }

        /// <summary>
        /// Creates simple WKT Node with array of values (ex. "SPHEROID["WGS_1984", 6378137.0, 298.257223563]") 
        /// </summary>
        /// <param name="key">Key of WKT Node</param>
        /// <param name="values">IEnumerable of values of WKT Node</param>
        public WktNode(string key, IEnumerable<object> values) : this(key, values.ToArray())
        {
        }

        /// <summary>
        /// Creates simple WKT Node with simple string value (ex. "PROJECTION["Mercator_Auxiliary_Sphere"]") 
        /// </summary>
        /// <param name="key">Key of WKT Node</param>
        /// <param name="value">IEnumerable of values of WKT Node</param>
        //public WktNode(string key, string value) : this(key, (object)value)
        //{
        //}

        /// <summary>
        /// Gets WKT Node Key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Returns WKT Object as formated string (ex. "SPHEROID["WGS_1984", 6378137.0, 298.257223563]")
        /// </summary>
        /// <param name="provider">FormatProvider for Value</param>
        /// <returns></returns>
        public override string ToString(IFormatProvider provider)
        {
            return String.Format("{0}[{1}]", Key, base.ToString(provider));
        }
    }
}