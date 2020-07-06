using Newtonsoft.Json;


namespace OnlineOrderInfo.Models
{
    public class MenuItem
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}