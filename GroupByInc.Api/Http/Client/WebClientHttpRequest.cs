#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using GroupByInc.Api.Util;

namespace GroupByInc.Api.Http.Client
{
    /// <summary>
    ///     <see cref="IClientHttpRequest" /> implementation that uses .NET
    ///     <see cref="WebClientHttpRequest.HttpWebRequest" />
    ///     's class to execute requests.
    /// </summary>
    /// <seealso cref="T:GroupByInc.Api.Http.Client.WebClientHttpRequestFactory" />
    /// <author>
    ///     Bruno Baia
    /// </author>
    public class WebClientHttpRequest : IClientHttpRequest
    {
        private Action<Stream> body;
        private bool isCancelled;
        private bool isExecuted;

        /// <summary>
        ///     <para>
        ///         Creates a new instance of <see cref="WebClientHttpRequest" />
        ///     </para>
        ///     <para>
        ///         with the given
        ///         <see cref="WebClientHttpRequest.HttpWebRequest" />
        ///         instance.
        ///     </para>
        /// </summary>
        /// <param name="request">
        ///     The
        ///     <see cref="WebClientHttpRequest.HttpWebRequest" />
        ///     instance to use.
        /// </param>
        public WebClientHttpRequest(HttpWebRequest request)
        {
            ArgumentUtils.AssertNotNull(request, "request");

            HttpWebRequest = request;
            Headers = new HttpHeaders();
        }

        /// <summary>
        ///     Gets the
        ///     <see cref="WebClientHttpRequest.HttpWebRequest" />
        ///     instance used by the request.
        /// </summary>
        public HttpWebRequest HttpWebRequest { get; private set; }

        /// <summary>
        ///     Ensures that the request can be executed.
        /// </summary>
        /// <see cref="T:System.InvalidOperationException">
        ///     If the request is already executed or is currently executing.
        /// </see>
        protected void EnsureNotExecuted()
        {
            if (isExecuted)
            {
                throw new InvalidOperationException("Client HTTP request already executed or is currently executing.");
            }
        }

        /// <summary>
        ///     Creates and returns an <see cref="IClientHttpResponse" />
        ///     implementation associated with the request.
        /// </summary>
        /// <param name="response">
        ///     The <see cref="HttpWebResponse" /> instance to use.
        /// </param>
        /// <returns>
        ///     An <see cref="IClientHttpResponse" /> implementation associated with
        ///     the request.
        /// </returns>
        protected virtual IClientHttpResponse CreateClientHttpResponse(HttpWebResponse response)
        {
            return new WebClientHttpResponse(response);
        }

        /// <summary>
        ///     Prepare the request for execution.
        /// </summary>
        /// <remarks>
        ///     Default implementation copies headers to the .NET request. Can be
        ///     overridden in subclasses.
        /// </remarks>
        protected virtual void PrepareForExecution()
        {
            // Copy headers
            foreach (string header in Headers)
            {
                // Special headers
                switch (header.ToUpper(CultureInfo.InvariantCulture))
                {
                    case "ACCEPT":
                    {
                        HttpWebRequest.Accept = Headers[header];
                        break;
                    }
                    case "CONTENT-LENGTH":
                    {
                        HttpWebRequest.ContentLength = Headers.ContentLength;
                        break;
                    }
                    case "CONTENT-TYPE":
                    {
                        HttpWebRequest.ContentType = Headers[header];
                        break;
                    }
                    case "CONNECTION":
                    {
                        string headerValue = Headers[header];
                        if (headerValue.Equals("Keep-Alive", StringComparison.OrdinalIgnoreCase))
                        {
                            HttpWebRequest.KeepAlive = true;
                        }
                        else if (!headerValue.Equals("Close", StringComparison.OrdinalIgnoreCase))
                        {
                            HttpWebRequest.Connection = headerValue;
                        }
                        break;
                    }
                    case "EXPECT":
                    {
                        HttpWebRequest.Expect = Headers[header];
                        break;
                    }
                    case "IF-MODIFIED-SINCE":
                    {
                        DateTime? date = Headers.IfModifiedSince;
                        if (date.HasValue)
                        {
                            HttpWebRequest.IfModifiedSince = date.Value;
                        }
                        else
                        {
                            HttpWebRequest.IfModifiedSince = DateTime.MinValue;
                        }
                        break;
                    }
                    case "RANGE":
                    {
                        string headerValue = Headers[header];
                        try
                        {
                            // Supports '<range specifier>=<from>-<to>' format
                            string[] rangesSpecifiers = headerValue.Split('=');
                            string rangesSpecifier = rangesSpecifiers[0];
                            string byteRangesSpecifier = rangesSpecifiers[1];
                            int byteRangesSpecifierSeparator = byteRangesSpecifier.IndexOf('-');

                            int from = int.Parse(byteRangesSpecifier.Substring(0, byteRangesSpecifierSeparator));
                            int to = int.Parse(byteRangesSpecifier.Substring(byteRangesSpecifierSeparator + 1));

                            HttpWebRequest.AddRange(rangesSpecifier, from, to);
                        }
                        catch
                        {
                            HttpWebRequest.Headers[header] = Headers[header];
                        }
                        break;
                    }
                    case "REFERER":
                    {
                        HttpWebRequest.Referer = Headers[header];
                        break;
                    }
                    case "TRANSFER-ENCODING":
                    {
                        HttpWebRequest.SendChunked = true;
                        string headerValue = Headers[header];
                        if (!headerValue.Equals("Chunked", StringComparison.OrdinalIgnoreCase))
                        {
                            HttpWebRequest.TransferEncoding = headerValue;
                        }
                        break;
                    }

                    case "USER-AGENT":
                    {
                        HttpWebRequest.UserAgent = Headers[header];
                        break;
                    }
                    default:
                    {
                        // Other headers
                        HttpWebRequest.Headers[header] = Headers[header];
                        break;
                    }
                }
            }
        }

