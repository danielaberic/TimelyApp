using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace timely.Helpers
{
    public static class Pagination
    {
       // public static void AddAplicationError(this HttpResponse response, string message)
       // {
       //     response.Headers.Add("Application-Error", message);
       //     response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
       //     response.Headers.Add("Access-Control-Allow-Origin", "*");
       // }
        public static void AddPagination(this HttpResponse response, [FromQuery] PaginationParameters @params, int count)
        {
            var paginationMetadata = new PaginationMetadata(@params.Page, count, @params.ItemsPerPage);

            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();

            response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
        }
    }
}
