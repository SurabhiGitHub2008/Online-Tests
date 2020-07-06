using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnlineOrderInfo.Models
{
    public class Restaurant
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("RestaurantMenus")]
        public List<RestaurantMenu> Menus { get; set; }
    }

    
}