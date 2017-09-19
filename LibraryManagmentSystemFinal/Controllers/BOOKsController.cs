using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagmentSystemFinal;
using PagedList;
using PagedList.Mvc;

namespace LibraryManagmentSystemFinal.Controllers
{
    public class BOOKsController : Controller
    {
        private LIbraryManagementDBContext db = new LIbraryManagementDBContext();
        public string CARD_ID { get; private set; }

        // GET: BOOKs
        public ActionResult Index(string searchBy, string search,int? page)
        {
            Session.Clear();
            if (searchBy == "ISBN")
            {
                return View(db.SearchNews.Where(x => x.ISBN.ToString() == search).ToList().ToPagedList(page ?? 1, 50));
            }
            else if (searchBy == "Title")
            {
                return View(db.SearchNews.Where(x => x.TITLE.Contains(search)).ToList().ToPagedList(page ?? 1, 50));
            }
            else if (searchBy == "Author")
            {
                return View(db.SearchNews.Where(x => x.AUTHORS.Contains(search)).ToList().ToPagedList(page ?? 1, 50));
            }
            else if (searchBy == "Availability" && search == null)
            {
                var result = (from s in db.SearchNews join b in db.BOOK_LOANS
                              on s.ISBN equals b.ISBN
                              where b.DATE_IN == null
                              select s).ToList();
            
                    return View(db.SearchNews.ToList().ToPagedList(page ?? 1, 50));

            }
            return View(db.SearchNews.ToList().ToPagedList(page ?? 1,50));
        }


        /*public ActionResult SearchList()
        {
            return View(db.Searches.ToList());
        }*/

        // GET: BOOKs/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK bOOK = db.BOOKs.Find(id);
            if (bOOK == null)
            {
                return HttpNotFound();
            }
            return View(bOOK);
        }

        // GET: BOOKs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BOOKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ISBN,TITLE")] BOOK bOOK)
        {
            if (ModelState.IsValid)
            {
                db.BOOKs.Add(bOOK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bOOK);
        }

        // GET: BOOKs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK bOOK = db.BOOKs.Find(id);
            if (bOOK == null)
            {
                return HttpNotFound();
            }
            return View(bOOK);
        }

