using System;
using System.Net;
using Demo.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Demo.Core.Models;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;

namespace Demo.Core.Handlers
{
    public class ExceptionHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly CamelCasePropertyNamesContractResolver CamelCaseContractResolver = new CamelCasePropertyNamesContractResolver();

        public static void Execute(IApplicationBuilder errorApp)
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json; charset=utf-8";

                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    var ex = error.Error;
                    var apiException = ex as BaseApiException;
                    if (apiException == null)
                    {
                        var message = ex.Message;

                        apiException = new UnknownApiException(message);
                    }

                    context.Response.StatusCode = apiException.HttpCode;
                    var answer = new ApiErrorResponse(apiException);
                    var json = JsonConvert.SerializeObject(answer, Formatting.Indented, new JsonSerializerSettings { ContractResolver = CamelCaseContractResolver }); 
                    await context.Response.WriteAsync(json, Encoding.UTF8);
                }
            });
        }
    }
}
