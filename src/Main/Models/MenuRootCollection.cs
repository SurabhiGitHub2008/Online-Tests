using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineOrderInfo.Models
{
    public class MenuRootCollection
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("DeliveryTypeCode")]
        public string DeliveryTypeCode { get; set; }
        [JsonProperty("MenuItemGroups")]
        public List<MenuItemGroup> MenuItemgroups { get; set; }
    }
}