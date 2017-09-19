using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagmentSystemFinal;

namespace LibraryManagmentSystemFinal.Controllers
{
    public class BORROWERsController : Controller
    {
        private LIbraryManagementDBContext db = new LIbraryManagementDBContext();

        // GET: BORROWERs
        public ActionResult Index()
        {
            return View(db.BORROWERs.ToList());
        }

        // GET: BORROWERs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BORROWER bORROWER = db.BORROWERs.Find(id);
            if (bORROWER == null)
            {
                return HttpNotFound();
            }
            return View(bORROWER);
        }

        // GET: BORROWERs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BORROWERs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CARD_ID,SSN,FNAME,LNAME,ADDRESS,PHONE")] BORROWER bORROWER)
        {
            if (ModelState.IsValid)
            {
                foreach (var ssns in db.BORROWERs)
                {
                    if (ssns.SSN == bORROWER.SSN)
                    {
                        return View("UserExists");
                    }
                }
                db.BORROWERs.Add(bORROWER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Session["ssn"] = bORROWER.SSN;
           

            return View(bORROWER);
        }

        // GET: BORROWERs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BORROWER bORROWER = db.BORROWERs.Find(id);
            if (bORROWER == null)
            {
                return HttpNotFound();
            }
            return View(bORROWER);
        }

        // POST: BORROWERs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CARD_ID,SSN,FNAME,LNAME,ADDRESS,PHONE")] BORROWER bORROWER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bORROWER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bORROWER);
        }

        // GET: BORROWERs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BORROWER bORROWER = db.BORROWERs.Find(id);
            if (bORROWER == null)
            {
                return HttpNotFound();
            }
            return View(bORROWER);
        }

        // POST: BORROWERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BORROWER bORROWER = db.BORROWERs.Find(id);
            db.BORROWERs.Remove(bORROWER);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
