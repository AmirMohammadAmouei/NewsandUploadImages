using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using News.Areas.Admin.Models;
using News.Areas.Admin.ViewModels;
using News.Context;

namespace News.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly NewsDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsController(NewsDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var result = _context.News.Include(x => x.Category).Select(x => new NewsViewModels
            {
                Id = x.Id,
                Title = x.Title,
                //NewsCategory = x.NewsCategory,
                Image = x.Image,
                Category = x.Category.Name,
                CreationDate = x.CreationDate,
                Description = x.Description,
                CountView = 1
            }).ToList();

            return View(result);
        }

        public IActionResult Create(ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "image");

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }

            var result = _context.NewsCategories.ToList();
            NewsModel news = new NewsModel();
            news.NewsCategory = new List<SelectListItem>();

            foreach (var item in result)
            {
                news.NewsCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsModel viewModels)
        {


            if (viewModels == null)
            {
                ModelState.AddModelError("", "خطا در درج اطلاعات!");
            }
            else
            {

                var files = HttpContext.Request.Form.Files;

                foreach (var Image in files)
                {

                    var file = Image;
                    if (file.Length > 0)
                    {
                        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "image");

                        if (files != null)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim();

                            using (var fileStream =
                                   new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                viewModels.Image = file.FileName;
                            }
                        }
                    }
                }
                var news = new NewsModel()
                {
                    Id = viewModels.Id,
                    Title = viewModels.Title,
                    CategoryId = viewModels.CategoryId,
                    CreationDate = DateTime.Now,
                    Description = viewModels.Description,
                    Image = viewModels.Image
                };

                await _context.AddAsync(news);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }
    }
}
