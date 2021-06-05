using Integration.Common.Validation.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integration.Common.Validation
{
    public static class Extentions
    {
        public static List<string> GetErrorList(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
        }

        public static IServiceCollection AddValidationFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilter>();

            return services;
        }
    }
}
