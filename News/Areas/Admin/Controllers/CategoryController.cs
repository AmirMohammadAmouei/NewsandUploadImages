using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Areas.Admin.Models;
using News.Areas.Admin.ViewModels;
using News.Context;


namespace News.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly NewsDbContext _context;

        public CategoryController(NewsDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.NewsCategories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            NewsCategoryViewModels viewModels = new NewsCategoryViewModels();
            return View(viewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsCategoryViewModels viewModels)
        {
            if (viewModels != null)
            {

                var category = new NewsCategory()
                {
                    Id = viewModels.Id,
                    Name = viewModels.Name,
                    CreationDate = DateTime.Now,
                };
                await _context.NewsCategories.AddAsync(category);
                await _context.SaveChangesAsync();


            }
            else
            {
                ModelState.AddModelError("", "خطا در ورود داده");
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(long id)
        {
            var findCategoryById = _context.NewsCategories.FirstOrDefault(x => x.Id == id);
            return View(findCategoryById);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(NewsCategory viewModels)
        {
            var findCategoryById = await _context.NewsCategories.FirstOrDefaultAsync(x => x.Id == viewModels.Id);

            if (findCategoryById == null)
            {
                return NotFound();
            }
            else
            {
                findCategoryById.Name = viewModels.Name;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var findCategoryById = _context.NewsCategories.Find(id);

            if (findCategoryById == null)
            {
                return NotFound();
            }
            else
            {
                _context.NewsCategories.Remove(findCategoryById);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
