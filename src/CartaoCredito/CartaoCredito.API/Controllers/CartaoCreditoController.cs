using CartaoCredito.Application;
using CartaoCredito.Application.Interfaces;
using CartaoDeCredito.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CartaoCredito.API.Controllers
{
    public class CartaoCreditoController : ControllerBase
    {
        private readonly ICartaoCreditoService _cartaoCreditoService;
        public CartaoCreditoController(ICartaoCreditoService cartaoCreditoService)
        {
            _cartaoCreditoService = cartaoCreditoService;
        }

        [HttpPost("/[controller]")]
        public async Task<IActionResult> Cadastrar([FromBody] CartaoCreditoDto cartaoCreditoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _cartaoCreditoService.CadastrarAsync(cartaoCreditoDto);
            
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            return CreatedAtAction(nameof(Cadastrar), resultado);
        }
    }
}
