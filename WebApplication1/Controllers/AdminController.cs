using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public AdminController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetAdmin()
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var admins = await connection.QueryAsync("SELECT * FROM Admin");
            return Ok(admins);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<dynamic>> GetAdmin(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var admin = await connection.QuerySingleOrDefaultAsync("SELECT * FROM Admin WHERE ID_Admin = @ID", new { ID = id });
            if (admin == null) return NotFound();
            return Ok(admin);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAdmin([FromBody] AdminUpdateModel admin)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "INSERT INTO Admin(Name,Password) VALUES (@Name, @Password)";
            var id = await connection.ExecuteScalarAsync<int>(sql, new { Name = admin.Name, Password = admin.Password });
            return CreatedAtAction(nameof(GetAdmin), new { id }, new { id, Name = admin.Name, Password = admin.Password });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmin(int id, [FromBody] AdminUpdateModel admin)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "UPDATE Admin SET Name = @Name, Password = @Password WHERE ID_Admin = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Name = admin.Name, Password = admin.Password, ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "DELETE FROM Admin WHERE ID_Admin = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }
}