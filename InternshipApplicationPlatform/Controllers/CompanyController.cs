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
    public class CompanyController : Controller
    {
        // GET: Company
        ETUEntities db = new ETUEntities();
        Class cs = new Class();

        public ActionResult Index()
        {
            cs.Companies = db.Company.ToList();
            cs.Users = db.User.ToList();
            cs.Students = db.Student.ToList();
            return View(cs);
        }
        public ActionResult GetListInformation()
        {
            cs.Companies = db.Company.ToList();
            return View(cs);
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
        public ActionResult UpdateInformation(HttpPostedFileBase logo, Company c)
        {
            var company = db.Company.Find(c.CompanyId);
            
            if (company == null)
            {
                return HttpNotFound();
            }

            if (logo != null && logo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(logo.FileName);
                var path = Path.Combine(Server.MapPath("~/Image/Company"), fileName);
                logo.SaveAs(path);

                company.Signature = fileName;
            }
            company.Adress = c.Adress;
            company.CompanyName = c.CompanyName;
            company.Tel = c.Tel;
            company.FirmaMail = c.FirmaMail;       
            company.Meslek = c.Meslek;                 
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UpdateInformationUser(int id)
        {
            var c = db.Company.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);
        }
        [HttpPost]
        public ActionResult UpdateInformationUser(HttpPostedFileBase yetkiliImg, HttpPostedFileBase imza, Company c)
        {
            var company = db.Company.Find(c.CompanyId);
            if (company == null)
            {
                return HttpNotFound();
            }

            if (yetkiliImg != null && yetkiliImg.ContentLength > 0)
            {
                var fileName = Path.GetFileName(yetkiliImg.FileName);
                var path = Path.Combine(Server.MapPath("~/Image/Company"), fileName);
                yetkiliImg.SaveAs(path);

                company.YetkiliImg = fileName;
            }
            if (imza != null && imza.ContentLength > 0)
            {
                var fileName = Path.GetFileName(imza.FileName);
                var path = Path.Combine(Server.MapPath("~/Image/Company"), fileName);
                imza.SaveAs(path);

                company.Signature = fileName;
            }
           
            company.YetkiliName = c.YetkiliName
            company.YetkiliSurname=c.YetkiliSurname
            company.Meslek = c.Meslek;
            company.YetkiliEmail = c.YetkiliEmail;
            company.YetkiliTel = c.YetkiliTel;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //public ActionResult PastAppDetails()
        //{
        //    cs.Internships = db.Internship.Where(u => u.IsComplete == 1).ToList();
        //    return View(cs);
        //}
        public ActionResult ReceivedAppDetails()
        {
            cs.Internships = db.Internship.Where(u => u.CompanyApprovalDate == null).ToList();
            return View(cs);
        }

        

    }
}