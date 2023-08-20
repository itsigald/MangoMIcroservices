using Mango.Web.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Mango.Web.Utility
{
    public static class SD
    {
        public static HttpMethod ToMethod(this ApiType apiType)
        {
            switch (apiType)
            {
                case ApiType.GET:
                    return HttpMethod.Get;
                case ApiType.POST:
                    return HttpMethod.Post;
                case ApiType.PUT:
                    return HttpMethod.Put;
                case ApiType.DELETE:
                    return HttpMethod.Delete;
                default:
                    return HttpMethod.Get;
            }
        }

        public async static Task<ResponseDto?> ToResponse(this HttpResponseMessage? httpResponse)
        {
            if(httpResponse == null)
                return null;

            switch(httpResponse.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Url Not found",
                        Result = null
                    };
                case System.Net.HttpStatusCode.Forbidden:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Forbidden access",
                        Result = null
                    };
                case System.Net.HttpStatusCode.Unauthorized:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Unauthorized access",
                        Result = null
                    };
                case System.Net.HttpStatusCode.InternalServerError:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Internal Server Error",
                        Result = null
                    };
                default:

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    string content = await httpResponse.Content.ReadAsStringAsync();
                    ResponseDto? responseDto = JsonSerializer.Deserialize<ResponseDto>(content, options);

                    return responseDto;
            }
        }

        public static string TokenCookie = "JwtToken";
        public static string RoleName = "role";
    }

    public enum ApiType
    {
        GET = 0,
        POST,
        PUT,
        DELETE
    }
}
