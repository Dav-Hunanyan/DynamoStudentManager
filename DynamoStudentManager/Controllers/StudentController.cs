using Amazon.DynamoDBv2.DataModel;
using DynamoStudentManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamoStudentManager.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDynamoDBContext _context;
        public StudentController(IDynamoDBContext context)
        {
            _context = context;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetById(int studentId)
        {
            var student = await _context.LoadAsync<Student>(studentId);
            if (student is null) return NotFound();
            return Ok(student);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.ScanAsync<Student>(default).GetRemainingAsync();
            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student studentRequest)
        {
            var student = await _context.LoadAsync<Student>(studentRequest.Id);
            if (student is not null) return BadRequest($"Student with {studentRequest.Id} id already exicts");
            await _context.SaveAsync(studentRequest);
            return Ok(studentRequest);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int studentID)
        {
            var student = await _context.LoadAsync<Student>(studentID);
            if (student is null) return NotFound();
            await _context.DeleteAsync(student);
            return NoContent();           
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student studentRequest)
        {
            var student = await _context.LoadAsync<Student>(studentRequest.Id);
            if (student is null) return NotFound();
            await _context.SaveAsync(studentRequest);
            return Ok(studentRequest);
           
        }
    }

}
