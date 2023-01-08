using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.DTO;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _service;
        public GenresController(IGenresService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
        if (ModelState.IsValid)
            {
              var g = await _service.GetAll();
                return Ok(g);
            }
           return BadRequest();
        }
        [HttpPost]
        public async Task<ActionResult> Add(CreateGenreDto dto)
        {
            var Genre= new Genre()
            {
                Name = dto.Name
            };
            await _service.Add(Genre);
           return Ok(Genre);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(byte id,CreateGenreDto dto)
        {
            var model = await _service.GetById(id);
            if(model != null)
            {
                model.Name = dto.Name;
                _service.Update(model);
                return Ok(model);
            }
            return NotFound();
          
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(byte id)
        {
            var model = await _service.GetById(id);
            if (model != null)
            {
                _service.Delete(model);
                return Ok(model);
            }
            return NotFound();

        }

    }
}
