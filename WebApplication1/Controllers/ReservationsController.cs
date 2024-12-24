using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly DatabaseContext _dbContext;

    public ReservationsController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<dynamic>>> GetReservation()
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var reservations = await connection.QueryAsync("SELECT * FROM Reservations");
            return Ok(reservations);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<dynamic>> GetReservations(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var reservation = await connection.QuerySingleOrDefaultAsync("SELECT * FROM Reservations WHERE ID_Reservation = @ID", new { ID = id });
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddReservations([FromBody] ReservationUpdateModel reservation)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "INSERT INTO Reservations (ID_Admin,ID_User,ID_Tour,DateReservation) VALUES (@ID_Admin,@ID_User,@ID_Tour,@DateReservation)";
            var id = await connection.ExecuteScalarAsync<int>(sql, new { ID_Admin = reservation.ID_Admin, ID_User = reservation.ID_User, ID_Tour = reservation.ID_Tour, DateReservation = reservation.DateReservation });
            return CreatedAtAction(nameof(GetReservations), new { id }, new { id, ID_Admin = reservation.ID_Admin, ID_User = reservation.ID_User, ID_Tour = reservation.ID_Tour, DateReservation = reservation.DateReservation });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservations(int id, [FromBody] ReservationUpdateModel reservation)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "UPDATE Reservations SET ID_Admin = @ID_Admin, ID_User = @ID_User, ID_Tour = @ID_Tour, DateReservation = @DateReservation WHERE ID_Reservation = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { ID_Admin = reservation.ID_Admin, ID_User = reservation.ID_User, ID_Tour = reservation.ID_Tour, DateReservation = reservation.DateReservation, ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservations(int id)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var sql = "DELETE FROM Reservations WHERE ID_Reservation = @ID";
            var rowsAffected = await connection.ExecuteAsync(sql, new { ID = id });
            if (rowsAffected == 0) return NotFound();
            return NoContent();
        }
    }
}