using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.API.Controllers
{
    public class ClientesController : Controller
    {
        [HttpPost("/[controller]")]
        public IActionResult Cadastrar()
        {
            return Ok();
        }
        
        [HttpGet("/[controller]")]
        public IActionResult Obter(Guid idCliente)
        {
            return Ok();
        }
    }
}