using FirstTask.Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstTask.Repository.Interface
{
    public interface IListRepository
    {
        Task<IEnumerable<SelectListItem>> GetCountryListAsync();
        Task<IEnumerable<SelectListItem>> GetStateListByCountryIdAsync(long countryId);
        Task<IEnumerable<SelectListItem>> GetCityListByStateIdAsync(long stateId);
    }
}
