using CadastroClientes.Application.DTOs;
using CadastroClientes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.API.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost("/[controller]")]
        public async Task<IActionResult> Cadastrar([FromBody] ClienteDto clienteDto)
        {
            var idCliente = await _clienteService.CriarAsync(clienteDto);
            return CreatedAtAction(nameof(Cadastrar), clienteDto);
        }

        [HttpGet("/[controller]")]
        public IActionResult Obter(Guid idCliente)
        {
            return Ok();
        }
    }
}