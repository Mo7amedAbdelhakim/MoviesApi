using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.DTO;
using MoviesApi.Models;
using MoviesApi.Services;
using System;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        public MoviesController(IMoviesService moviesService, IGenresService service)
        {
            _moviesService = moviesService;
            _genresService = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateMovieDTO Dto)
        {
            if (Dto.Poster == null)
                return BadRequest("Poster is required!");

            if (!_allowedExtenstions.Contains(Path.GetExtension(Dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (Dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValidGenre = await _genresService.isValidGenre(Dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genere ID!");

            using var dataStream = new MemoryStream();
            await Dto.Poster.CopyToAsync(dataStream);
            if (ModelState.IsValid)
            {
                var movie = new Movie()
                {
                    Title = Dto.Title,
                    Year = Dto.Year,
                    Rate = Dto.Rate,
                    Poster = dataStream.ToArray(),
                    GenreId = Dto.GenreId,
                    StoreLine = Dto.StoreLine,
                };
                _moviesService.Add(movie);
                return Ok(movie);

            }
            return BadRequest();

        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
            
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var model = await _moviesService.GetById(id);
            if (model != null)
            {
                var dto = new MovieDetails
                {
                    Id = model.Id,
                    GenreId = model.GenreId,
                    GenreName = model.Genre.Name,
                    Poster = model.Poster,
                    StoreLine = model.StoreLine,
                    Title = model.Title,
                    Rate = model.Rate,
                    Year = model.Year
                };
                return Ok(dto);
            }
            return BadRequest("Movie Not Found");
        }
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreID)
        {
            var movies = await _moviesService.GetAll(genreID);
            return Ok(movies);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _moviesService.GetById(id);
            if(model != null)
            { 
           _moviesService.Delete(model);
              
                return Ok(model);
            }
            return NotFound();
        
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromForm] CreateMovieDTO Dto)
        {
            var model = await _moviesService.GetById(id);

            var isValidGenre = await _genresService.isValidGenre(Dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genere ID!");

            if (Dto.Poster != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(Dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (Dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");
                using var dataStream = new MemoryStream();
                await Dto.Poster.CopyToAsync(dataStream);
                model.Poster = dataStream.ToArray();
            }
                
            if (model != null)
            { 
             model.Title= Dto.Title;
                model.Year= Dto.Year;
                model.GenreId= Dto.GenreId;
                model.StoreLine = Dto.StoreLine;
                model.Rate = Dto.Rate;
               
                _moviesService.Update(model);
                return Ok(model);
            
            }
            return NotFound();

        }
    }
}