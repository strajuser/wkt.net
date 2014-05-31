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

namespace Wkt.NET.IO
{
    /// <summary> 
    /// Abstract base class for sequential WKT readers
    /// </summary>
    internal abstract class WktReader : IDisposable
    {
        /// <summary>
        /// Gets current reader value
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Gets current state of reader
        /// </summary>
        public ReaderState State { get; private set; }

        /// <summary>
        /// Gets or Sets culture for parsing data
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Creates WktReader with InvariantCulture
        /// </summary>
        protected WktReader()
        {
            Culture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// Sets state and value with parsing and casting value data to closest data type
        /// </summary>
        /// <param name="state">State of reader</param>
        /// <param name="value">String value</param>
        protected void SetState(ReaderState state, string value)
        {
            State = state;
            Value = ParseValue(value);
        }


        /// <summary>
        /// Sets state and value with parsing and casting value data to closest data type
        /// </summary>
        /// <param name="stateValue">Class with State and Value</param>
        protected void SetState(StateValue stateValue)
        {
            SetState(stateValue.State, stateValue.Value);
        }

        /// <summary>
        /// Class for union of state and value in queues
        /// </summary>
        protected class StateValue
        {
            public ReaderState State { get; private set; }
            public string Value { get; private set; }

            public StateValue(ReaderState state, string value)
            {
                Value = value;
                State = state;
            }
        }

        /// <summary>
        /// Try to parse string data and convert to closest data type
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Parsed value</returns>
        private object ParseValue(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            // If string is like '"smth"' - remove double quotes and make string
            if (value.StartsWith("\""))
            {
                value = value.Substring(1, value.Length - 1);
                if (value.EndsWith("\""))
                    value = value.Substring(0, value.Length - 1);
                return value;
            }

            // TODO: make better performance without exceptions
            try
            {
                return Convert.ToInt32(value, Culture);
            }
            catch (FormatException)
            {
            }
            try
            {
                return Convert.ToDouble(value, Culture);
            }
            catch (FormatException)
            {
            }

            {
                DateTime val;
                if (DateTime.TryParse(value, Culture, DateTimeStyles.None, out val))
                    return val;
            }
            {
                Guid val;
                if (Guid.TryParse(value, out val))
                    return val;
            }

            return value;
        }

        /// <summary> 
        /// Reads the next part of WKT 
        /// </summary>
        /// <returns><c>true</c> if the next part was read successfully; <c>false</c> if there are nothing to read.</returns>
        public abstract bool Read();

        #region | IDisposable Members |

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WktReader()
        {
            Dispose(false);
        }

        #endregion
    }
}