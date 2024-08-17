using Microsoft.AspNetCore.Mvc;
using BookQuotesApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookQuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly BookQuotesContext _context;

        public QuotesController(BookQuotesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Quote>> GetQuotes()
        {
            return _context.Quotes.ToList();
        }
    }
}
