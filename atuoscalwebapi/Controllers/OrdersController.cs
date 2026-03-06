using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using atuoscalwebapi.Models;


namespace atuoscalwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration _config;

        public OrdersController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Orders order)
        {
            var connectionString = _config.GetConnectionString("Default");

            using SqlConnection conn = new SqlConnection(connectionString);

            await conn.OpenAsync();

            var cmd = new SqlCommand(
                "INSERT INTO Orders(CustomerName,ProductName,CreatedDate) VALUES(@c,@p,GETDATE())",
                conn);

            cmd.Parameters.AddWithValue("@c", order.CustomerName);
            cmd.Parameters.AddWithValue("@p", order.ProductName);

            await cmd.ExecuteNonQueryAsync();

            return Ok("Order Created");
        }
    }
}
