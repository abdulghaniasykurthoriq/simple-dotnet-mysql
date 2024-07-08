using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyRestfulApi.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestfulApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public PeopleController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        public async Task<IEnumerable<Person>> GetPeople()
        {
            using var db = _databaseService.CreateConnection();
            db.Open();
            var people = await db.QueryAsync<Person>("SELECT * FROM person");
            return people.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            using var db = _databaseService.CreateConnection();
            db.Open();
            var person = await db.QueryFirstOrDefaultAsync<Person>("SELECT * FROM person WHERE id = @Id", new { Id = id });
            if (person == null)
            {
                return NotFound(new { Status = "Error", Message = $"Person with ID {id} not found" });
            }
            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult> AddPerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var db = _databaseService.CreateConnection();
            db.Open();
            string sql = "INSERT INTO person (name) VALUES (@Name); SELECT LAST_INSERT_ID();";
            var newId = await db.ExecuteScalarAsync<int>(sql, new { Name = person.Name });
            person.Id = newId;
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePerson(int id, [FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var db = _databaseService.CreateConnection();
            db.Open();
            var rowsAffected = await db.ExecuteAsync("UPDATE person SET name = @Name WHERE id = @Id", new { Name = person.Name, Id = id });
            if (rowsAffected == 0)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(int id)
        {
            using var db = _databaseService.CreateConnection();
            db.Open();
            var rowsAffected = await db.ExecuteAsync("DELETE FROM person WHERE id = @Id", new { Id = id });
            if (rowsAffected == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
