



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;
using TodoApp.ViewModels;

namespace TodoApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        //این متغیر فقط از داخل این کلاس قابل دسترسیه
        private readonly ApplicationDbContext _context;

        //برای مدیریت کاربران ایجاد، ویرایش، حذف و تأیید هویته
        //نام متغیر که نشان‌دهنده سرویس مدیریت کاربره
        private readonly UserManager<ApplicationUser> _userManager;

        //تعریف سازنده کلاس
        public TodoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            //مقداردهی به متغیرهای خصوصی
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            //تعریف متغیر و مقداردهی اولیه
            // بارگذاری تمام تسک‌های غیر آرشیوشده برای این کاربر
            var todos = _context.Todos
                                .Where(t => t.UserId == user.Id && !t.IsArchived)
                                .Select(t => new TodoItemViewModel
                                {
                                    Id = t.Id,
                                    Title = t.Title,
                                    Priority = t.Priority,
                                    CreatedAt = t.CreatedAt,
                                    IsFinished = t.IsFinished,
                                    IsArchived = t.IsArchived
                                })
                                .ToList();

            var viewModel = new TodoViewModel
            {
                SearchQuery = string.Empty,
                Todos = todos
            };

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new TodoItemViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemViewModel model)
        {
            //اگر تمامی اعتبارسنجی‌ها موفق باشه
            //اعتبارسنجی‌ها با استفاده از ویژگی‌های اتربیوت مربوط به مدل تعریف میشن 
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var todo = new Todo
                {
                    Title = model.Title,
                    Priority = model.Priority,
                    CreatedAt = model.CreatedAt ?? DateTime.Now,
                    IsFinished = model.IsFinished,
                    IsArchived = model.IsArchived,
                    UserId = user.Id
                };
                //افزودن شیء به جدول
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    //var todo = await _context.Todos.FindAsync(id);
        //    //امنتر 
        //    var user = await _userManager.GetUserAsync(User);
        //    var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

        //    if (todo == null)
        //    {

        //        return Unauthorized();
        //        //return NotFound();
        //    }

        //    _context.Todos.Remove(todo);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Index));
        //}


        //حذف داینامیک//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

            if (todo == null)
            {
                return Unauthorized();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }









        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //var todo = await _context.Todos.FindAsync(id);
            //امنتر//
            var user = await _userManager.GetUserAsync(User);
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
            if (todo == null)
            {
                return Unauthorized();
                //return NotFound();
            }

            var model = new TodoItemViewModel
            {
                Title = todo.Title,
                Priority = todo.Priority,
                CreatedAt = todo.CreatedAt,
                IsFinished = todo.IsFinished,
                IsArchived = todo.IsArchived
            };

            return PartialView("_EditTodoModal", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditTodoModal", model);
            }

            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Title = model.Title;
            todo.Priority = model.Priority;
            todo.CreatedAt = model.CreatedAt ?? todo.CreatedAt;
            todo.IsFinished = model.IsFinished;
            todo.IsArchived = model.IsArchived;

            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // آپدیت مقادیر چک‌باکس‌ها با متد UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Consumes("application/json")]
        public IActionResult UpdateStatus([FromBody] UpdateStatusViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Field))
                return BadRequest("Invalid data");

            // پیدا کردن تسک براساس Id
            //var todo = _context.Todos.FirstOrDefault(t => t.Id == model.Id);
            //امنتر//
            var user = _userManager.GetUserAsync(User).Result;
            var todo = _context.Todos.FirstOrDefault(t => t.Id == model.Id && t.UserId == user.Id);
            if (todo == null)

                return Unauthorized();
            //return NotFound();

            // به‌روزرسانی فیلد
            if (model.Field == "IsFinished")
            {
                todo.IsFinished = model.Value;
            }
            else if (model.Field == "IsArchived")
            {
                todo.IsArchived = model.Value;
            }
            else
            {
                return BadRequest("Invalid field");
            }

            // ذخیره تغییرات
            _context.SaveChanges();

            return Ok();
        }



        [HttpGet]
        public async Task<IActionResult> Archived()
        {
            var user = await _userManager.GetUserAsync(User);
            var archivedTodos = _context.Todos
                                        .Where(t => t.UserId == user.Id && t.IsArchived)
                                        .ToList();
            return View(archivedTodos);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(TodoViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            // اعمال فیلترها
            var todos = _context.Todos
                                .Where(t => t.UserId == user.Id && !t.IsArchived);

            if (!string.IsNullOrEmpty(model.SearchQuery))
            {
                //todos = todos.Where(t => t.Title.Contains(model.SearchQuery));

                //امنتر کردن جستجو//
                var sanitizedQuery = model.SearchQuery.Trim();
                // ای اف یک متد ویژه انتیتی فریمورکه
                //برای جلوگیری از حملات تزریق اس کیول
                todos = todos.Where(t => EF.Functions.Like(t.Title, $"%{sanitizedQuery}%"));
            }

            if (!string.IsNullOrEmpty(model.PriorityFilter))
            {
                todos = todos.Where(t => t.Priority == model.PriorityFilter);
            }
            //بررسی می کنه که یک متغیر پایان یافته نالیبل  مقدار داره یا نه
            if (model.IsFinishedFilter.HasValue)
            {
                todos = todos.Where(t => t.IsFinished == model.IsFinishedFilter.Value);
            }

            if (model.StartDateFilter.HasValue)
            {
                todos = todos.Where(t => t.CreatedAt >= model.StartDateFilter.Value);
            }

            if (model.EndDateFilter.HasValue)
            {
                todos = todos.Where(t => t.CreatedAt <= model.EndDateFilter.Value);
            }

            // تبدیل به ViewModel
            var filteredTodos = todos.Select(t => new TodoItemViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Priority = t.Priority,
                CreatedAt = t.CreatedAt,
                IsFinished = t.IsFinished,
                IsArchived = t.IsArchived
            }).ToList();

            var viewModel = new TodoViewModel
            {
                SearchQuery = model.SearchQuery,
                PriorityFilter = model.PriorityFilter,
                IsFinishedFilter = model.IsFinishedFilter,
                StartDateFilter = model.StartDateFilter,
                EndDateFilter = model.EndDateFilter,
                Todos = filteredTodos
            };

            return View("Index", viewModel);

        }





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteArchiveTodo(int id)
        //{
        //    //var todo = await _context.Todos.FindAsync(id);
        //    //امنتر 
        //    var user = await _userManager.GetUserAsync(User);
        //    var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

        //    if (todo == null)
        //    {

        //        return Unauthorized();
        //        //return NotFound();
        //    }

        //    _context.Todos.Remove(todo);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Archived));
        //}







    }
}

























