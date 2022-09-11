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
    public class TypesController : Controller
    {
        private readonly DataContext _context;

        public TypesController(DataContext context)
        {
            _context = context;

        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
            



            return _context.Types != null ?
                        View(await _context.Types.ToListAsync()) :
                        Problem("Entity set 'DataContext.Types'  is null.");
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (types == null)
            {
                return NotFound();
            }

            return View(types);
        }

        // GET: Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type")] Types types)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(types);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(types);

            if (types == null)
                return NotFound();

            _context.Types.Add(types);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types.FindAsync(id);
            if (types == null)
            {
                return NotFound();
            }
            return View(types);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] Types types)
        {
            if (id != types.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypesExists(types.Id))
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
            return View(types);
        }

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (types == null)
            {
                return NotFound();
            }

            return View(types);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Types == null)
            {
                return Problem("Entity set 'DataContext.Types'  is null.");
            }
            var types = await _context.Types.FindAsync(id);
            if (types != null)
            {
                _context.Types.Remove(types);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypesExists(int id)
        {
            return (_context.Types?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //get all types list to grid
        public JsonResult GetAllTypesJson()
        {
            var _types = _context.Types;

            var data = new
            {
                Items = _types,
                TotalCount = _types.Count()
            };

            return new JsonResult(data);
        }
        
        //save grid updates to DB
        [HttpPost]
        public HttpStatusCode UpdateTypes(Object request)
        {
            try
            {
                var models = JsonConvert.DeserializeObject<IEnumerable<Types>>(Request.Form.FirstOrDefault().Value);

                Types data = new Types();

                foreach (var item in models)
                {
                    data.Id = item.Id;
                    data.Type = item.Type;
                }

                _context.Types.Update(data);
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


        
    }
}
