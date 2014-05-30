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
using System.Collections.Generic;
using System.Linq;
using Wkt.NET.Enum;
using Wkt.NET.IO;
using Wkt.NET.Linq;

namespace Wkt.NET.Serialization
{
    /// <summary>
    /// Internal class for serialization/deserialization WKT data
    /// </summary>
    internal class WktSerializerInternal : IDisposable
    {
        private readonly Stack<object> _stack = new Stack<object>();
        private readonly WktReader _reader;

        public WktSerializerInternal(WktReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Deserialize data from current WktReader
        /// </summary>
        /// <returns></returns>
        public object Deserialize()
        {
            while (_reader.Read())
                ProcessReaderState();

            //TODO: throw Exception if _stack contains several elements
            return _stack.Pop();
        }

        /// <summary>
        /// Process current reader state
        /// </summary>
        private void ProcessReaderState()
        {
            switch (_reader.State)
            {
                case ReaderState.KeyEnded:
                {
                    var key = _reader.Value.ToString();
                    _stack.Push(new KeyToken(key));
                }
                    break;
                case ReaderState.ValueEnded: 
                    if (_reader.Value != null)
                        _stack.Push(new WktValue(_reader.Value));
                    break;
                case ReaderState.Finished:
                case ReaderState.NodeEnded:
                {
                    if (_reader.Value != null)
                        _stack.Push(new WktValue(_reader.Value));

                    // If node ended get data from stack until KeyToken, and replace this data with WktNode
                    var values = new List<object>();
                    while (_stack.Any())
                    {
                        var temp = _stack.Pop();
                        var key = temp as KeyToken;
                        if (key != null)
                        {
                            values.Reverse();
                            var node = new WktNode(key.Key, values);
                            _stack.Push(node);
                            break;
                        }

                        values.Add(temp);   
                    }

                    if (!_stack.Any())
                    {
                        values.Reverse();
                        _stack.Push(new WktArray(values));
                    }
                }
                    break;
            }
        }

        /// <summary>
        /// Nested class for pushing key data to stack
        /// </summary>
        private class KeyToken
        {
            public string Key { get; private set; }

            public KeyToken(string key)
            {
                Key = key;
            }
        }

        #region | IDisposable Members |
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WktSerializerInternal()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _reader.Dispose();
        }
        #endregion
    }
}