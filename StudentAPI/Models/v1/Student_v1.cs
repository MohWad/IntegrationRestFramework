using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Models.v1
{
    public class Student_v1
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Gender { get; set; }
    }
}
