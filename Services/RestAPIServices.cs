using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net.Http;
using WebAPI5.Models.RestAPI;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace WebAPI5.Services
{
    public class RestAPIServices : IRestAPIServices
    {
        private bool disposedValue = false;
        private readonly IConfiguration _configuration;

        private readonly HttpClient _httpClient;
        private readonly string _baseURL;
        private readonly string _baseURL_Exchange;
        private readonly string _apiKey_Exchange;

        private readonly string _baseURL_geo;
        private readonly string _apiKey_geo;


        private readonly string _AccountSid;
        private readonly string _AuthToken;
        private readonly string _PhoneNumber;

        public RestAPIServices(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseURL = configuration["RestCountry:BaseUrl"];
            _baseURL_Exchange = configuration["ExchangeRate:BaseUrl"];
            _apiKey_Exchange = configuration["ExchangeRate:APIKey"];

            _baseURL_geo = configuration["ipgeolocation:BaseUrl"];
            _apiKey_geo = configuration["ipgeolocation:APIKey"];

            _AccountSid = configuration["Twilio:AccountSid"];
            _AuthToken = configuration["Twilio:AuthToken"];
            _PhoneNumber = configuration["Twilio:PhoneNumber"];

            TwilioClient.Init(_AccountSid, _AuthToken);

        }

        public async Task SendSmsAsync(string to, string message)
        {
            await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber(_PhoneNumber),
                to: new PhoneNumber(to)
            );
        }

        public async Task MakeCallAsync(string to, string url)
        {
            await CallResource.CreateAsync(
                to: new PhoneNumber(to),
                from: new PhoneNumber(_PhoneNumber),
                url: new Uri(url)
            );
        }



        public async Task<RestAPIModel> GetCountriesAll()
        {
            var url = _baseURL + $"all";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RestAPIModel>(content);
        }

        public async Task<ExchangeRateResponse> GetExchnageRate(string currencyCode)
        {
            var url = _baseURL_Exchange + _apiKey_Exchange + $"/latest/" + currencyCode;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeRateResponse>(content);
        }

        public async Task<ExchangeConvertorResponse> GetExchnageRateConevertor(string FromCurrencyCode, string ToCurrencyCode)
        {
            var url = _baseURL_Exchange + _apiKey_Exchange + $"/enriched/" + FromCurrencyCode + "/" + ToCurrencyCode;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeConvertorResponse>(content);
        }

        public async Task<ExchangeConvertorResponse> GetGeoLocation(string ipAddress)
        {
            //'https://api.ipgeolocation.io/ipgeo?apiKey=API_KEY&ip=8.8.8.8'
            var url = _baseURL_geo + $"/ipgeo?apiKey=" + _apiKey_geo + "&ip=" + ipAddress;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeConvertorResponse>(content);
        }

        public async Task<ExchangeConvertorResponse> GetGeoAstro(string Location)
        {
            //'https://api.ipgeolocation.io/ipgeo?apiKey=API_KEY&ip=8.8.8.8'
            var url = _baseURL_geo + $"/astronomy?apiKey=" + _apiKey_geo + "&location=" + Location;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeConvertorResponse>(content);
        }

        //public async Task<JokesModel> GetJokes(PagingModel model)
        //{
        //    model.PageSize = model.PageSize <= 10 ? model.PageSize : 10;
        //    var range = string.Empty;
        //    if (model.PageNumber == 1)
        //    {
        //        var f = model.PageNumber;
        //        var t = model.PageSize * model.PageNumber;
        //        range = f + "-" + t;
        //    }
        //    else if (model.PageNumber > 1)
        //    {
        //        var f = model.PageSize * (model.PageNumber - 1) + 1;
        //        var t = model.PageSize * (model.PageNumber);
        //        range = f + "-" + t;
        //    }
        //    //var url = "https://v2.jokeapi.dev/joke/Programming?amount=10";
        //    //https://v2.jokeapi.dev/joke/Programming?idRange=10-20&amount=10
        //    var url = "https://v2.jokeapi.dev/joke/Programming?idRange=" + range + "&amount=" + model.PageSize.ToString();

        //    var response = await _httpClient.GetAsync(url);
        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<JokesModel>(content);
        //}


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
