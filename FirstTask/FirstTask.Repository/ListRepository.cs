using FirstTask.Entities.Data;
using FirstTask.Repository.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstTask.Repository
{
    public class ListRepository : IListRepository
    {
        private readonly FirstTaskDBContext _context;
        public ListRepository(FirstTaskDBContext context)
        {
            _context = context;
        }
       

        public async Task<IEnumerable<SelectListItem>> GetCountryListAsync()
        {
            return await _context.Countries.Where(x => x.DeletedAt == null).Select(x => new SelectListItem()
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName,
            }).ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetStateListByCountryIdAsync(long countryId)
        {
            return await _context.States.Where(state => state.DeletedAt == null && state.CountryId == countryId).Select(x => new SelectListItem()
            {
                Value = x.StateId.ToString(),
                Text = x.StateName,
            }).ToListAsync();
        }
        public async Task<IEnumerable<SelectListItem>> GetCityListByStateIdAsync(long stateId)
        {
            return await _context.Cities.Where(city => city.DeletedAt == null && city.StateId == stateId).Select(x => new SelectListItem()
            {
                Value = x.CityId.ToString(),
                Text = x.CityName,
            }).ToListAsync();
        }
    }
}
