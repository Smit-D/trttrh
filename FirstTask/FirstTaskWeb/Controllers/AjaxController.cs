using FirstTask.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FirstTaskWeb.Controllers
{
    public class AjaxController : Controller
    {
        private readonly IListRepository _listRepository;
        public AjaxController(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }
        public async Task<JsonResult> GetStateListByCountryIdJson([FromQuery] long countryId)
        {
            var getList = await _listRepository.GetStateListByCountryIdAsync(countryId);
            return Json(getList);
        }

        public async Task<JsonResult> GetCityListByStateIdJson([FromQuery] long stateId)
        {
            var getList = await _listRepository.GetCityListByStateIdAsync(stateId);
            return Json(getList);
        }
    }
}
