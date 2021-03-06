using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public readonly IProAgilRepository _repositorio;
        public readonly IMapper _mapper;

        public EventoController(IProAgilRepository repositorio, IMapper mapper)
        {
            this._repositorio = repositorio;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try    
            {
                var eventos = await _repositorio.GetAllEventoAsync(true);
                var result = _mapper.Map<EventoDTO[]>(eventos);

                return Ok(result);
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
                var evento = await _repositorio.GetEventoAsyncById(id, true);
                var result = _mapper.Map<EventoDTO>(evento);

                return Ok(result);
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

        [HttpPost("upload")]
        public async Task<IActionResult> upload()
        {
            try    
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources","Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\""," ").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok();

                }

                return BadRequest();
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDTO model)
        {
            try    
            {
                var evento = _mapper.Map<Evento>(model);
                _repositorio.Add(evento);
                if(await _repositorio.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}",_mapper.Map<EventoDTO>(evento));
                }
                return BadRequest();
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDTO model)
        {
            try    
            {
                var evento = await _repositorio.GetEventoAsyncById(id, false);
                if(evento == null) return NotFound();

                var lotesExcluir = evento.Lotes.Where(l => !model.Lotes.Any(m => m.Id.Equals(l.Id))).ToArray();
                var redesExcluir = evento.RedesSociais.Where(r => !model.RedesSociais.Any(m => m.Id.Equals(r.Id))).ToArray();

                if(lotesExcluir.Any())
                    _repositorio.DeleteRange(lotesExcluir);

                if(redesExcluir.Any())
                    _repositorio.DeleteRange(redesExcluir);

                _mapper.Map(model, evento);

                _repositorio.UpDate(evento);
                if(await _repositorio.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}",_mapper.Map<EventoDTO>(evento));
                }
                return BadRequest();
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try    
            {
                var evento = await _repositorio.GetEventoAsyncById(id, false);

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