using DBCU_WebApp.core;
using DBCU_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DBCU_WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NewsManagementController : Controller
    {
        private readonly DBUWebContext _context;

        private readonly UserManager<IdentityUser> _usermanager;

        private readonly ILogger<NewsManagementController> _logger;

        public NewsManagementController(DBUWebContext context,
            UserManager<IdentityUser> usermanager,
            ILogger<NewsManagementController> logger)
        {
            _context = context;
            _usermanager = usermanager;
            _logger = logger;
        }

        public const int ITEMS_PER_PAGE = 10;
        // GET: Admin/Post
        public async Task<IActionResult> Index([Bind(Prefix = "page")] int pageNumber)
        {

            if (pageNumber == 0)
                pageNumber = 1;

            var listPosts = _context.News
                //.Include (p => p.Author)
                .Include(p => p.NewsCategories)
                .ThenInclude(c => c.Category)
                .OrderByDescending(p => p.DateCreated);

            _logger.LogInformation(pageNumber.ToString());

            // Lấy tổng số dòng dữ liệu
            var totalItems = listPosts.Count();
            // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục)
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);

            //if (pageNumber > totalPages)
            //    return RedirectToAction (nameof (NewsManagementController.Index), new { page = totalPages });

            var posts = await listPosts
                .Skip(ITEMS_PER_PAGE * (pageNumber - 1))
                .Take(ITEMS_PER_PAGE)
                .ToListAsync();

            // return View (await listPosts.ToListAsync());
            ViewData["pageNumber"] = pageNumber;
            ViewData["totalPages"] = totalPages;

            return View(posts.AsEnumerable());
        }

        // GET: Admin/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.News
                //.Include (p => p.Author)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [BindProperty]
        public int[] selectedCategories { set; get; }

        // GET: Admin/Post/Create
        public async Task<IActionResult> Create()
        {

            // Thông tin về User tạo Post
            var user = await _usermanager.GetUserAsync(User);
            ViewData["userpost"] = $"{user.UserName} {user.UserName}";

            // Danh mục chọn để đăng bài Post, tạo MultiSelectList
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            return View();

        }

        // POST: Admin/Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,TitleEn,Description,DescriptionEn,Slug,Content,ContentEn,Author,Published")] NewsBase post, IFormFile StrUrlImage)
        {

            if (ModelState["Title"].ValidationState == ModelValidationState.Invalid)
            {
                ModelState.AddModelError(String.Empty, "Phải có tiêu đề bài viết TV");
            }
            if (ModelState["TitleEn"].ValidationState == ModelValidationState.Invalid)
            {
                ModelState.AddModelError(String.Empty, "Phải có tiêu đề bài viết TA");
            }

            var user = await _usermanager.GetUserAsync(User);
            ViewData["userpost"] = $"{user.UserName}";

            // Phát sinh Slug theo Title
            if (ModelState["Slug"].ValidationState == ModelValidationState.Invalid)
            {
                post.Slug = Utils.GenerateSlug(post.Title);
                ModelState.SetModelValue("Slug", new ValueProviderResult(post.Slug));
                // Thiết lập và kiểm tra lại Model
                ModelState.Clear();
                TryValidateModel(post);
            }
            // Phát sinh Slug theo Title
            if (ModelState["SlugEn"].ValidationState == ModelValidationState.Invalid)
            {
                post.SlugEn = Utils.GenerateSlug(post.TitleEn);
                ModelState.SetModelValue("SlugEn", new ValueProviderResult(post.SlugEn));
                // Thiết lập và kiểm tra lại Model
                ModelState.Clear();
                TryValidateModel(post);
            }

            if (selectedCategories.Length == 0)
            {
                ModelState.AddModelError(String.Empty, "Phải ít nhất một chuyên mục");
            }

            bool SlugExisted = await _context.News.Where(p => p.Slug == post.Slug).AnyAsync();
            if (SlugExisted)
            {
                ModelState.AddModelError(nameof(post.Slug), "Slug đã có trong Database");
            }

            if (StrUrlImage != null && StrUrlImage.Length > 0)
            {
                var fileName = Path.GetFileName(StrUrlImage.FileName); ;
                var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                var fileExtension = Path.GetExtension(fileName);
                var newFileName = String.Concat(myUniqueFileName, fileExtension);
                var filepath =
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\News\{newFileName}";

                using (FileStream fs = System.IO.File.Create(filepath))
                {
                    StrUrlImage.CopyTo(fs);
                    fs.Flush();
                }

                post.StrUrlImage = newFileName;
            }

            if (ModelState.IsValid)
            {
                //Tạo Post
                var newpost = new News()
                {
                    AuthorId = user.Id,
                    Title = post.Title,
                    TitleEn = post.TitleEn,
                    Slug = post.Slug,
                    SlugEn = post.SlugEn,
                    Content = post.Content,
                    ContentEn = post.ContentEn,
                    Description = post.Description,
                    DescriptionEn = post.DescriptionEn,
                    Published = post.Published,
                    StrUrlImage = post.StrUrlImage,
                    Author = post.Author,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                };
                _context.Add(newpost);
                await _context.SaveChangesAsync();

                // Chèn thông tin về PostCategory của bài Post
                foreach (var selectedCategory in selectedCategories)
                {
                    _context.Add(new NewsCategory() { NewsID = newpost.NewsId, CategoryID = selectedCategory });
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title", selectedCategories);
            return View(post);
        }

        // GET: Admin/Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var post = await _context.Posts.FindAsync (id);
            var post = await _context.News.Where(p => p.NewsId == id)
                //.Include (p => p.Author)
                .Include(p => p.NewsCategories)
                .ThenInclude(c => c.Category).FirstOrDefaultAsync();
            if (post == null)
            {
                return NotFound();
            }

            ViewData["userpost"] = $"{post.AuthorId}";
            ViewData["datecreate"] = post.DateCreated.ToShortDateString();

            // Danh mục chọn
            var selectedCates = post.NewsCategories.Select(c => c.CategoryID).ToArray();
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title", selectedCates);

            return View(post);
        }

        // POST: Admin/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,Title,TitleEn,Description,DescriptionEn,Slug,SlugEn,Author,Content,ContentEn")] NewsBase post, IFormFile StrUrlImage)
        {

            if (id != post.NewsId)
            {
                return NotFound();
            }

            if (post.Title.Length == 0)
            {
                ModelState.AddModelError(String.Empty, "Phải có tiêu đề bài viết TV");
            }
            if (post.TitleEn.Length == 0)
            {
                ModelState.AddModelError(String.Empty, "Phải có tiêu đề bài viết TA");
            }

            // Phát sinh Slug theo Title
            if (ModelState["Slug"].ValidationState == ModelValidationState.Invalid)
            {
                post.Slug = Utils.GenerateSlug(post.Title);
                ModelState.SetModelValue("Slug", new ValueProviderResult(post.Slug));
                // Thiết lập và kiểm tra lại Model
                ModelState.Clear();
                TryValidateModel(post);
            }

            // Phát sinh Slug theo Title
            if (ModelState["SlugEn"].ValidationState == ModelValidationState.Invalid)
            {
                post.SlugEn = Utils.GenerateSlug(post.TitleEn);
                ModelState.SetModelValue("SlugEn", new ValueProviderResult(post.SlugEn));
                // Thiết lập và kiểm tra lại Model
                ModelState.Clear();
                TryValidateModel(post);
            }

            //if (selectedCategories.Length == 0) {
            //    ModelState.AddModelError (String.Empty, "Phải ít nhất một chuyên mục");
            //}

            bool SlugExisted = await _context.News.Where(p => p.Slug == post.Slug && p.NewsId != post.NewsId).AnyAsync();
            if (SlugExisted)
            {
                ModelState.AddModelError(nameof(post.Slug), "Slug đã có trong Database");
            }

            if (StrUrlImage != null && StrUrlImage.Length > 0)
            {
                var fileName = Path.GetFileName(StrUrlImage.FileName) + DateTime.Now.ToString(); ;
                var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                var fileExtension = Path.GetExtension(fileName);
                var newFileName = String.Concat(myUniqueFileName, fileExtension);
                var filepath =
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\News\{newFileName}";

                using (FileStream fs = System.IO.File.Create(filepath))
                {
                    StrUrlImage.CopyTo(fs);
                    fs.Flush();
                }

                post.StrUrlImage = fileName;
            }

            if (StrUrlImage != null && StrUrlImage.Length > 0)
            {
                var fileName = Path.GetFileName(StrUrlImage.FileName); ;
                var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                var fileExtension = Path.GetExtension(fileName);
                var newFileName = String.Concat(myUniqueFileName, fileExtension);
                var filepath =
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\News\{newFileName}";

                using (FileStream fs = System.IO.File.Create(filepath))
                {
                    StrUrlImage.CopyTo(fs);
                    fs.Flush();
                }

                post.StrUrlImage = newFileName;
            }

            if (ModelState.IsValid)
            {

                // Lấy nội dung từ DB
                var postUpdate = await _context.News.Where(p => p.NewsId == id)
                    .Include(p => p.NewsCategories)
                    .ThenInclude(c => c.Category)
                    .FirstOrDefaultAsync();

                if (postUpdate == null)
                {
                    return NotFound();
                }

                // Cập nhật nội dung mới
                postUpdate.Title = post.Title;
                postUpdate.TitleEn = post.TitleEn;
                postUpdate.Description = post.Description;
                postUpdate.DescriptionEn = post.DescriptionEn;
                postUpdate.Content = post.Content;
                postUpdate.ContentEn = post.ContentEn;
                postUpdate.Slug = post.Slug;
                postUpdate.SlugEn = post.SlugEn;

                postUpdate.DateUpdated = DateTime.Now;

                if (StrUrlImage != null && StrUrlImage.Length > 0)
                {
                    postUpdate.StrUrlImage = post.StrUrlImage;
                }

                // Các danh mục không có trong selectedCategories
                //var listcateremove = postUpdate.NewsCategories
                //                               .Where(p => !selectedCategories.Contains(p.CategoryID))
                //                               .ToList();
                //listcateremove.ForEach(c => postUpdate.NewsCategories.Remove(c));

                //// Các ID category chưa có trong postUpdate.PostCategories
                //var listCateAdd = selectedCategories
                //                    .Where(
                //                        id => !postUpdate.NewsCategories.Where(c => c.CategoryID == id).Any()
                //                    ).ToList();

                //listCateAdd.ForEach(id => {
                //    postUpdate.NewsCategories.Add(new NewsCategory() {
                //        NewsID = postUpdate.NewsId,
                //        CategoryID = id
                //    });
                //});

                try
                {

                    _context.Update(postUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.NewsId))
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

            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title", selectedCategories);
            return View(post);
        }

        // GET: Admin/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.News
                //.Include (p => p.Author)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.News.FindAsync(id);
            _context.News.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.News.Any(e => e.NewsId == id);
        }


    }
}