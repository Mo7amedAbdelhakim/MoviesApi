using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.DTO;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly ApplicationDbContext _context;
        public MoviesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Movie> Add(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return (movie);
        }

        public Movie Delete(Movie movie)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public async Task<List<Movie>> GetAll(byte genreID = 0)
        {
            var movies = await _context.Movies.Where(m=>m.GenreId==genreID||genreID==0)
                .OrderByDescending(m=>m.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
            return movies;
        }


        public async Task<Movie> GetById(int id)
        {
            var model = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            return model;
        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}
