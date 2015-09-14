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
using System.Net;
using System.Security.Cryptography.X509Certificates;
using GroupByInc.Api.Util;

namespace GroupByInc.Api.Http.Client
{
    /// <summary>
    ///     <see cref="IClientHttpRequestFactory" /> implementation that uses .NET
    ///     <see cref="HttpWebRequest" /> 's class to create requests.
    /// </summary>
    /// <see cref="T:GroupByInc.Api.Http.Client.WebClientHttpRequest" />
    /// <author>
    ///     Bruno Baia
    /// </author>
    public class WebClientHttpRequestFactory : IClientHttpRequestFactory
    {
        #region IClientHttpRequestFactory Members

        /// <summary>
        ///     Create a new <see cref="IClientHttpRequest" /> for the specified URI
        ///     and HTTP method.
        /// </summary>
        /// <param name="uri">The URI to create a request for.</param>
        /// <param name="method">The HTTP method to execute.</param>
        /// <returns>
        ///     The created request
        /// </returns>
        public virtual IClientHttpRequest CreateRequest(Uri uri, HttpMethod method)
        {
            ArgumentUtils.AssertNotNull(uri, "uri");
            ArgumentUtils.AssertNotNull(method, "method");

            HttpWebRequest httpWebRequest;
            httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

            httpWebRequest.Method = method.ToString();

            if (AllowAutoRedirect != null)
            {
                httpWebRequest.AllowAutoRedirect = AllowAutoRedirect.Value;
            }
            if (UseDefaultCredentials.HasValue)
            {
                httpWebRequest.UseDefaultCredentials = UseDefaultCredentials.Value;
            }
            if (CookieContainer != null)
            {
                httpWebRequest.CookieContainer = CookieContainer;
            }
            if (Credentials != null)
            {
                httpWebRequest.Credentials = Credentials;
            }
            if (_clientCertificates != null)
            {
                foreach (X509Certificate2 certificate in _clientCertificates)
                {
                    httpWebRequest.ClientCertificates.Add(certificate);
                }
            }
            if (Proxy != null)
            {
                httpWebRequest.Proxy = Proxy;
            }
            if (Timeout != null)
            {
                httpWebRequest.Timeout = Timeout.Value;
            }
            if (Expect100Continue != null)
            {
                httpWebRequest.ServicePoint.Expect100Continue = Expect100Continue.Value;
            }
            if (AutomaticDecompression.HasValue)
            {
                httpWebRequest.AutomaticDecompression = AutomaticDecompression.Value;
            }
            return new WebClientHttpRequest(httpWebRequest);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a boolean value that controls whether the request
        ///     should follow redirection responses.
        /// </summary>
        public bool? AllowAutoRedirect { get; set; }

        /// <summary>
        ///     Gets or sets a boolean value that controls whether default
        ///     credentials are sent with this request.
        /// </summary>
        public bool? UseDefaultCredentials { get; set; }

        /// <summary>
        ///     Gets or sets the cookies for the request.
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        ///     Gets or sets authentication information for the request.
        /// </summary>
        public ICredentials Credentials { get; set; }

        private X509CertificateCollection _clientCertificates;

        /// <summary>
        ///     Gets or sets the collection of security certificates that are
        ///     associated with this request.
        /// </summary>
        public X509CertificateCollection ClientCertificates
        {
            get
            {
                if (_clientCertificates == null)
                {
                    _clientCertificates = new X509CertificateCollection();
                }
                return _clientCertificates;
            }
        }

        /// <summary>
        ///     Gets or sets proxy information for the request.
        /// </summary>
        /// <remarks>
        ///     The default value is set by calling the
        ///     <see cref="System.Net.GlobalProxySelection.Select" /> property.
        /// </remarks>
        public IWebProxy Proxy { get; set; }

        /// <summary>
        ///     Gets or sets the time-out value in milliseconds for synchronous
        ///     request only.
        /// </summary>
        /// <remarks>
        ///     The default is 100,000 milliseconds (100 seconds).
        /// </remarks>
        public int? Timeout { get; set; }

        /// <summary>
        ///     Gets or sets a boolean value that determines whether 100-Continue
        ///     behavior is used.
        /// </summary>
        /// <remarks>
        ///     The default value is <see langword="true" /> .
        /// </remarks>
        public bool? Expect100Continue { get; set; }

        /// <summary>
        ///     Gets or sets the type of decompression that is automatically used
        ///     for the response result of this request.
        /// </summary>
        public DecompressionMethods? AutomaticDecompression { get; set; }

        #endregion
    }
}