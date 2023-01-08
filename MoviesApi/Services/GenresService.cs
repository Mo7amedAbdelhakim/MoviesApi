using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;
        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Genre> Add(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }
        public Genre Delete(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<List<Genre>> GetAll()
        {
            var genre = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
            return genre;
        }

        public async Task<Genre> GetById(byte id)
        {
            var model = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            return model;
        }

        public Task< bool> isValidGenre(byte id)
        {
         return _context.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre Update(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return genre;
        }
    }
}
