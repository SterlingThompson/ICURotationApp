using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ICURotationApp.Models
{
    public class DenialsController : Controller
    {
        private readonly FacilityContext _context;

        public DenialsController(FacilityContext context)
        {
            _context = context;
        }

        // GET: Denials
        public async Task<IActionResult> Index(string deniedsendingfacilities, string deniedreceivingfacilities)
        {
            IQueryable<string> DeniedSendingFacilityQuery = from f in _context.Denials
                                                      orderby f.SendingFacility
                                                      select f.SendingFacility;

            IQueryable<string> DeniedReceivingFacilityQuery = from g in _context.Denials
                                                        orderby g.ReceivingFacility
                                                        select g.ReceivingFacility;

            var facilities = from f in _context.Denials
                             select f;

            if (!string.IsNullOrEmpty(deniedsendingfacilities))
            {
                facilities = facilities.Where(s => s.SendingFacility.Contains(deniedsendingfacilities));
            }

            if (!string.IsNullOrEmpty(deniedreceivingfacilities))
            {
                facilities = facilities.Where(t => t.ReceivingFacility == deniedreceivingfacilities);
            }

            var DenialVM = new DenialViewModel
            {
                SendingFacilities = new SelectList(await DeniedSendingFacilityQuery.Distinct().ToListAsync()),
                ReceivingFacilities = new SelectList(await DeniedReceivingFacilityQuery.Distinct().ToListAsync()),
                Denials = await facilities.ToListAsync()

            };
            return View(DenialVM);
        }

        // GET: Denials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var denials = await _context.Denials
                .FirstOrDefaultAsync(m => m.DenialId == id);
            if (denials == null)
            {
                return NotFound();
            }

            return View(denials);
        }

        // GET: Denials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Denials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DenialId,Date,GoldenHourNumber,ChiefComplaint,SendingFacility,ReceivingFacility,DenialReason")] Denials denials)
        {
            if (ModelState.IsValid)
            {
                _context.Add(denials);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(denials);
        }

        // GET: Denials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var denials = await _context.Denials.FindAsync(id);
            if (denials == null)
            {
                return NotFound();
            }
            return View(denials);
        }

        // POST: Denials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DenialId,Date,GoldenHourNumber,ChiefComplaint,SendingFacility,ReceivingFacility,DenialReason")] Denials denials)
        {
            if (id != denials.DenialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(denials);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DenialsExists(denials.DenialId))
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
            return View(denials);
        }

        // GET: Denials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var denials = await _context.Denials
                .FirstOrDefaultAsync(m => m.DenialId == id);
            if (denials == null)
            {
                return NotFound();
            }

            return View(denials);
        }

        // POST: Denials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var denials = await _context.Denials.FindAsync(id);
            _context.Denials.Remove(denials);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DenialsExists(int id)
        {
            return _context.Denials.Any(e => e.DenialId == id);
        }
    }
}
