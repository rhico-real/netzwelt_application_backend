namespace netzweltapi.Models
{
    public class TerritoryData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public List<TerritoryData> children { get; set; }
    }

    public class Data
    {
        public List<TerritoryData> data { get; set; }
    }
}
