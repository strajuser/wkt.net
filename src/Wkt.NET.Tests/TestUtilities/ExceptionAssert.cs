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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wkt.NET.Tests.TestUtilities
{
    /// <summary>
    /// Extensions for testing exceptions
    /// </summary>
    internal class ExceptionAssert
    {
        public static void Throws<TException>(string message, Action action, string assertMessage = null)
            where TException : Exception
        {
            try
            {
                action();

                Assert.Fail(assertMessage + Environment.NewLine + "Exception of type {0} expected; got none exception", typeof(TException).Name);
            }
            catch (TException ex)
            {
                if (message != null)
                    Assert.AreEqual(message, ex.Message,
                        assertMessage + Environment.NewLine + "Unexpected exception message." + Environment.NewLine + "Expected: " + message + Environment.NewLine + "Got: " + ex.Message +
                        Environment.NewLine + Environment.NewLine + ex);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(assertMessage + Environment.NewLine + "Exception of type {0} expected; got exception of type {1}.", typeof(TException).Name, ex.GetType().Name), ex);
            }
        }

        public static void Throws<TException>(Action action, string assertMessage = null)
            where TException : Exception
        {
            try
            {
                action();

                Assert.Fail(assertMessage + Environment.NewLine + "Exception of type {0} expected; got none exception", typeof(TException).Name);
            }
            catch (TException) { }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(assertMessage + Environment.NewLine + "Exception of type {0} expected; got exception of type {1}.", typeof(TException).Name, ex.GetType().Name), ex);
            }
        }
    }
}