using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ECommerce.Application.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorType
    {
        Failure = 1,
        Validation = 2,
        NotFound = 3,
        Conflict = 4,
        UnAuthorized = 5,
        Forbidden = 6,
        InvalidCredentials = 7,
    }
}