        #region IClientHttpRequest Members

        /// <summary>
        ///     Gets the HTTP method of the request.
        /// </summary>
        public HttpMethod Method
        {
            get { return new HttpMethod(HttpWebRequest.Method); }
        }

        /// <summary>
        ///     Gets the URI of the request.
        /// </summary>
        public Uri Uri
        {
            get { return HttpWebRequest.RequestUri; }
        }

        /// <summary>
        ///     Gets the message headers.
        /// </summary>
        public HttpHeaders Headers { get; private set; }

        /// <summary>
        ///     Sets the <see langword="delegate" /> that writes the
        ///     <see cref="body" /> message as a stream.
        /// </summary>
        public Action<Stream> Body
        {
            set { body = value; }
        }

        /// <summary>
        ///     <see cref="Execute" /> this request, resulting in a
        ///     <see cref="IClientHttpResponse" /> that can be read.
        /// </summary>
        /// <returns>
        ///     The response result of the execution
        /// </returns>
        /// <see cref="T:System.InvalidOperationException">
        ///     If the request is already executed or is currently executing.
        /// </see>
        public IClientHttpResponse Execute()
        {
            EnsureNotExecuted();

            try
            {
                // Prepare
                PrepareForExecution();

                // Write
                if (body != null)
                {
                    using (Stream stream = HttpWebRequest.GetRequestStream())
                    {
                        body(stream);
                    }
                }

                // Read
                HttpWebResponse httpWebResponse = HttpWebRequest.GetResponse() as HttpWebResponse;
                if (HttpWebRequest.HaveResponse && httpWebResponse != null)
                {
                    return CreateClientHttpResponse(httpWebResponse);
                }
            }
            catch (WebException ex)
            {
                // This exception can be raised with some status code 
                // Try to retrieve the response from the error
                HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    return CreateClientHttpResponse(httpWebResponse);
                }
                throw;
            }
            finally
            {
                isExecuted = true;
            }

            return null;
        }

