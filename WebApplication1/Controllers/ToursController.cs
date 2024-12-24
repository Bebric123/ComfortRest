using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics.Metrics;

[ApiController]
[Route("api/[controller]")]
public class ToursController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public ToursController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetTours()
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var tours = await connection.QueryAsync("SELECT * FROM Tours");
            return Ok(tours);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<dynamic>> GetTours(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var tour = await connection.QuerySingleOrDefaultAsync("SELECT * FROM Tours WHERE ID_Tour = @ID", new { ID = id });
            if (tour == null) return NotFound();
            return Ok(tour);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddReservations([FromBody] TourUpdateModel tour)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "INSERT INTO Tours (Name,Cost,ID_Countrie,Description) VALUES (@Name,@Cost,@ID_Countrie,@Description)";
            var id = await connection.ExecuteScalarAsync<int>(sql, new { Name = tour.Name, Cost = tour.Cost, ID_Countrie = tour.ID_Countrie, Description = tour.Description });
            return CreatedAtAction(nameof(GetTours), new { id }, new { id, Name = tour.Name, Cost = tour.Cost, ID_Countrie = tour.ID_Countrie, Description = tour.Description });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTours(int id, [FromBody] TourUpdateModel tour)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "UPDATE Tours  SET Name = @Name, Cost = @Cost, ID_Countrie = @ID_Countrie, Description = @Description WHERE ID_Tour = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Name = tour.Name, Cost = tour.Cost, ID_Countrie = tour.ID_Countrie, Description = tour.Description, ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTours(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "DELETE FROM Tours WHERE ID_Tour = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }
}