        // POST: BOOKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ISBN,TITLE")] BOOK bOOK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bOOK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bOOK);
        }

        // GET: BOOKs/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK bOOK = db.BOOKs.Find(id);
            if (bOOK == null)
            {
                return HttpNotFound();
            }
            return View(bOOK);
        }

        // POST: BOOKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            BOOK bOOK = db.BOOKs.Find(id);
            db.BOOKs.Remove(bOOK);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CheckOut(long? id)
        {
            Session["ISBN"] = id;
            return View();

        }
        public ActionResult CheckInBook(long? id)
        {
            return View();
        }
        public ActionResult SetCardId(BORROWER borrower)
        {
            int cardId = borrower.CARD_ID;
            Session["cardID"] = borrower.CARD_ID;
            bool result = db.BORROWERs.Where(x => x.CARD_ID == cardId).Any();
            if (result == false) return View("StudentNotFound");
           
            return RedirectToAction("ReturnBookLoan");
        }
        public ActionResult ReturnBookLoan()
        {
            return View(db.BOOK_LOANS.ToList().Where(x => x.CARD_ID == Convert.ToInt32(Session["cardID"])));

        }

        public ActionResult ValidateCheckOut(BORROWER borrower)
        {
            int cardId = borrower.CARD_ID;
            Session["cardID"] = borrower.CARD_ID;
            bool result = db.BORROWERs.Where(x => x.CARD_ID == cardId).Any();

            //1. First Condition--If the student is registered
            if (result == false) return View("StudentNotFound");
            //2. Second Condition--If the student has already issued three books
            //int count = db.BOOK_LOANS.Where(x => x.CARD_ID == cardId).Count();
            int count = (from bk in db.BOOK_LOANS
                         join b in db.BORROWERs
                         on bk.CARD_ID equals b.CARD_ID
                         where borrower.CARD_ID == bk.CARD_ID
                         where bk.DATE_IN == null
                         select bk.LOAN_ID).Count();

            int overDue_count = (from bk in db.BOOK_LOANS
                                 join b in db.BORROWERs
                                 on bk.CARD_ID equals b.CARD_ID
                                 where bk.CARD_ID == borrower.CARD_ID
                                 where DateTime.Now > bk.DUE_DATE
                                 select bk.LOAN_ID).Count();

            //Session["count"] = count;
            if (count == 3 || overDue_count > 0)
            {
                return View("MaximumLimitReached");
            }

            //3. If the student has unpaid Fine
            var query = (from bk in db.BOOK_LOANS
                         join f in db.FINES
                        on bk.LOAN_ID equals f.LOAN_ID
                         where f.PAID == null
                         select bk.CARD_ID).ToList();
            foreach (int value in query)
            {
                if (value == borrower.CARD_ID)
                {
                    return View("Fine");
                }
            }
            //4. If the book is overdue
            var query1 = (from bk in db.BOOK_LOANS
                          where DateTime.Now > bk.DUE_DATE
                          select bk.CARD_ID).ToList();

            foreach (int value in query1)
            {
                if (value == borrower.CARD_ID)
                {
                    return View("BookOverDue");
                }
            }

            //5. If Book is not Available
            var reqBook = (from b in db.BOOK_LOANS
                           where b.DATE_IN == null
                           select b.ISBN).ToList();
            foreach (var value in reqBook)
            {
                if (value == Convert.ToInt64(Session["ISBN"]))
                {
                    return View("BookNotAvailable");
                }
            }
            return RedirectToAction("AddBookLoan");

        }
        public ActionResult AddBookLoan()
        {
            return View(db.BOOK_LOANS.ToList().Where(x => x.CARD_ID == Convert.ToInt32(Session["cardID"])));
        }
        public ActionResult ConfirmBookLoan()
        {

            BOOK_LOANS bk_loan = new BOOK_LOANS();
            bk_loan.CARD_ID = Convert.ToInt32(Session["cardID"]);
            /* foreach(var value in db.BOOK_LOANS)
            {
                if (value.ISBN == bk_loan.ISBN)
                    return View("BookNotAvailable");
                else
                {
                }
            }*/
            bk_loan.ISBN = Convert.ToInt64(Session["ISBN"]);
            bk_loan.DATE_OUT = DateTime.Now;
            bk_loan.DUE_DATE = DateTime.Now.AddDays(14);
            bk_loan.DATE_IN = null;
            db.BOOK_LOANS.Add(bk_loan);
            db.SaveChanges();
            //Session.Clear();
            return RedirectToAction("AddBookLoan");

        }

        public ActionResult CheckIn(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var fine = db.FINES.Find(id);
            /*if (fine == null)
            {
                return HttpNotFound();
            }*/
            var book_loan = db.BOOK_LOANS.Find(id);
            //if (fine.BOOK_LOANS.DATE_IN > fine.BOOK_LOANS.DUE_DATE)
            //DateTime due_date = new DateTime(fine.BOOK_LOANS.DUE_DATE);
            DateTime due_date = book_loan.DUE_DATE;
            if (DateTime.Now > Convert.ToDateTime(due_date))
            {
                FINE fines = new FINE();
                DateTime default_date =  new DateTime(9999,12,12);
                var days = DateTime.Now.Subtract(due_date).TotalDays;
                fines.FINE_AMT = Convert.ToDecimal(0.25 * days);
                fines.LOAN_ID = book_loan.LOAN_ID;
                fines.PAID = default_date;
                FINEsController cont = new FINEsController();
                cont.Create(fines);
                db.SaveChanges();
                return View("FineAdded");
            }
            else
            {
                BOOK_LOANS bkln = db.BOOK_LOANS.Find(id);
                bkln.DATE_IN = DateTime.Now;
                var isbn = bkln.ISBN;
                //SearchNew book = db.SearchNews.Find(isbn);
                //db.SearchNews.Add(book);
                db.SaveChanges();
            }
            return RedirectToAction("ReturnBookLoan");
        }

        public ActionResult ViewFines(BOOK_LOANS book_loans)
        {
            //return View(db.FINES.ToList().Where(x => x.CARD_ID == Convert.ToInt32(Session["cardID"])));

            //var query = (from bk in db.BOOK_LOANS
            //             join f in db.FINES
            //            on bk.LOAN_ID equals f.LOAN_ID
            //            where bk.CARD_ID == Convert.ToInt32(Session["cardID"])
            //             select f.LOAN_ID).ToList();

            return View(db.FINES.ToList().Where(x=>x.BOOK_LOANS.CARD_ID == Convert.ToInt32(Session["cardID"])));           
        }


        public ActionResult TestTemp()
        {
            TempData["Test"] = "Sample TempData Test";
            return View();
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
