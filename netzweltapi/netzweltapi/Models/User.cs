namespace netzweltapi.Models
{
    public class User
    {
        public string username { get; set; } = "";
        public string displayName { get; set; } = "";
        public List<string> roles { get; set; }
    }

    public class UserParams
    {
        public string username { get; set; } = "";
        public string password { get; set; } = "";
    }
}
