using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Common.Responses
{
    public class ErrorResponse : ResponseBase
    {
        public ErrorResponse()
        {

        }
        public ErrorResponse(List<Error> errors)
        {
            Errors = errors;
        }

        public ErrorResponse(Error error)
        {
            Errors = new List<Error>();
            Errors.Add(error);
        }

        public string Data { get; set; }
    }
}
