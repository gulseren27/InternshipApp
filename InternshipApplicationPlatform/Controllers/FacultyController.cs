using InternshipApplicationPlatform.Models;
using InternshipApplicationPlatform.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternshipApplicationPlatform.Controllers
{
    public class FacultyController : Controller
    {
        // GET: Faculty
        ETUEntities db = new ETUEntities();
        Class cs = new Class();
        public ActionResult IndexFaculty()
        {
            cs.Users = db.User.ToList();
            return View(cs);
        }
        public ActionResult ReceivedByMe()
        {
            cs.Internships = db.Internship.Where(u => u.DeaneryApproval == null).ToList();
            return View(cs);
        }
        public ActionResult ListPast()
        {
            cs.Internships = db.Internship.ToList();
            return View(cs);
        }
        public ActionResult Accept()
        {
            return View();
        }
        public ActionResult Reject()
        {
            return View();
        }
        

        [HttpGet]
        public ActionResult UpdateInfo(int id)
        {
            var c = db.User.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);
        }
        [HttpPost]
        public ActionResult UpdateInfo(HttpPostedFileBase dekanImg, HttpPostedFileBase imza, Faculty c, User u)
        {


            var faculty = db.Faculty.Find(c.FacultyId);
            var user = db.User.Find(u.UserId);
            if (faculty == null)
            {
                return HttpNotFound();
            }

            if (dekanImg != null && dekanImg.ContentLength > 0)
            {
                var fileName = Path.GetFileName(dekanImg.FileName);
                var path = Path.Combine(Server.MapPath("~/Image/Faculty"), fileName);
                dekanImg.SaveAs(path);

                //user.dekanImg = fileName;
            }
            if (imza != null && imza.ContentLength > 0)
            {
                var fileName = Path.GetFileName(imza.FileName);
                var path = Path.Combine(Server.MapPath("~/Image/Faculty"), fileName);
                imza.SaveAs(path);

                user.Signature = fileName;
            }

            user.FullName = u.Name + " " + u.Surname;
            faculty.Name = c.Name;
            user.Email = u.Email;
            user.Tel = u.Tel;
            db.SaveChanges();
            return RedirectToAction("IndexFaculty");
        }

    }
}