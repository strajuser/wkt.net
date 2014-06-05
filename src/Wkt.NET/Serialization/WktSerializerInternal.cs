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
using System.Text;
using Wkt.NET.Enum;
using Wkt.NET.Linq;

namespace Wkt.NET.Serialization
{
    /// <summary>
    /// Internal class for serialization WKT data
    /// </summary>
    internal class WktSerializerInternal
    {
        private readonly WktSerializationSettings _settings;
        private readonly StringBuilder _sb;

        public WktSerializerInternal() : this(WktSerializationSettings.CreateDefaultSettings())
        {
            
        }

        public WktSerializerInternal(WktSerializationSettings settings)
        {
            _settings = settings;
            _sb = new StringBuilder();
        }

        public string Serialize(object obj)
        {
            if (!(obj is WktValue))
                throw new NotSupportedException("Only WktValue class is supported for serialization in this version");

            ProcessWktValue((WktValue)obj);

            return _sb.ToString();
        }

        private void ProcessWktValue(WktValue value)
        {
            var node = value as WktNode;
            if (node != null)
            {
                _sb.Append(node.Key).Append(GetOpeningArrayChar());
                for (int i = 0; i < node.Count; i++)
                {
                    ProcessWktValue(node[i]);
                    if (i <= node.Count - 2)
                        _sb.Append(_settings.NodeSeparator);
                }
                _sb.Append(GetClosingArrayChar());

                return;
            }

            var array = value as WktArray;
            if (array != null)
            {
                _sb.Append(GetOpeningArrayChar());
                for (int i = 0; i < array.Count; i++)
                {
                    ProcessWktValue(array[i]);
                    if (i <= array.Count - 2)
                        _sb.Append(_settings.ArraySeparator);
                }
                _sb.Append(GetClosingArrayChar());

                return;
            }

            _sb.Append(value.ToString(_settings.FormatProvider));
        }

        private char GetOpeningArrayChar()
        {
            switch (_settings.ArraySerializeType)
            {
                case ArraySerializeType.SquareBrackets:
                default:
                    return '[';
                case ArraySerializeType.Parentheses:
                    return '(';
            }
        }

        private char GetClosingArrayChar()
        {
            switch (_settings.ArraySerializeType)
            {
                case ArraySerializeType.SquareBrackets:
                default:
                    return ']';
                case ArraySerializeType.Parentheses:
                    return ')';
            }
        }
    }
}