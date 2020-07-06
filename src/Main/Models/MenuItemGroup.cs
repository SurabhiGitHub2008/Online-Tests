using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnlineOrderInfo.Models
{
    public class MenuItemGroup
    {
        [JsonProperty("MenuItems")]
        public List<MenuItem> MenuItems { get; set; }
    }
    
}