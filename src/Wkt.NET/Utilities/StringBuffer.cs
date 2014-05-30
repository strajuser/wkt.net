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
using System.Diagnostics;

namespace Wkt.NET.Utilities
{
    /// <summary>
    /// Internal helper class for building strings like StringBuilder with reuse
    /// </summary>
    internal class StringBuffer
    {
        private char[] _buffer;
        private int _position;

        private static readonly char[] EmptyBuffer = new char[0];

        public StringBuffer()
        {
            _buffer = EmptyBuffer;
        }

        public StringBuffer(int initalSize)
        {
            _buffer = new char[initalSize];
        }

        /// <summary>
        /// Adds to buffer character <paramref name="value"/>
        /// </summary>
        /// <param name="value">Characted to add</param>
        public void Append(char value)
        {
            if (_position == _buffer.Length)
                EnsureSize(1);

            _buffer[_position++] = value;
        }

        /// <summary>
        /// Adds to buffer several number of characters from another array <paramref name="buffer"/>
        /// </summary>
        /// <param name="buffer">Characters array</param>
        /// <param name="startIndex">Start index to add</param>
        /// <param name="count">Count of symbols to add</param>
        public void Append(char[] buffer, int startIndex, int count)
        {
            Debug.Assert(startIndex + count <= buffer.Length);
            if (_position + count >= _buffer.Length)
                EnsureSize(count);
            Array.Copy(buffer, startIndex, _buffer, _position, count);
            _position += count;
        }

        /// <summary>
        /// Clears a current buffer
        /// </summary>
        public void Clear()
        {
            _buffer = EmptyBuffer;
            _position = 0;
        }

        /// <summary>
        /// Returns current ToString() state and clears current buffer
        /// </summary>
        /// <returns></returns>
        public string Flush()
        {
            var rez = ToString();
            Clear();
            return rez;
        }

        /// <summary>
        /// Checks if current buffer is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _position == 0;
        }

        private void EnsureSize(int appendLength)
        {
            var newBuffer = new char[(_position + appendLength) * 2];
            Array.Copy(_buffer, newBuffer, _position);
            _buffer = newBuffer;
        }

        public override string ToString()
        {
            return ToString(0, _position);
        }

        private string ToString(int start, int length)
        {
            Debug.Assert(start + length <= _buffer.Length);
            return new string(_buffer, start, length);
        }
    }
}