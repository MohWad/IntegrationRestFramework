using AutoMapper;
using Integration.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleAPI.Services;
using StudentAPI.DTOs;
using StudentAPI.Models.v1;
using StudentAPI.Models.v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Controllers.v2
{
    [Route("api/v{version:ApiVersion}/students")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
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

        [HttpPost]
        [Authorize(Policy = "CreateStudentV2")]
        [ApiVersion("2.0")]
        public IActionResult AddNewStudent(NewStudent_v2 newStudent)
        {
            var clientId = User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value ?? "Anonymous";

            _logger.LogInformation("Begin processing {endpoint} - Client: {client}", HttpContext.Request.Path.Value, clientId);

            var studentDTO = _mapper.Map<StudentDTO_v2>(newStudent);
            studentDTO = _studentService.AddStudent(studentDTO);

            var student = _mapper.Map<Student_v2>(studentDTO);

            var response = new Response<Student_v2>(student);

            _logger.LogInformation("End processing {endpoint} - Client: {client}", HttpContext.Request.Path.Value, clientId);

            return Ok(response);
        }
    }
}
