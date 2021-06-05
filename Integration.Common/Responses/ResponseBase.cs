using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Common.Responses
{
    public class ResponseBase
    {
        public bool Success { get; set; }

        public List<Error> Errors { get; set; }
    }
}
