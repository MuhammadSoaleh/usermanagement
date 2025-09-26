using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace usermanagement.Models
{
    public class Department
    {
        public int id { get; set; }

        public string departmentname { get; set; }
        [NotMapped]
        // This will hold the selected company ID from the dropdown
        public int companyid { get; set; }
        
        public string companyname { get; set; }

        // Not mapped, only used for dropdown list population
        [NotMapped]
        public List<company> Companies { get; set; }

        public bool isactive { get; set; } = true;
    }
}
