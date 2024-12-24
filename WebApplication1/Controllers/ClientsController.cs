using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public ClientController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetClient()
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var clients = await connection.QueryAsync("SELECT * FROM Users");
            return Ok(clients);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<dynamic>> GetClient(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var client = await connection.QuerySingleOrDefaultAsync("SELECT * FROM Users WHERE ID_User = @ID", new { ID = id });
            if (client == null) return NotFound();
            return Ok(client);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] UserUpdateModel client)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "INSERT INTO Users (Name,LastName,PhoneNumber,Email) VALUES (@Name,@LastName,@PhoneNumber,@Email)";
            var id = await connection.ExecuteScalarAsync<int>(sql, new { Name = client.Name, LastName = client.LastName,PhoneNumber = client.PhoneNumber,Email = client.Email });
            return CreatedAtAction(nameof(GetClient), new { id }, new { id, Name = client.Name, LastName = client.LastName, PhoneNumber = client.PhoneNumber, Email = client.Email });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] UserUpdateModel client)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "UPDATE Users SET Name = @Name, LastName = @LastName, PhoneNumber = @PhoneNumber, Email = @Email WHERE ID_User = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Name = client.Name, LastName = client.LastName, PhoneNumber = client.PhoneNumber, Email = client.Email, ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "DELETE FROM Users WHERE ID_User = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }
}