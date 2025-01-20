using BookStore.Data;
using BookStore.Filters;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly IAuthorizationService _authService;
        public bool CanEditBook { get; set; }

        public BooksController(BookStoreContext context, IAuthorizationService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: Books
        // Requires level 1 permission
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var authResult = await _authService.AuthorizeAsync(User, "CanUpdateBook");


            if (_context.Book == null)
            {
                return Problem("Entity set 'BookStoreContext.Book' is null.");
            }
            var books = from m in _context.Book select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                books = books.Where(s => s.Title!.ToUpper().Contains(searchString) ||
                                    s.Author!.ToUpper().Contains(searchString));

            }

            var viewModel = new BookListViewModel
            {
                Books = await books.ToListAsync(),
                CanEditBook = authResult.Succeeded
            };

            return View(viewModel);
        }


        // GET: HOME
        // Req level 1 perm
        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }



        // GET: Books/Details/5
        // Requires level 1 permission
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var authResult = await _authService.AuthorizeAsync(User, "CanUpdateBook");

            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookViewModel
            {
                Book = book,
                CanEditBook = authResult.Succeeded
            };

            return View(viewModel);
        }



        // GET: Books/Create
        // Requires level 2 permission
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var authResult = await _authService
                .AuthorizeAsync(User, "CanUpdateBook");
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }
            else
            {
                return View();
            }
        }

        // POST: Books/Create
        // Requires level 2 permission
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateModel]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Price")] Book book)
        {
            var authResult = await _authService
                .AuthorizeAsync(User, "CanUpdateBook");
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }
            else
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Books/Edit/5
        // Requires level 2 permission
        [HttpGet]
        [Authorize]
        [EnsureBookExist]
        public async Task<IActionResult> Edit(int? id)
        {

            var book = await _context.Book.FindAsync(id);
            var authResult = await _authService
                .AuthorizeAsync(User, "CanUpdateBook");
            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }
            else
            {
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }

        }

        // POST: Books/Edit/5
        // Requires level 2 permission
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [EnsureBookExist]
        [ValidateModel]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Price")] Book book)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Delete/5
        // Requires level 3 permission
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        // Requires level 3 permission
        [FeatureEnabled(IsEnabled = false)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
                //book.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //[HttpGet]
        public IActionResult Error(int statusCode)
        {

            if (statusCode == (int)HttpStatusCode.NotFound)
            {
                return View("Not Found");
            }
            return View(statusCode);

        }


        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
