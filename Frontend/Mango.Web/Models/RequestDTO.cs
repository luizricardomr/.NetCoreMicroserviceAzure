﻿using static Mango.Web.Utility.SD;

namespace Mango.Web.Models
{
    public class RequestDTO
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AcessToken { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
