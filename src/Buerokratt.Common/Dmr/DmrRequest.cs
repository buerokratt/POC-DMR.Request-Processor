﻿using System.Diagnostics.CodeAnalysis;
using Buerokratt.Common.Models;

namespace Buerokratt.Common.Dmr
{
    /// <summary>
    /// The model for Dmr requests, including headers and payload/body
    /// </summary>
    [ExcludeFromCodeCoverage] // No logic so not appropriate for code coverage
    public record DmrRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="headers">The headers that should be added to the headers property.</param>
        public DmrRequest(HeadersInput headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            Headers = headers;
        }

        /// <summary>
        /// The headers of requests to or from Dmr
        /// </summary>
        public HeadersInput Headers { get; }

        /// <summary>
        /// The payload (request body) of requests to or from Dmr
        /// </summary>
        public DmrRequestPayload? Payload { get; set; }
    }
}