using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Common.Responses
{
    public class Response<T> : ResponseBase
    {
        public Response(T data)
        {
            Data = data;
            Success = true;
        }

        public T Data { get; set; }
    }
}
