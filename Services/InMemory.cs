using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI5.DataModel;

namespace WebAPI5.Services
{
    public class InMemory :IReposotory
    {
        private List<Country> _country;
        public InMemory()
        {
            _country = new List<Country>() {
                new Country{ Id=1,Name="India"},
                new Country{ Id=2,Name="UK"},
                new Country{ Id=3,Name="USA"},
                new Country{ Id=4,Name="India"}
            };
        }

        
        public List<Country> GetAllCountries() {
            //await Task.Delay(1000);
            return _country;
        }
        public Country GetCountriesOnly(int id)
        {
            //await Task.Delay(1000);
            return  _country.FirstOrDefault(c=>c.Id==id);
        }
        public void AddCountries(Country country)
        {
            _country.Add(country);
           
        }
    }
}
