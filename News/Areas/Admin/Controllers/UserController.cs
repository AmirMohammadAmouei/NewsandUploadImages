using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Areas.Admin.Models;
using News.Areas.Admin.ViewModels;
using News.Common;
using News.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly NewsDbContext _context;
        private readonly IPasswordHasher _hashedPassword;

        public UserController(NewsDbContext context, IPasswordHasher hashedPassword)
        {
            _context = context;
            _hashedPassword = hashedPassword;
        }

        [HttpGet]
        public async Task<IActionResult> Index(UserFilterViewModels viewModels)
         {
            int skipCount = (viewModels.Page - 1) * viewModels.Limit;

            var query =  _context.Users.Where(x => 
            (string.IsNullOrEmpty(viewModels.UserName) ? true : x.UserName.Contains(viewModels.UserName))
            && (string.IsNullOrEmpty(viewModels.PersonalId) ? true : x.PersonalId.Contains(viewModels.PersonalId)));

            var result = await query.Select(x => new UserViewModels
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                PersonalId = x.PersonalId,
                PhoneNumber = x.PhoneNumber,
            }).ToListAsync();


            int totalCount = _context.Users.Count();
            ViewData["TotalCount"] = totalCount;
            ViewData["Page"] = viewModels.Page;
            ViewData["Limit"] = viewModels.Limit;
            ViewData["Filter"] = viewModels;

            return View(result);
        }




        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserViewModels viewModels = new UserViewModels();
            return View(viewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserViewModels viewModels)
        {

            if (viewModels != null)
            {
                if (viewModels.Password != viewModels.ConfirmPassword)
                {
                    ModelState.AddModelError("", "رمز عبور تکرار آن یکسان نمی باشد!");
                    return View();
                }

                Random rnd = new Random();
                var createRandomPersonalId = rnd.Next(1000000, 999999999).ToString();
                var hashPassword = _hashedPassword.HashPassword(viewModels.Password);

                var newUser = new User
                {
                    Id = viewModels.Id,
                    FirstName = viewModels.FirstName,
                    LastName = viewModels.LastName,
                    UserName = viewModels.UserName,
                    Password = hashPassword,
                    PersonalId = createRandomPersonalId,
                    PhoneNumber = viewModels.PhoneNumber,
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var findUserById = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (findUserById == null)
            {
                return NotFound();
            }
            return View(findUserById);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User viewModels)
        {
            var findUserById = await _context.Users.FirstOrDefaultAsync(x => x.Id == viewModels.Id);
            if (findUserById == null)
            {
                ModelState.AddModelError("", "خطا در دریافت اطلاعات");
            }
            else
            {

                var hashPassword = _hashedPassword.HashPassword(viewModels.Password);

                findUserById.FirstName = viewModels.FirstName;
                findUserById.LastName = viewModels.LastName;
                findUserById.PhoneNumber = viewModels.PhoneNumber;
                findUserById.Password = hashPassword;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Remove(long id)
        {
            var finduserById = await _context.Users.FindAsync(id);

            if (finduserById == null)
            {
                return NotFound();
            }
            else
            {
                _context.Users.Remove(finduserById);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
