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
    public class FINEsController : Controller
    {
        private LIbraryManagementDBContext db = new LIbraryManagementDBContext();

        // GET: FINEs
        public ActionResult Index()
        {
            var fINES = db.FINES.Include(f => f.BOOK_LOANS);
            return View(fINES.ToList());
        }

        // GET: FINEs/Details/5
        public ActionResult Details(decimal? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FINE fINE = db.FINES.Find(id);
            if (fINE == null)
            {
                return HttpNotFound();
            }
            return View(fINE);
        }

        // GET: FINEs/Create
        public ActionResult Create()
        {
            ViewBag.LOAN_ID = new SelectList(db.BOOK_LOANS, "LOAN_ID", "LOAN_ID");
            return View();
        }

        // POST: FINEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LOAN_ID,FINE_AMT,PAID")] FINE fINE)
        {
            if (ModelState.IsValid)
            {
                db.FINES.Add(fINE);
                BOOKsController book = new BOOKsController();
                //db.SaveChanges();
                book.ViewFines(fINE.BOOK_LOANS);
                return RedirectToAction("Index");
            }

            ViewBag.LOAN_ID = new SelectList(db.BOOK_LOANS, "LOAN_ID", "LOAN_ID", fINE.LOAN_ID);
            return View(fINE);
        }

        // GET: FINEs/Edit/5
        public ActionResult Edit(decimal? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FINE fINE = db.FINES.Find(id);
            if (fINE == null)
            {
                return HttpNotFound();
            }
            ViewBag.LOAN_ID = new SelectList(db.BOOK_LOANS, "LOAN_ID", "LOAN_ID", fINE.LOAN_ID);
            return View(fINE);
        }

        // POST: FINEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LOAN_ID,FINE_AMT,PAID")] FINE fINE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fINE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LOAN_ID = new SelectList(db.BOOK_LOANS, "LOAN_ID", "LOAN_ID", fINE.LOAN_ID);
            return View(fINE);
        }

        // GET: FINEs/Delete/5
        public ActionResult Delete(decimal? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FINE fINE = db.FINES.Find(id);
            if (fINE == null)
            {
                return HttpNotFound();
            }
            return View(fINE);
        }

        // POST: FINEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            FINE fINE = db.FINES.Find(id);
            db.FINES.Remove(fINE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Payment(int id, decimal fine_amt)
        {
            Session["loanID"] = id;
            Session["fine_amt"] = fine_amt;
            return View();
        }
        public ActionResult ConfirmPayment()
        {
            //var result = db.FINES.Where(x => x.LOAN_ID == x.BOOK_LOANS.LOAN_ID);


            //var value = Convert.ToInt32(Session["loanID"]);
            //FINE fine = new FINE();
            //foreach(var items in db.FINES)
            //{
            //    if(items.LOAN_ID == value)
            //    {
            //        fine.LOAN_ID = value;
            //        fine.FINE_AMT = Convert.ToDecimal(Session["fine_amt"]);
            //        fine.PAID = DateTime.Now;
            //        db.FINES.Remove(items);
            //        db.SaveChanges();
            //        db.FINES.Add(fine);
            //        db.SaveChanges();
            //        return View("Index");
            //    }
            //}
            var id = Convert.ToInt32(Session["loanID"]);
            FINE fine = db.FINES.Find(id);
            fine.FINE_AMT = Convert.ToDecimal(Session["fine_amt"]);
            fine.PAID = DateTime.Now;
            db.FINES.Add(fine);
            db.SaveChanges();
                    
                
            

            
            return View("Index");
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
