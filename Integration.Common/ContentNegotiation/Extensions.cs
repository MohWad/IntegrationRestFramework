using Integration.Common.ContentNegotiation.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Common.ContentNegotiation
{
    public static class Extensions
    {
        public static void UseContentNegotiation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ContentNegotiationMiddleware>();
        }
    }
}
