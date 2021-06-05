using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Models.v2
{
    public class NewStudent_v2
    {
        public string NationalId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public int Level { get; set; }
    }
}
