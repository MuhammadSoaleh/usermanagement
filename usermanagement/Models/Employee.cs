using System.ComponentModel.DataAnnotations.Schema;

namespace usermanagement.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phoneno { get; set; }
        public string email { get; set; }
        public string department { get; set; }
        [NotMapped]
        public int departmentid { get; set; }
        public string company { get; set; }
        [NotMapped]
        public int companyid { get; set; }
        public bool isactive { get; set; }
        
       // public DateTime DateTime { get; set; }=DateTime.Now;

        [NotMapped]
        public List<Department> Department { get; set; }
        [NotMapped]
        public List<company> companies  { get; set; }

        
    }
}
