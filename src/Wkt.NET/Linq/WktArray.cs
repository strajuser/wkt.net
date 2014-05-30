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
    public class WktArray : WktValue, IList<WktValue>
    {
        private readonly List<WktValue> _values;

        /// <summary>
        /// Creates array of values for WKT objects structure
        /// </summary>
        /// <param name="values">Array of values of WKT Array</param>
        public WktArray(params object[] values)
            : base(Utilities.CreateWktList(values).ToList())
        {
            _values = (List<WktValue>)Value;
        }

        /// <summary>
        /// Creates array of values for WKT objects structure
        /// </summary>
        /// <param name="values">Enumerable of values of WKT Array</param>
        public WktArray(IEnumerable<object> values) : this(values.ToArray())
        {
        }

        #region | IList<WktValue> Members |

        public IEnumerator<WktValue> GetEnumerator()
        {
            return (_values as IEnumerable<WktValue>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_values as IEnumerable).GetEnumerator();
        }

        public void Add(WktValue item)
        {
            _values.Add(item);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(WktValue item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(WktValue[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public bool Remove(WktValue item)
        {
            return _values.Remove(item);
        }

        public int Count
        {
            get { return _values.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public int IndexOf(WktValue item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, WktValue item)
        {
            _values.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _values.RemoveAt(index);
        }

        public WktValue this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }

        #endregion

        /// <summary>
        /// Gets items by key in collection. If there's single element - returns WktValue, otherwise WktArray
        /// </summary>
        /// <param name="key">Key of element</param>
        /// <returns></returns>
        public WktValue this[string key]
        {
            get
            {
                var nodes = _values.OfType<WktNode>().ToList();
                if (nodes.Count(x => x.Key == key) <= 1)
                    return nodes.FirstOrDefault(x => x.Key == key);

                return new WktArray(nodes.Where(x => x.Key == key));
            }
        }

        /// <summary>
        /// Returns WKT Array as formated string (ex. ""WGS_1984", 6378137.0, 298.257223563")
        /// </summary>
        /// <param name="provider">FormatProvider for Value</param>
        /// <returns></returns>
        public override string ToString(IFormatProvider provider)
        {
            var objects = (IEnumerable<WktValue>)Value;

            return String.Join(",", objects.Select(x => x.ToString(provider)));
        }
    }
}