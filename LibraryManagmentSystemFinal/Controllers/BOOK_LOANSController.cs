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
    public class BOOK_LOANSController : Controller
    {
        private LIbraryManagementDBContext db = new LIbraryManagementDBContext();

        // GET: BOOK_LOANS
        public ActionResult Index()
        {
            var bOOK_LOANS = db.BOOK_LOANS.Include(b => b.BOOK).Include(b => b.BORROWER);
            return View(bOOK_LOANS.ToList());
        }

        // GET: BOOK_LOANS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK_LOANS bOOK_LOANS = db.BOOK_LOANS.Find(id);
            if (bOOK_LOANS == null)
            {
                return HttpNotFound();
            }
            return View(bOOK_LOANS);
        }

        // GET: BOOK_LOANS/Create
        public ActionResult Create()
        {
            ViewBag.ISBN = new SelectList(db.BOOKs, "ISBN", "TITLE");
            ViewBag.CARD_ID = new SelectList(db.BORROWERs, "CARD_ID", "SSN");
            return View();
        }

        // POST: BOOK_LOANS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LOAN_ID,ISBN,CARD_ID,DATE_OUT,DUE_DATE,DATE_IN")] BOOK_LOANS bOOK_LOANS)
        {
            if (ModelState.IsValid)
            {
                db.BOOK_LOANS.Add(bOOK_LOANS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ISBN = new SelectList(db.BOOKs, "ISBN", "TITLE", bOOK_LOANS.ISBN);
            ViewBag.CARD_ID = new SelectList(db.BORROWERs, "CARD_ID", "SSN", bOOK_LOANS.CARD_ID);
            return View(bOOK_LOANS);
        }

        // GET: BOOK_LOANS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK_LOANS bOOK_LOANS = db.BOOK_LOANS.Find(id);
            if (bOOK_LOANS == null)
            {
                return HttpNotFound();
            }
            ViewBag.ISBN = new SelectList(db.BOOKs, "ISBN", "TITLE", bOOK_LOANS.ISBN);
            ViewBag.CARD_ID = new SelectList(db.BORROWERs, "CARD_ID", "SSN", bOOK_LOANS.CARD_ID);
            return View(bOOK_LOANS);
        }

        // POST: BOOK_LOANS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LOAN_ID,ISBN,CARD_ID,DATE_OUT,DUE_DATE,DATE_IN")] BOOK_LOANS bOOK_LOANS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bOOK_LOANS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ISBN = new SelectList(db.BOOKs, "ISBN", "TITLE", bOOK_LOANS.ISBN);
            ViewBag.CARD_ID = new SelectList(db.BORROWERs, "CARD_ID", "SSN", bOOK_LOANS.CARD_ID);
            return View(bOOK_LOANS);
        }

        // GET: BOOK_LOANS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK_LOANS bOOK_LOANS = db.BOOK_LOANS.Find(id);
            if (bOOK_LOANS == null)
            {
                return HttpNotFound();
            }
            return View(bOOK_LOANS);
        }

        // POST: BOOK_LOANS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BOOK_LOANS bOOK_LOANS = db.BOOK_LOANS.Find(id);
            db.BOOK_LOANS.Remove(bOOK_LOANS);
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
