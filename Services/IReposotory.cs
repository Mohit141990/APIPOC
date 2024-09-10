using WebAPI5.Database;
using WebAPI5.DataModel;

namespace WebAPI5.Services
{
    /// <summary>
    /// This is create for DI practical example
    /// </summary>
    public interface IReposotory
    {
        void AddCountries(Country country);
        List<Country> GetAllCountries();
        Country GetCountriesOnly(int id);

        
    }
}