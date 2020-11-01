using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PalestranteController : ControllerBase
    {
        public readonly IProAgilRepository _repositorio;

        public PalestranteController(IProAgilRepository repositorio)
        {
            this._repositorio = repositorio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try    
            {
                var results = await _repositorio.GetAllPalestranteAsync(true);
                return BadRequest(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try    
            {
                var results = await _repositorio.GetPalestranteAsyncById(id, true);
                return BadRequest(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
            
        }

        [HttpGet("getByNome{nome}")]
        public async Task<IActionResult> Get(string nome)
        {
            try    
            {
                var results = await _repositorio.GetAllPalestranteAsyncByNome(nome, true);
                return BadRequest(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try    
            {
                _repositorio.Add(model);
                if(await _repositorio.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}",model);
                }
                return BadRequest();
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(int palestranteId, Palestrante model)
        {
            try    
            {
                var palestrante = await _repositorio.GetPalestranteAsyncById(palestranteId, false);

                if(palestrante == null) return NotFound();

                _repositorio.UpDate(model);
                if(await _repositorio.SaveChangesAsync())
                {
                    return Created($"/api/Palestrante/{model.Id}",model);
                }
                return BadRequest();
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int palestranteId)
        {
            try    
            {
                var palestrante = await _repositorio.GetPalestranteAsyncById(palestranteId, false);

                if(palestrante == null) return NotFound();

                _repositorio.Delete(palestrante);

                if(await _repositorio.SaveChangesAsync())
                    return Ok();
                
                return BadRequest();
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
    }
}