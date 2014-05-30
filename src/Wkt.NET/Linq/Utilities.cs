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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wkt.NET.Linq
{
    /// <summary>
    /// Helper class for transforming WktObjects
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Transform source values to WktValue objects if it's need
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<WktValue> CreateWktList(IEnumerable source)
        {
            return source.Cast<object>().Select(CreateWktValue);
        }

        /// <summary>
        /// Transform source value to WktValue object if it's need
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static WktValue CreateWktValue(object value)
        {
            return value is WktValue ? (WktValue)value : new WktValue(value);
        }
    }
}