        /// <summary>
        ///     <see cref="Execute" /> this request asynchronously.
        /// </summary>
        /// <param name="state">
        ///     An optional user-defined object that is passed to the method invoked
        ///     when the asynchronous operation completes.
        /// </param>
        /// <param name="executeCompleted">
        ///     The <see cref="System.Action`1" /> to perform when the asynchronous
        ///     execution completes.
        /// </param>
        /// <see cref="T:System.InvalidOperationException">
        ///     If the request is already executed or is currently executing.
        /// </see>
        public void ExecuteAsync(object state, Action<ClientHttpRequestCompletedEventArgs> executeCompleted)
        {
            EnsureNotExecuted();

            AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(state);
            ExecuteState executeState = new ExecuteState(executeCompleted, asyncOperation);

            try
            {
                // Prepare
                PrepareForExecution();

                // Post request
                if (body != null)
                {
                    HttpWebRequest.BeginGetRequestStream(ExecuteRequestCallback, executeState);
                }
                else
                {
                    // Get request
                    HttpWebRequest.BeginGetResponse(ExecuteResponseCallback, executeState);
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
                {
                    throw;
                }
                ExecuteAsyncCallback(executeState, null, ex);
            }
            finally
            {
                isExecuted = true;
            }
        }

        /// <summary>
        ///     Cancels a pending asynchronous operation.
        /// </summary>
        public void CancelAsync()
        {
            isCancelled = true;
            try
            {
                if (HttpWebRequest != null)
                {
                    HttpWebRequest.Abort();
                }
            }
            catch (Exception exception)
            {
                if (((exception is OutOfMemoryException) || (exception is StackOverflowException)) ||
                    (exception is ThreadAbortException))
                {
                    throw;
                }
            }
        }

        #endregion

        #region Async methods/classes

        private void ExecuteRequestCallback(IAsyncResult result)
        {
            ExecuteState state = (ExecuteState) result.AsyncState;

            try
            {
                // Write
                using (Stream stream = HttpWebRequest.EndGetRequestStream(result))
                {
                    body(stream);
                }

                // Read
                HttpWebRequest.BeginGetResponse(ExecuteResponseCallback, state);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
                {
                    throw;
                }
                ExecuteAsyncCallback(state, null, ex);
            }
        }

        private void ExecuteResponseCallback(IAsyncResult result)
        {
            ExecuteState state = (ExecuteState) result.AsyncState;

            IClientHttpResponse response = null;
            Exception exception = null;
            try
            {
                HttpWebResponse httpWebResponse = HttpWebRequest.EndGetResponse(result) as HttpWebResponse;
                if (HttpWebRequest.HaveResponse && httpWebResponse != null)
                {
                    response = CreateClientHttpResponse(httpWebResponse);
                }
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
                {
                    throw;
                }
                exception = ex;
                // This exception can be raised with some status code 
                // Try to retrieve the response from the error
                if (ex is WebException)
                {
                    HttpWebResponse httpWebResponse = ((WebException) ex).Response as HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        exception = null;
                        response = CreateClientHttpResponse(httpWebResponse);
                    }
                }
            }

            ExecuteAsyncCallback(state, response, exception);
        }

        // This is the method that the underlying, free-threaded asynchronous behavior will invoke.  
        // This will happen on an arbitrary thread.
        private void ExecuteAsyncCallback(ExecuteState state, IClientHttpResponse response, Exception exception)
        {
            // Package the results of the operation
            ClientHttpRequestCompletedEventArgs eventArgs = new ClientHttpRequestCompletedEventArgs(response, exception,
                isCancelled,
                state.AsyncOperation.UserSuppliedState);
            ExecuteCallbackArgs<ClientHttpRequestCompletedEventArgs> callbackArgs =
                new ExecuteCallbackArgs<ClientHttpRequestCompletedEventArgs>(eventArgs,
                    state.ExecuteCompleted);

            // End the task. The asyncOp object is responsible for marshaling the call.
            state.AsyncOperation.PostOperationCompleted(ExecuteResponseReceived, callbackArgs);
        }

        private static void ExecuteResponseReceived(object arg)
        {
            ExecuteCallbackArgs<ClientHttpRequestCompletedEventArgs> callbackArgs =
                (ExecuteCallbackArgs<ClientHttpRequestCompletedEventArgs>) arg;
            if (callbackArgs.Callback != null)
            {
                callbackArgs.Callback(callbackArgs.EventArgs);
            }
        }

        private class ExecuteCallbackArgs<T> where T : class
        {
            public readonly Action<T> Callback;
            public readonly T EventArgs;

            public ExecuteCallbackArgs(T eventArgs,
                Action<T> callback)
            {
                EventArgs = eventArgs;
                Callback = callback;
            }
        }

        private class ExecuteState
        {
            public readonly AsyncOperation AsyncOperation;
            public readonly Action<ClientHttpRequestCompletedEventArgs> ExecuteCompleted;

            public ExecuteState(
                Action<ClientHttpRequestCompletedEventArgs> executeCompleted,
                AsyncOperation asyncOperation)
            {
                ExecuteCompleted = executeCompleted;
                AsyncOperation = asyncOperation;
            }
        }

        #endregion
    }
}