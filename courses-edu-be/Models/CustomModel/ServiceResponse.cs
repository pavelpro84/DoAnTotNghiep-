﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace courses_edu_be.Model.CustomModel
{
    public class ServiceResponse
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public object Data { get; set; }
        public int ErrorCode { get; set; }
    }
}
