using CadastroClientes.Application.DTOs;
using CadastroClientes.Application.Interfaces;
using CadastroClientes.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.API.Controllers
{
    public class ClientesController(IClienteService clienteService) : ControllerBase
    {
        private readonly IClienteService _clienteService = clienteService;

        [HttpPost("/[controller]")]
        public async Task<IActionResult> Cadastrar([FromBody] ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ClientesResult cadastro = await _clienteService.CadastrarAsync(clienteDto);
            return CreatedAtAction(nameof(Cadastrar), cadastro);
        }

        [HttpGet("/[controller]")]
        public async Task<IActionResult> Obter([FromQuery] string cpf)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ClientesResult cliente = await _clienteService.ObterAsync(cpf);
            return Ok(cliente);
        }
    }
}