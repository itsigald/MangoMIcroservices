using Mango.Web.Models;
using Mango.Web.Utility;
using System.Text;
using System.Text.Json;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory clientFactory, ITokenProvider tokenProvider)
        {
            _clientFactory = clientFactory;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            using (HttpClient client = _clientFactory.CreateClient("MangoAPI"))
            {
                ResponseDto? responseDto = null;

                try
                {
                    HttpRequestMessage httpRequest = new();
                    httpRequest.Headers.Add("Accept", "application/json");

                    if(withBearer)
                        httpRequest.Headers.Add("Authorization", $"Bearer {_tokenProvider.GetToken()}");

                    httpRequest.RequestUri = new Uri(requestDto.Url);
                    httpRequest.Method = requestDto.ApiType.ToMethod();

                    if (requestDto.Data != null)
                    {
                        string jsonString = JsonSerializer.Serialize(requestDto.Data);
                        httpRequest.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    }

                    HttpResponseMessage? httpResponse = await client.SendAsync(httpRequest);
                    responseDto = await httpResponse.ToResponse();

                    return responseDto;
                }
                catch (Exception ex)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                    };
                }
            }
        }
    }
}
