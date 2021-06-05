using Integration.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Net;

namespace Integration.Common.Versioning
{
    public class ApiVersioningErrorResponseProvider : DefaultErrorResponseProvider
    {
        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            string errorMessage = "";

            if (context.ErrorCode == "UnsupportedApiVersion")
                errorMessage = "This API version is not supported";
            else if (context.ErrorCode == "ApiVersionUnspecified")
                errorMessage = "API version must be specified";

            //You can initialize your own class here. Below is just a sample.
            var errorResponse = new ErrorResponse(new Error { Code = "", Message = errorMessage });
            var response = new ObjectResult(errorResponse);
            response.StatusCode = (int)HttpStatusCode.BadRequest;

            return response;
        }
    }
}
