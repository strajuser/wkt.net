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
using System.Runtime.Serialization;
using Wkt.NET.Interfaces;
using Wkt.NET.Utilities;

namespace Wkt.NET.Exceptions
{
    /// <summary>
    /// Base class for WktExceptions
    /// </summary>
    [Serializable]
    public class WktException : Exception
    {
        public WktException()
        {
        }

        public WktException(string message) : base(message)
        {
        }

        public WktException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WktException(SerializationInfo info,StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Creates WktException with position data
        /// </summary>
        /// <param name="positionProvider">Current position provider</param>
        /// <param name="message">Exception message</param>
        /// <returns></returns>
        internal static WktException Create(IPositionProvider positionProvider, string message)
        {
            message = "Error parsing input data, position: {0}. {1}".Format(positionProvider.CurrentPosition, message);
            return new WktException(message);
        }
    }
}