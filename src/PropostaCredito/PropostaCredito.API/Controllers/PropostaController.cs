using Microsoft.AspNetCore.Mvc;
using PropostaCredito.Application.DTOs;
using PropostaCredito.Application.Interfaces;

namespace PropostaCredito.API.Controllers
{
    public class PropostaController : ControllerBase
    {
        // Aqui você pode injetar o repositório de propostas e outros serviços necessários
        private readonly IPropostaService _propostaService;
        public PropostaController(IPropostaService propostaService)
        {
            _propostaService = propostaService;
        }

        [HttpPost("/[controller]")]
        public async Task<IActionResult> Proposta([FromBody] PropostaDto propostaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Aqui você pode chamar o serviço para cadastrar a proposta
            var resultado = await _propostaService.CadastrarAsync(propostaDto);
            return CreatedAtAction(nameof(Proposta), resultado);
        }
    }
}
