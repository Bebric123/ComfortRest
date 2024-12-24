using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public CountriesController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetCountries()
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var countries = await connection.QueryAsync("SELECT * FROM Countries");
            return Ok(countries);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<dynamic>> GetCountry(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var country = await connection.QuerySingleOrDefaultAsync("SELECT * FROM Countries WHERE ID_Countrie = @ID", new { ID = id });
            if (country == null) return NotFound();
            return Ok(country);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddCountry([FromBody] CountryUpdateModel country)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "INSERT INTO Countries(Name) VALUES (@Name)";
            var id = await connection.ExecuteScalarAsync<int>(sql, new { Name = country.Name });
            return CreatedAtAction(nameof(GetCountry), new { id }, new { id, Name = country.Name });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCountry(int id, [FromBody] CountryUpdateModel country)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "UPDATE Countries SET Name = @Name WHERE ID_Countrie = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Name = country.Name, ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "DELETE FROM Countries WHERE ID_Countrie = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }
}