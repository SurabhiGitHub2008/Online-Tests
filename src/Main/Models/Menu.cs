using Newtonsoft.Json;

namespace OnlineOrderInfo.Models
{
    public class Menu
    {
        public Menu( string menuName, string deliveryTypeCode, int menuItemId, string menuItemName)
        {
            MenuName = menuName;
            DeliveryTypeCode = deliveryTypeCode;
            MenuItemId = menuItemId;
            MenuItemName = menuItemName;
        }

        public Menu()
        { }

        public int MenuItemId{ get; set; }
        public string MenuItemName{ get; set; }
        [JsonProperty("Name")]
        public string MenuName { get; set; }
        [JsonProperty("DeliveryTypeCode")]
        public string DeliveryTypeCode { get; set; }
        
    }
}