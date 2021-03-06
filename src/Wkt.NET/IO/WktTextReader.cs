﻿#region License
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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wkt.NET.Enum;
using Wkt.NET.Exceptions;
using Wkt.NET.Interfaces;
using Wkt.NET.Utilities;

namespace Wkt.NET.IO
{
    /// <summary>
    /// Reader that reads WKT-data from TextReader, Stream or String
    /// </summary>
    internal class WktTextReader : WktReader, IPositionProvider
    {
        private readonly TextReader _reader;
        private readonly StringBuffer _buffer = new StringBuffer(1024);
        // Internal Stack with delimiters to validate
        private readonly Stack<char?> _delimeters = new Stack<char?>();

        // Max queue length is 2 - at the end of node
        private readonly Queue<StateValue> _stateQueue = new Queue<StateValue>(2);

        private WktTextReader(TextReader reader)
        {
            _reader = reader;
            SetState(ReaderState.Start, null);
        }

        public WktTextReader(Stream stream)
            : this(new StreamReader(stream))
        {
        }

        public WktTextReader(string data) : this(new MemoryStream(Encoding.UTF8.GetBytes(data)))
        {
        }

        /// <summary> 
        /// Reads the next part of WKT 
        /// </summary>
        /// <returns><c>true</c> if the next part was read successfully; <c>false</c> if there are nothing to read.</returns>
        public override bool Read()
        {
            while (true)
            {
                if (_stateQueue.Any())
                {
                    SetState(_stateQueue.Dequeue());
                    return true;
                }

                var c = _reader.Read();
                _pos++;
                if (c == -1 && !_buffer.IsEmpty())
                {
                    _allowContinue = false;
                    SetState(ReaderState.Value, _buffer.Flush());
                    return true;
                }
                if (c == -1)
                {
                    // Check if _delimiters are empty
                    var delimiter = _delimeters.PeekOrDefault();
                    if (delimiter != null)
                        throw WktException.Create(this, "Expected '{0}'".Format(delimiter));

                    if (_allowContinue)
                        throw WktException.Create(this, "Expected value after ','");

                    break;
                }

                ProcessNextChar((char) c);
            }

            SetState(ReaderState.Finished, null);
            return false;
        }

        /// <summary>
        /// Process the current char in reader and read next if need
        /// </summary>
        private void ProcessNextChar(char c)
        {
            switch (c)
            {
                case '\n': case '\r':
                    if (CheckInStringValue())
                        _buffer.Append(c);
                    break;
                    
                case '\"':
                    if (CheckInStringValue())
                        _delimeters.Pop();
                    else
                    {
                        if (!_buffer.IsEmpty())
                            throw WktException.Create(this, "Unexpected '\"'");

                        _delimeters.Push('\"');
                    }

                    _buffer.Append(c);

                    break;

                case ' ':
                    if (CheckInStringValue())
                    {
                        _buffer.Append(c);
                        return;
                    }

                    if (!_buffer.IsEmpty())
                        EnqueueValue();

                    break;

                case '(':
                    if (CheckInStringValue())
                    {
                        _buffer.Append(c);
                        return;
                    }

                    _allowContinue = false;
                    
                    _delimeters.Push('(');
                    _stateQueue.Enqueue(new StateValue(ReaderState.Key, _buffer.Flush()));
                    
                    break;

                case ')':
                    if (CheckInStringValue())
                    {
                        _buffer.Append(c);
                        return;
                    }

                    {
                        var delimiter = _delimeters.PeekOrDefault();
                        if (delimiter != '(')
                            throw WktException.Create(this, "Bad closing ')', expected '{0}'".Format(GetExpectedDelimiter(delimiter)));
                    }

                    if (!_buffer.IsEmpty())
                        EnqueueValue();
                    else if (_allowContinue)
                        throw WktException.Create(this, "Expected value after ','");

                    _delimeters.Pop();
                    _stateQueue.Enqueue(new StateValue(ReaderState.Node, _buffer.Flush()));

                    break;

                case '[':
                    if (CheckInStringValue())
                    {
                        _buffer.Append(c);
                        return;
                    }

                    _allowContinue = false;

                    _delimeters.Push('[');
                    _stateQueue.Enqueue(new StateValue(ReaderState.Key, _buffer.Flush()));
                    break;

                case ']':
                    if (CheckInStringValue())
                    {
                        _buffer.Append(c);
                        return;
                    }

                    {
                        var delimiter = _delimeters.PeekOrDefault();
                        if (delimiter != '[')
                            throw WktException.Create(this, "Bad closing ']', expected '{0}'".Format(GetExpectedDelimiter(delimiter)));
                    }

                    if (!_buffer.IsEmpty())
                        EnqueueValue();
                    else if (_allowContinue)
                        throw WktException.Create(this, "Expected value after ','");

                    _delimeters.Pop();
                    _stateQueue.Enqueue(new StateValue(ReaderState.Node, _buffer.Flush()));
                    break;
                
                case ',':
                    if (CheckInStringValue())
                    {
                        _buffer.Append(c);
                        return;
                    }

                    if (!_buffer.IsEmpty())
                        EnqueueValue();
                    else if (State != ReaderState.Node)
                        throw WktException.Create(this, "Expected value");
                    _allowContinue = true;
                    break;
                
                default:
                    _buffer.Append(c);
                    break;
            }
        }

        private char? GetExpectedDelimiter(char? lastDelimiter)
        {
            switch (lastDelimiter)
            {
                case '"': return '"';
                case '[': return ']';
                case '(': return ')';
                case null: return null;
            }
            throw WktException.Create(this, "Unknown delimiter {0}".Format(lastDelimiter));
        }

        private bool _allowContinue;
        private void EnqueueValue()
        {
            _allowContinue = false;
            _stateQueue.Enqueue(new StateValue(ReaderState.Value, _buffer.Flush()));
        }

        /// <summary>
        /// Checks if current state is string value (e.g. character is between '"')
        /// </summary>
        /// <returns><c>true</c> if current characters between '"', <c>false</c> otherwise</returns>
        /// <remarks>
        /// It's necessary to correct parsing "smth[]\/,"
        /// </remarks>
        private bool CheckInStringValue()
        {
            if (!_delimeters.Any())
                return false;

            var del = _delimeters.Peek();

            if (del == '\"')
                return true;

            return false;
        }

        private int _pos;
        /// <summary>
        /// Gets current position of reader
        /// </summary>
        public int CurrentPosition { get { return _pos; } }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _reader.Dispose();

            base.Dispose(disposing);
        }
    }
}