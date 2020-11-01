using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public readonly IProAgilRepository _repositorio;

        public EventoController(IProAgilRepository repositorio)
        {
            this._repositorio = repositorio;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try    
            {
                var results = await _repositorio.GetAllEventoAsync(true);
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
                var results = await _repositorio.GetEventoAsyncById(id, true);
                return Ok(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpGet("getByTema{tema}")]
        public async Task<IActionResult> GetAction(string tema)
        {
            try    
            {
                var results = await _repositorio.GetAllEventoAsyncByTema(tema, true);
                return Ok(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
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
        public async Task<IActionResult> Put(int eventoId, Evento model)
        {
            try    
            {
                var evento = await _repositorio.GetEventoAsyncById(eventoId, false);

                if(evento == null) return NotFound();

                _repositorio.UpDate(model);
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try    
            {
                var evento = await _repositorio.GetEventoAsyncById(eventoId, false);

                if(evento == null) return NotFound();

                _repositorio.Delete(evento);

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