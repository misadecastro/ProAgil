using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.WebAPI.Data;
using ProAgil.WebAPI.Model;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        public readonly DataContext _context;

        public EventoController(DataContext context)
        {
            this._context = context;

        }

        [HttpGet]
        public async Task<IActionResult> GetAction()
        {
            try    
            {
                var results = await _context.Eventos.ToListAsync();
                return Ok(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAction(int id)
        {
            try    
            {
                var results = await _context.Eventos.FirstOrDefaultAsync(e => e.EventoId.Equals(id));
                return Ok(results);
            }
            catch(System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
    }
}