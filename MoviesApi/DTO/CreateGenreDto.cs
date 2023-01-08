using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTO
{
    public class CreateGenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
