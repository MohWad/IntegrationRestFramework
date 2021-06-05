using StudentAPI.DTOs;
using System.Collections.Generic;

namespace SampleAPI.Services
{
    public interface IStudentService
    {
        IEnumerable<StudentDTO_v1> GetAllStudents(int page, int pageSize);
        IEnumerable<StudentDTO_v1> GetStudentsByGender(string gender, int page, int pageSize);
        StudentDTO_v1 GetStudent(int id);
        StudentDTO_v1 AddStudent(StudentDTO_v1 student);
        StudentDTO_v2 AddStudent(StudentDTO_v2 student);
        bool CheckStudentNationalId(string nationalId);
    }
}
