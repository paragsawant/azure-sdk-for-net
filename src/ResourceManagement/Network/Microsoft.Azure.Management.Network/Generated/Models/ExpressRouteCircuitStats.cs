// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
// 
// Code generated by Microsoft (R) AutoRest Code Generator 0.14.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.Network.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;
    using Microsoft.Rest.Azure;

    /// <summary>
    /// Contains Stats associated with the peering
    /// </summary>
    public partial class ExpressRouteCircuitStats
    {
        /// <summary>
        /// Initializes a new instance of the ExpressRouteCircuitStats class.
        /// </summary>
        public ExpressRouteCircuitStats() { }

        /// <summary>
        /// Initializes a new instance of the ExpressRouteCircuitStats class.
        /// </summary>
        public ExpressRouteCircuitStats(int? primarybytesIn = default(int?), int? primarybytesOut = default(int?), int? secondarybytesIn = default(int?), int? secondarybytesOut = default(int?))
        {
            PrimarybytesIn = primarybytesIn;
            PrimarybytesOut = primarybytesOut;
            SecondarybytesIn = secondarybytesIn;
            SecondarybytesOut = secondarybytesOut;
        }

        /// <summary>
        /// Gets BytesIn of the peering on primary device.
        /// </summary>
        [JsonProperty(PropertyName = "primarybytesIn")]
        public int? PrimarybytesIn { get; set; }

        /// <summary>
        /// Gets BytesOut of the peering on primary device.
        /// </summary>
        [JsonProperty(PropertyName = "primarybytesOut")]
        public int? PrimarybytesOut { get; set; }

        /// <summary>
        /// Gets BytesIn of the peering on secondary device.
        /// </summary>
        [JsonProperty(PropertyName = "secondarybytesIn")]
        public int? SecondarybytesIn { get; set; }

        /// <summary>
        /// Gets BytesOut of the peering on secondary device.
        /// </summary>
        [JsonProperty(PropertyName = "secondarybytesOut")]
        public int? SecondarybytesOut { get; set; }

    }
}
