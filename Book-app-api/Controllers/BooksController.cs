using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookQuotesApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace BookQuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookQuotesContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookQuotesContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                _logger.LogWarning("User is not authenticated.");
                return Unauthorized();
            }
            
            var userId = int.Parse(userIdClaim);
            var books = await _context.Books.Where(b => b.UserId == userId).ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                _logger.LogWarning("User is not authenticated.");
                return Unauthorized();
            }
            
            var userId = int.Parse(userIdClaim);
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                _logger.LogWarning($"Book with id {id} not found for user {userId}.");
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Errors: {errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                _logger.LogWarning("User is not authenticated.");
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim);
            book.UserId = userId; // Set the UserId from the authenticated user's claims

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Book with id {book.Id} created for user {userId}.");

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                _logger.LogWarning("User is not authenticated.");
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim);

            // Ensure the book ID in the route matches the book ID in the request body
            if (id != book.Id)
            {
                _logger.LogWarning($"Mismatched book id. Route id: {id}, Book id: {book.Id}");
                return BadRequest("Mismatched book id.");
            }

            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (existingBook == null)
            {
                _logger.LogWarning($"Book with id {id} not found or does not belong to user {userId}.");
                return NotFound("Book not found.");
            }

            // Ensure the UserId is set correctly
            book.UserId = userId;

            // Update existing book properties
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Genre = book.Genre;
            existingBook.PublishedDate = book.PublishedDate;

            _context.Entry(existingBook).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Book with id {book.Id} updated for user {userId}.");

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                _logger.LogWarning("User is not authenticated.");
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim);
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                _logger.LogWarning($"Book with id {id} not found for user {userId}.");
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Book with id {book.Id} deleted for user {userId}.");

            return NoContent();
        }
    }
}
