
using Microsoft.AspNetCore.Mvc;
using AssignmentManagement.Core;

namespace AssignmentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _service;

        public AssignmentController(IAssignmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.ListAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] Assignment assignment)
        {
            _service.AddAssignment(assignment);
            return CreatedAtAction(nameof(GetAll), new { assignment.Title }, assignment);
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var success = _service.DeleteAssignment(name);
            if (!success)
                return NotFound();
            return Ok();
        }
    }
}
