using StudentAPI.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace SampleAPI.Services
{
    public class StudentService : IStudentService
    {
        private List<StudentDTO_v1> _students = new List<StudentDTO_v1>
        {
            new StudentDTO_v1 { Id = 101, FirstName = "Mohammed", LastName = "Abdullah", Gender = "Male" },
            new StudentDTO_v1 { Id = 102, FirstName = "Saleh", LastName = "Ali", Gender = "Male" },
            new StudentDTO_v1 { Id = 103, FirstName = "Ahmed", LastName = "Abdulrahman", Gender = "Male" },
            new StudentDTO_v1 { Id = 104, FirstName = "Kahled", LastName = "Abdulaziz", Gender = "Male" },
            new StudentDTO_v1 { Id = 105, FirstName = "Waleed", LastName = "Ahmed", Gender = "Male" },
            new StudentDTO_v1 { Id = 106, FirstName = "Majed", LastName = "Hamad", Gender = "Male" },
            new StudentDTO_v1 { Id = 107, FirstName = "Turki", LastName = "Abdulmalik", Gender = "Male" },
            new StudentDTO_v1 { Id = 108, FirstName = "Ziad", LastName = "Saleh", Gender = "Male" },
            new StudentDTO_v1 { Id = 109, FirstName = "Reem", LastName = "Mahmood", Gender = "Female" },
            new StudentDTO_v1 { Id = 110, FirstName = "Lyla", LastName = "Abdullah", Gender = "Female" },
            new StudentDTO_v1 { Id = 111, FirstName = "Fatima", LastName = "Mohammed", Gender = "Female" },
            new StudentDTO_v1 { Id = 112, FirstName = "Nora", LastName = "Ibrahim", Gender = "Female" },
            new StudentDTO_v1 { Id = 113, FirstName = "Sara", LastName = "Ahmed", Gender = "Female" }
        };

        private List<StudentDTO_v2> _students_v2 = new List<StudentDTO_v2>
        {
            new StudentDTO_v2 { Id = 101, FirstName = "Mohammed", LastName = "Abdullah", Level = 3, Phone = "050647851" },
            new StudentDTO_v2 { Id = 102, FirstName = "Saleh", LastName = "Ali", Level = 2, Phone = "04785123695"  },
            new StudentDTO_v2 { Id = 103, FirstName = "Ahmed", LastName = "Abdulrahman", Level = 4, Phone = "147852158"  },
            new StudentDTO_v2 { Id = 104, FirstName = "Kahled", LastName = "Abdulaziz", Level = 4, Phone = "7485952158"  },
            new StudentDTO_v2 { Id = 105, FirstName = "Waleed", LastName = "Ahmed", Level = 4, Phone = "264782158"  },
            new StudentDTO_v2 { Id = 106, FirstName = "Reem", LastName = "Mahmood", Level = 4, Phone = "9876542121"  },
            new StudentDTO_v2 { Id = 107, FirstName = "Fatima", LastName = "Mohammed", Level = 4, Phone = "7412589632"  },
        };

        public IEnumerable<StudentDTO_v1> GetAllStudents(int page, int pageSize)
        {
            return _students
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize);
        }

        public IEnumerable<StudentDTO_v1> GetStudentsByGender(string gender, int page, int pageSize)
        {
            return _students
                    .Where(s => s.Gender == gender)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize);
        }

        public StudentDTO_v1 GetStudent(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public StudentDTO_v1 AddStudent(StudentDTO_v1 student)
        {
            student.Id = _students.Count + 1;

            _students.Add(student);

            return student;
        }

        public StudentDTO_v2 AddStudent(StudentDTO_v2 student)
        {
            student.Id = _students_v2.Count + 1;

            _students_v2.Add(student);

            return student;
        }

        public bool CheckStudentNationalId(string nationalId)
        {
            if (nationalId == null)
                return false;

            if (nationalId.Length == 10)
                return true;

            return false;
        }
    }
}
