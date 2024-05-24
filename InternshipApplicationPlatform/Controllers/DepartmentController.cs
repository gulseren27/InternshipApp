using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternshipApplicationPlatform.Models;
using InternshipApplicationPlatform.Models.ViewModel;

namespace InternshipApplicationPlatform.Controllers
{
    public class DepartmentController : Controller
    {
        ETUEntities db = new ETUEntities();
        Class cs = new Class();

        // GET: Department
        public ActionResult Index()
        {
            cs.Users = db.User.ToList();
            return View(cs);
        }

        public ActionResult ApproveApplication()
        {
            return View();
        }
        public ActionResult RejectApplication()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateInformation(int id)
        {
            var c = db.Company.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);
        }
        [HttpPost]
        public ActionResult UpdateInformation(HttpPostedFileBase imza,  User d)
        {
            var dprtm = db.User.Find(d.UserId);
            if (dprtm == null)
            {
                return HttpNotFound();
            }
            if (imza != null && imza.ContentLength > 0)
            {
                var fileName = Path.GetFileName(imza.FileName);
                var path = Path.Combine(Server.MapPath("~/Image/Department"), fileName);
                imza.SaveAs(path);

                dprtm.Signature= fileName;
            }

            dprtm.Name = d.Name;
            dprtm.Surname = d.Surname;
            dprtm.Tel = d.Tel;
            dprtm.Email = d.Email;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public ActionResult GetListAllInternships()
        {
            cs.Internships = db.Internship.ToList();
            return View(cs);
        }
       
        public ActionResult ReceivedByMe() // Tarafıma ulaşan başvurular
        {
            cs.Internships = db.Internship.Where(u => u.HeadOfDepartmentApproval == null).ToList();
            return View(cs);
        }

    }
}