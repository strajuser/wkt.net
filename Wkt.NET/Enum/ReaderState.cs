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

namespace Wkt.NET.Enum
{
    /// <summary>
    /// Specifies current state of the reader
    /// </summary>
    internal enum ReaderState
    {
        /// <summary> Reader is initialized, Read() has not been called </summary>
        Start,
        /// <summary> The end has been reached </summary>
        Finished,

        /// <summary> Read Method has been started readding key </summary>
        KeyStarted,
        //Key,
        /// <summary> Read Method has been ended readding key </summary>
        KeyEnded,
        /// <summary> Read Method has been started readding value </summary>
        ValueStarted,
        //Value,
        /// <summary> Read Method has been ended readding value </summary>
        ValueEnded,
        /// <summary> Read Method has been started readding node </summary>
        NodeStarted,
        //Node,
        /// <summary> Read Method has been ended readding node </summary>
        NodeEnded,
        
        /// <summary> An error occurs while reading </summary>
        Error
    }
}