using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;

// Use System.Text.Json source generation for even faster serialization
[JsonSerializable(typeof(Film))]
[JsonSerializable(typeof(Film[]))]
internal partial class AppJsonContext : JsonSerializerContext { }


public class AppDbContext
{
    private readonly string _connectionString;
    public AppDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}

public class Film
{
    [Column("film_id")]
    public int FilmId { get; set; }
    [Column("title")]
    public string Title { get; set; } = string.Empty;
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    [Column("release_year")]
    public int ReleaseYear { get; set; }
    [Column("language_id")]
    public int LanguageId { get; set; }
    [Column("original_language_id")]
    public int? OriginalLanguageId { get; set; }
    [Column("rental_duration")]
    public int RentalDuration { get; set; }
    [Column("rental_rate")]
    public decimal RentalRate { get; set; }
    [Column("length")]
    public int Length { get; set; }
    [Column("replacement_cost")]
    public decimal ReplacementCost { get; set; }
    [Column("rating")]
    public string Rating { get; set; } = string.Empty;
    [Column("last_update")]
    public DateTime LastUpdate { get; set; }
    [Column("special_features")]
    public string[] SpecialFeatures { get; set; } = Array.Empty<string>();
    // [Column("fulltext")]
    // public NpgsqlTypes.NpgsqlTsVector? FullText { get; set; } = null!;
}

public interface IFilmRepository
{
    Task<IEnumerable<Film>> GetAllAsync();
    Task<Film?> GetByIdAsync(int id);
    Task<Film> AddAsync(Film film);
    Task<Film?> UpdateAsync(Film film);
    Task<bool> DeleteAsync(int id);
}

public class FilmRepository : IFilmRepository
{
    private readonly AppDbContext _context;
    public FilmRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Film>> GetAllAsync()
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<Film>("SELECT * FROM film");
    }
    public async Task<Film?> GetByIdAsync(int id)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Film>("SELECT * FROM film WHERE film_id = @id", new { id });
    }
    public async Task<Film> AddAsync(Film film)
    {
        using var conn = _context.CreateConnection();
        var sql = @"INSERT INTO film (title, description, release_year, language_id, original_language_id, rental_duration, rental_rate, length, replacement_cost, rating, last_update, special_features, fulltext)
                    VALUES (@Title, @Description, @ReleaseYear, @LanguageId, @OriginalLanguageId, @RentalDuration, @RentalRate, @Length, @ReplacementCost, @Rating, @LastUpdate, @SpecialFeatures, @FullText)
                    RETURNING *;";
        return await conn.QuerySingleAsync<Film>(sql, film);
    }
    public async Task<Film?> UpdateAsync(Film film)
    {
        using var conn = _context.CreateConnection();
        var sql = @"UPDATE film SET title=@Title, description=@Description, release_year=@ReleaseYear, language_id=@LanguageId, original_language_id=@OriginalLanguageId, rental_duration=@RentalDuration, rental_rate=@RentalRate, length=@Length, replacement_cost=@ReplacementCost, rating=@Rating, last_update=@LastUpdate, special_features=@SpecialFeatures, fulltext=@FullText WHERE film_id=@FilmId RETURNING *;";
        return await conn.QueryFirstOrDefaultAsync<Film>(sql, film);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = _context.CreateConnection();
        var sql = "DELETE FROM film WHERE film_id = @id";
        var rows = await conn.ExecuteAsync(sql, new { id });
        return rows > 0;
    }
}
