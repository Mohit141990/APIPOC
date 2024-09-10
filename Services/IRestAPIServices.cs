using WebAPI5.Models.RestAPI;

namespace WebAPI5.Services
{
    public interface IRestAPIServices :IDisposable
    {

        Task<RestAPIModel> GetCountriesAll();
        Task<ExchangeRateResponse> GetExchnageRate(string currencyCode);
        Task<ExchangeConvertorResponse> GetExchnageRateConevertor(string FromCurrencyCode, string ToCurrencyCode);

        Task<ExchangeConvertorResponse> GetGeoLocation(string ipAddress);
        Task<ExchangeConvertorResponse> GetGeoAstro(string Location);

        Task SendSmsAsync(string to, string message);
        Task MakeCallAsync(string to, string url);

        //Task<JokesModel> GetJokes(PagingModel model);
    }
}
