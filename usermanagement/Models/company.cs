namespace usermanagement.Models
{
    public class company
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public bool isactive { get; set; } = true;

    }
}
