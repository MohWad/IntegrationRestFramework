using AutoMapper;
using Integration.Common.Caching.Filters;
using Integration.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleAPI.Services;
using StudentAPI.DTOs;
using StudentAPI.Models.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Controllers.v1
{
    [Route("api/v{version:ApiVersion}/students")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        public StudentController(ILoggerFactory loggerFactory, IStudentService studentService, IMapper mapper)
        {
            _logger = loggerFactory.CreateLogger("StudentController");
            _studentService = studentService;
            _mapper = mapper;
        }


        /// <summary>
        /// Get a list of all students
        /// </summary>
        /// <param name="request">PageNumber and PageSize</param>
        /// <returns>List of students</returns>
        [HttpPost("all")]
        //[Authorize(Policy = "GetStudents")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student_v1>))]
        public ActionResult<IEnumerable<Student_v1>> GetAllStudent(AllStudents_v1 request)
        {
            var clientId =  User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value ?? "Anonymous";

            _logger.LogInformation("Begin processing {endpoint} - Client: {client}", HttpContext.Request.Path.Value, clientId);

            var studentDTOs = _studentService.GetAllStudents(request.Pagination.PageNumber, request.Pagination.PageSize);

            var students = _mapper.Map<IEnumerable<Student_v1>>(studentDTOs);

            var response = new Response<IEnumerable<Student_v1>>(students);

            _logger.LogInformation("End processing {endpoint} - Client: {client}", HttpContext.Request.Path.Value, clientId);

            return Ok(response);
        }

        [HttpPost("bygender")]
        [Authorize(Policy = "GetStudents")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student_v1>))]
        public ActionResult<IEnumerable<Student_v1>> GetStudentByGender(StudentsByGender_v1 request)
        {
            var clientId = User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value ?? "Anonymous";

            _logger.LogInformation("Begin processing {endpoint} - Client: {client}", HttpContext.Request.Path.Value, clientId);

            var studentDTOs = _studentService.GetStudentsByGender(request.Gender, request.Pagination.PageNumber, request.Pagination.PageSize);

            var students = _mapper.Map<IEnumerable<Student_v1>>(studentDTOs);

            var response = new Response<IEnumerable<Student_v1>>(students);

            _logger.LogInformation("End processing {endpoint} - Client: {client}", HttpContext.Request.Path.Value, clientId);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = "CreateStudent")]
        [ApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student_v1))]
        public IActionResult AddNewStudent(NewStudent_v1 newStudent)
        {
            var clientId = User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value ?? "Anonymous";

            _logger.LogInformation("Begin processing {endpoint} - Client: {client}", HttpContext.Request.Path.Value, clientId);

            var studentDTO = _mapper.Map<StudentDTO_v1>(newStudent);
            studentDTO = _studentService.AddStudent(studentDTO);

            var student = _mapper.Map<Student_v1>(studentDTO);

            var response = new Response<Student_v1>(student);

            _logger.LogInformation("End processing {endpoint} - Client: {client}", HttpContext.Request.Path.Value, clientId);

            return Ok(response);
        }
    }
}
