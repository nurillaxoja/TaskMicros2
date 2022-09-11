using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaskMicros2.Data;
using TaskMicros2.Models;
using TaskMicros2.Models.ToDeserialize;

namespace TaskMicros2.Controllers
{
    public class DatasController : Controller
    {
        private readonly DataContext _context;

        public DatasController(DataContext context)
        {
            _context = context;

            //checks if types column is empty
            if (_context.Types.ToList().Count == 0)
                AddTypes();


        }

        // GET: Datas
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Data.Include(d => d.Category).Include(d => d.Type).Include(d => d.User);
            return View(await dataContext.ToListAsync());
        }

        // GET: Datas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Data == null)
            {
                return NotFound();
            }

            var datas = await _context.Data
                .Include(d => d.Category)
                .Include(d => d.Type)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datas == null)
            {
                return NotFound();
            }

            return View(datas);
        }

        // GET: Datas/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category");
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Datas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,LastDate,Amount,Commentary,TypeId,CategoryId,UserId")] Datas datas)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(datas);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category", datas.CategoryId);
            //ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type", datas.TypeId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", datas.UserId);
            //return View(datas);

            if (datas == null)
                return NotFound();

            Datas toAdd = new Datas()
            {
                Id = datas.Id,
                StartDate = datas.StartDate.ToUniversalTime(),
                LastDate = datas.LastDate.ToUniversalTime(),
                CategoryId = datas.CategoryId,
                TypeId = datas.TypeId,
                UserId = datas.UserId,
                Commentary = datas.Commentary,
                Amount = datas.Amount
            };

            _context.Data.Add(toAdd);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Datas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Data == null)
            {
                return NotFound();
            }

            var datas = await _context.Data.FindAsync(id);
            if (datas == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category", datas.CategoryId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type", datas.TypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", datas.UserId);
            return View(datas);
        }

        // POST: Datas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,LastDate,Amount,Commentary,TypeId,CategoryId,UserId")] Datas datas)
        {
            if (id != datas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatasExists(datas.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category", datas.CategoryId);
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Type", datas.TypeId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", datas.UserId);
            return View(datas);
        }

        // GET: Datas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Data == null)
            {
                return NotFound();
            }

            var datas = await _context.Data
                .Include(d => d.Category)
                .Include(d => d.Type)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (datas == null)
            {
                return NotFound();
            }

            return View(datas);
        }

        // POST: Datas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Data == null)
            {
                return Problem("Entity set 'DataContext.Data'  is null.");
            }
            var datas = await _context.Data.FindAsync(id);
            if (datas != null)
            {
                _context.Data.Remove(datas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatasExists(int id)
        {
            return (_context.Data?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        //gets all data from DB
        public JsonResult GetAllDataJson()
        {
            var _datas = _context.Data;
            var _categories = _context.Categories;
            var _users = _context.Users;
            var _types = _context.Types;

            var res = from data in _datas
                      join category in _categories
                      on data.CategoryId equals category.Id
                      join type in _types
                      on data.TypeId equals type.Id
                      join user in _users
                      on data.UserId equals user.Id
                      select new
                      {
                          Id = data.Id,
                          StartDate = data.StartDate,
                          LastDate = data.LastDate,
                          Commentary = data.Commentary,
                          Amount = data.Amount,
                          Type = type.Type,
                          Category = category.Category,
                          User = user.UserName,
                          TypeId = data.TypeId,
                          CategoryId = data.CategoryId,
                          UserId = data.UserId,

                      };
            var dataToPass = new
            {
                Items = res,
                TotalCount = res.Count()
            };

            return new JsonResult(dataToPass);
        }

        // used to update grid changes to db
        [HttpPost]
        public HttpStatusCode UpdateData(Object request)
        {
            try
            {
                var models = JsonConvert.DeserializeObject<IEnumerable<DataDeserialize>>(Request.Form.FirstOrDefault().Value);
                
                Datas data = new Datas();

                foreach (var item in models)
                {
                    data.Id = item.Id;
                    data.StartDate = item.StartDate;
                    data.LastDate = item.LastDate;
                    data.Commentary = item.Commentary;
                    data.Amount = item.Amount;
                    data.TypeId = item.TypeId;
                    data.CategoryId = item.CategoryId;
                    data.UserId = item.UserId;
                }

                _context.Data.Update(data);
                var saveResult = _context.SaveChanges();

                if (saveResult == 0)
                    return HttpStatusCode.InternalServerError;

                return HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        // used to add income and expenses column value if they dont exists
        private void AddTypes()
        {
            Types type1 = new Types
            {
                Type = "Доходы"
            };

            Types type2 = new Types
            {
                Type = "Расходы"
            };
            _context.Types.Add(type1);
            _context.SaveChanges();
            _context.Types.Add(type2);
            _context.SaveChanges();
        }
    }

}
