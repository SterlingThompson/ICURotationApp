using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ICURotationApp.Models
{
    public class AcceptancesController : Controller
    {
        private readonly FacilityContext _context;

        public AcceptancesController(FacilityContext context)
        {
            _context = context;
        }

        // GET: Acceptances
    /*  The Index action method provides the view which is an AcceptingFacilityViewModel object w/the neccessary data obtained
        via LINQ query.  The data is passed after a new instance of the AcceptingFacilityViewModel is created(acceptSendingVM)
        and specified properties of the new AcceptingFacilityViewModel instance are assigned data collected from query.  */
        public async Task<IActionResult> Index(string sfacilities, string rfacilities)
        {
            IQueryable<string> SendingFacilityQuery = from f in _context.Acceptance
                                             orderby f.SendingFacility
                                             select f.SendingFacility;

            IQueryable<string> ReceivingFacilityQuery = from f in _context.Acceptance
                                                        orderby f.ReceivingFacility
                                                        select f.ReceivingFacility;

            var facilities = from f in _context.Acceptance
                             select f;

            if(!string.IsNullOrEmpty(rfacilities))
            {
                facilities = facilities.Where(t => t.ReceivingFacility == rfacilities);
            }

            if (!string.IsNullOrEmpty(sfacilities))
            {
                facilities = facilities.Where(s => s.SendingFacility == sfacilities);
            }

            var acceptSendingVM = new AcceptSendingFacilityViewModel
            {
                SendingFacilities = new SelectList(await SendingFacilityQuery.Distinct().ToListAsync()),
                ReceivingFacilities = new SelectList(await ReceivingFacilityQuery.Distinct().ToListAsync()),
                Acceptances = await facilities.ToListAsync()

            };
            return View(acceptSendingVM);
        }

        // GET: Acceptances/Details/5
        /* DETAILS ACTION METHOD NOT USED IN THIS APPLICATION. 
         
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acceptance = await _context.Acceptance
                .FirstOrDefaultAsync(m => m.AcceptanceId == id);
            if (acceptance == null)
            {
                return NotFound();
            }

            return View(acceptance);
        }
        */

        // GET: Acceptances/Create
        public IActionResult Create(int? id)
        {
         /* Utilizes LINQ to obtain receiving facility name based on an integer value(id).
            The id is compared to a FacilityId of the FacilityList table in the FacilityContext database and if true
            the name of that facility is assigned to the ViewData variable with given name "newReceivingFacility". 
            This ViewData was captured so that when the Create view is delivered to the browser the option of pre-populating
            Receiving Facility input field is available.  Further coding has been done in the Create view Razor page
            to fully implement this feature. */

            if (id != null)
            {
                ViewData["newReceivingFacility"] = _context.FacilityList.Single(e => e.FacilityId == id).Name;

            }

            return View();
        }

        // POST: Acceptances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

     /* The Create method is not only used to create/add a new data populated row to the Acceptance table.  This method
        also automatically updates the FacilityList Index view based upon certain conditions.  This feature was implemented
        to relieve the user of the ICU Rotation process of tracking facility skips and whether or not 
        a facility should be set as next in rotation.  */
        public async Task<IActionResult> Create(int id, [Bind("AcceptanceId,Date,GoldenHourNumber,ChiefComplaint,SendingFacility,ReceivingFacility,InitiationLocation,SkipReason")] Acceptance acceptance)
        {

            if (ModelState.IsValid)
            {

             /* LINQ query is used to assign a single element that meets the neccessary condition(where FacilityId == id)
                to the declared variable.  The id parameter is passed to the action method from the URL request and is 
                used to designate the facility that was selected as having accepted a patient via hyperlink.  It is also 
                utilized to evaluate certain conditions of the FacilityList table and make changes to the table based upon
                those conditions. */
                var facility = _context.FacilityList.Single(x => x.FacilityId == id);

             /* Assigns the next facility in the FacilityList table after the one that was designated as accepting a patient
                via click of hyperlink to the declared variable.  */
                var nextFacility = _context.FacilityList.Single(y => y.FacilityId == id + 1);


             /* This query is used to filter FacilityList table by Region and assign the data to the declared variable.
                It is also used to provide a count to determine when the very last facility in a region is being evaluated which 
                in turn determines if evaluation should begin at the very first facility in a specific region(BeginningOfRotation)
                or not.  The ToList method was added to the end of the query to eliminate an error pertaining to multiple threads being
                in use simultaneously.  */
                var query = _context.FacilityList.Where(skips => skips.Region == facility.Region).ToList();
                

                /* "facility" in all conditional statements will be referencing the hospital(& its related fields) that had the "Accepted"
                   hyperlink located on the same row with it clicked by the user.  In the particular conditional statement below based on 
                   the condition of the "facility" one action is performed and the changes saved.  Under these particular 
                   conditions no other conditions need be evaluated and the state of the table can be saved.  */
                if (facility.NextInRotation == false && facility.FacilityId == id)
                {
                    ++facility.NumberOfSkips;

                 /* Below is one instance where a new row is added to the Acceptance table along w/updates to the FacilityList table.
                    After which the changes are saved to the database(_context which is an object of the FacilityContext class). */
                    _context.Add(acceptance);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                    
                }

                 /* This portion of the code is to be executed based on the fact that after an acceptance, one of the hospitals following
                    the hospital(in a specific region) that just accepted a patient can be set as next in rotation based upon the proper
                    conditions(Number of Skips being 0 & not already being set as Next in Rotation). */ 

                    if (nextFacility.NumberOfSkips == 0
                        && facility.NextInRotation == true
                        && nextFacility.Region == facility.Region)
                    {


                        facility.NextInRotation = false;
                        nextFacility.NextInRotation = true;
                        _context.Add(acceptance);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                        
                }

                    if (facility.NextInRotation == true
                        && nextFacility.NumberOfSkips > 0
                        && nextFacility.Region == facility.Region)

                        foreach (var item in query)
                        {
                           
                            if (item.FacilityId > id)
                            {
                                if(facility.NextInRotation)
                                {
                                    facility.NextInRotation = false;
                                }
                                
                                if (item.NextInRotation == false && item.NumberOfSkips == 0)
                                {
                                    item.NextInRotation = true;

                                    _context.Add(acceptance);
                                    await _context.SaveChangesAsync();
                                    return RedirectToAction(nameof(Index));
                            }

                                if (item.NumberOfSkips > 0)
                                {
                                    --item.NumberOfSkips;
                                }


                                if (item.FacilityId > query.Count())
                                {

                                     break;
                                    
                                }


                            }
                        }

             /* This portion of code is to be executed after a hospital acceptance and based on the condition of the following
                hospitals in that particular region evaluation has to start again from the top of the list of hospitals
                in that region(starting at the beginning of the rotation).  */
                foreach (var item in query)
                {
                    if (facility.FacilityId >= item.FacilityId)
                    {
                        if (item.NumberOfSkips == 0)
                        {
                            facility.NextInRotation = false;
                            item.NextInRotation = true;
                            _context.Add(acceptance);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }

                        if (item.NumberOfSkips > 0)
                        {

                            --item.NumberOfSkips;

                        }

                    } 

                }
                   

                }    

            return View(acceptance);
        }

        // GET: Acceptances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
             var acceptance = await _context.Acceptance
                  .FirstOrDefaultAsync(m => m.AcceptanceId == id);
            /*var acceptance = await _context.Acceptance.FindAsync(id);*/
            if (acceptance == null)
            {
                return NotFound();
            }

            return View(acceptance);
        }

        // POST: Acceptances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("AcceptanceId,Date,GoldenHourNumber,ChiefComplaint,SendingFacility,ReceivingFacility,InitiationLocation,SkipReason")] Acceptance acceptance)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(acceptance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcceptanceExists(acceptance.AcceptanceId))
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
            return View(acceptance);
        }

        // GET: Acceptances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acceptance = await _context.Acceptance
                .FirstOrDefaultAsync(m => m.AcceptanceId == id);
            if (acceptance == null)
            {
                return NotFound();
            }

            return View(acceptance);
        }

        // POST: Acceptances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var acceptance = await _context.Acceptance.FindAsync(id);
            _context.Acceptance.Remove(acceptance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcceptanceExists(int? id)
        {
            return _context.Acceptance.Any(e => e.AcceptanceId == id);
        }
    }
}
