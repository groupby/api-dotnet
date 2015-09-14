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
using GroupByInc.Api.Http;
using GroupByInc.Api.Http.Client;
using GroupByInc.Api.Util;

namespace GroupByInc.Api.Tests.Http.Client.Testing
{
    /// <summary>
    /// Mock implementation of <see cref="IClientHttpRequestFactory"/>. 
    /// Contains a list of expected <see cref="MockClientHttpRequest"/>s, and iterates over those.
    /// </summary>
    /// <author>Arjen Poutsma</author>
    /// <author>Lukas Krecan</author>
    /// <author>Craig Walls</author>
    /// <author>Bruno Baia (.NET)</author>
    public class MockClientHttpRequestFactory : IClientHttpRequestFactory
    {

        private MockClientHttpRequest _mockClientHttpRequest;

        /// <summary>
        /// Create a new <see cref="IClientHttpRequest"/> for the specified URI and HTTP method.
        /// </summary>
        /// <param name="uri">The URI to create a request for.</param>
        /// <param name="method">The HTTP method to execute.</param>
        /// <returns>The created request.</returns>
        public IClientHttpRequest CreateRequest(Uri uri, HttpMethod method)
        {
            ArgumentUtils.AssertNotNull(uri, "uri");
            ArgumentUtils.AssertNotNull(method, "method");

            MockClientHttpRequest currentRequest = _mockClientHttpRequest;
            currentRequest.Uri = uri;
            currentRequest.Method = method;
            return currentRequest;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddMockClient(MockClientHttpRequest mockClientHttpRequest)
        {
            ArgumentUtils.AssertNotNull(mockClientHttpRequest, "mockClientHttpRequest");
            _mockClientHttpRequest = mockClientHttpRequest;
        }
    }

}
