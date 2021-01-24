using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ICURotationApp.Models;
using Microsoft.AspNetCore.Routing;

namespace ICURotationApp.Models
{
    public class FacilityListsController : Controller
    {
        private readonly FacilityContext _context;

        public FacilityListsController(FacilityContext context)
        {
            _context = context;
        }

        // GET: FacilityLists
        public async Task<IActionResult> Index(string facilityRegion, string searchString)
        {

            // Use LINQ to get list of Regions.
            IQueryable<string> regionQuery = from f in _context.FacilityList
                                            orderby f.Region
                                            select f.Region;

            var facilities = from f in _context.FacilityList
                         select f;

            if (!string.IsNullOrEmpty(searchString))
            {
                facilities = facilities.Where(s => s.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(facilityRegion))
            {
                facilities = facilities.Where(x => x.Region == facilityRegion);
            }

            var facilityRegionVM = new FacilityRegionViewModel
            {
                Regions = new SelectList(await regionQuery.Distinct().ToListAsync()),
                Facilities = await facilities.ToListAsync()
            };
            return View(facilityRegionVM);
        }

        // GET: FacilityLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facilityList = await _context.FacilityList
                .FirstOrDefaultAsync(m => m.FacilityId == id);
            if (facilityList == null)
            {
                return NotFound();
            }

            return View(facilityList);
        }
        
        // GET: FacilityLists/Create
        /*
        
        ***CREATE METHOD IS NOT USED FOR FacilityList TABLE.***  

        public IActionResult Create()
        {
            return View();
        }

        // POST: FacilityLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacilityId,Name,Region,NextInRotation,NumberOfSkips")] FacilityList facilityList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facilityList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facilityList);
        }
        */

        // GET: FacilityLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facilityList = await _context.FacilityList.FindAsync(id);
            if (facilityList == null)
            {
                return NotFound();
            }
            return View(facilityList);
        }

        // POST: FacilityLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacilityId,Name,Region,NextInRotation,NumberOfSkips")] FacilityList facilityList)
        {
            if (id != facilityList.FacilityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facilityList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityListExists(facilityList.FacilityId))
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
            return View(facilityList);
        }

    /*  This action method is used to redirect to a different controller as well as pass an "id" to a named action
        method(Create) within that controller.  The id is the same id that was passed to the initial action method. */
        public IActionResult PatientAccepted(int? id)
        {
            return RedirectToAction("Create", new RouteValueDictionary(
            new { controller = "Acceptances", action = "Create", Id = id }));


        }
        


        // GET: FacilityLists/Delete/5

        /*
         
        ***DELETE ACTION METHOD NOT USED ON FacilityList TABLE.***

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facilityList = await _context.FacilityList
                .FirstOrDefaultAsync(m => m.FacilityId == id);
            if (facilityList == null)
            {
                return NotFound();
            }

            return View(facilityList);
        }

        // POST: FacilityLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facilityList = await _context.FacilityList.FindAsync(id);
            _context.FacilityList.Remove(facilityList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        } 

        */

        private bool FacilityListExists(int id)
        {
            return _context.FacilityList.Any(e => e.FacilityId == id);
        }
    }

        
}
