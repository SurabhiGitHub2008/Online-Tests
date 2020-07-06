using OnlineOrderInfo.Models;
using OnlineOrderInfo.Service;
using System.Collections.Generic;
using System.Web.Http;

namespace OnlineOrderInfo.Controllers
{
    [RoutePrefix("api/MenuItems")]
    public class MenuItemController : ApiController
    {
        [HttpGet]
        [Route("Menu")]
        [System.Web.Http.Description.ResponseType(typeof(IEnumerable<Menu>))]
        public IHttpActionResult Menu(string name="", int restaurantId = 9245)
        {
            MenuItemRepository menuSvc = new MenuItemRepository();
            
            return Ok(menuSvc.GetMenuItems(name,restaurantId));
        }


    }
}
