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

            company.YetkiliName = c.YetkiliName;
            company.YetkiliSurname = c.YetkiliSurname;
            company.Meslek = c.Meslek;
            company.YetkiliEmail = c.YetkiliEmail;
            company.YetkiliTel = c.YetkiliTel;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult PastAppDetails()
        {
            var completedInternships = db.Internship.Where(i => i.IsComplete.HasValue && i.IsComplete.Value).ToList();
            return View(completedInternships);
        }

        [HttpGet]
        public ActionResult ReceivedAppDetails(int id)
        {
            // Kullanıcı bilgisini çek
            User user = db.User.FirstOrDefault(u => u.UserId == id);

            // Öğrenci bilgisini çek
            Student student = db.Student.FirstOrDefault(s => s.UserId == id);

            //Staj bilgisini çek
            Internship intern = db.Internship.FirstOrDefault(i => i.UserId == id);

            var c = db.User.Find(id);
            var a = db.Student.Find(id);
            var b = db.Internship.Find(id);
            if (c == null || a==null || b == null)
            {
                return HttpNotFound();
            }
                ViewBag.User = user;
                ViewBag.Student = student;
                ViewBag.Internship = intern;

            return View();
        }

        [HttpPost]
        public ActionResult ReceivedAppDetails(Internship i, Student c, User u )
        {
           // cs.Internships = db.Internship.Where(m => m.CompanyApprovalDate == null).ToList();
            var intern = db.Internship.FirstOrDefault(m => m.CompanyApprovalDate == null);
            var student = db.Student.Find(c.StudentId);
            var user = db.User.Find(u.UserId);

            if (intern == null || student == null || user==null)
            {
                return HttpNotFound();
            }

            var viewModel = new InternshipModel();

                intern.StudentName = i.StudentName;
                intern.StudentSurname = i.StudentSurname;
                intern.Transcript = i.Transcript;
                intern.StudentTel = i.StudentTel;
                intern.StudentTC = i.StudentTC;
                intern.StudentSSK = i.StudentSSK;
                intern.StudentsPlaceOfBirth = i.StudentsPlaceOfBirth;
                intern.StudentsMother = i.StudentsMother;
                intern.StudentSignature = i.StudentSignature;
                intern.StudentsHealthInsurance = i.StudentsHealthInsurance;
                intern.StudentsDateOfBirth = i.StudentsDateOfBirth;
                intern.StudentsAddress = i.StudentsAddress;
                intern.StudentMail = i.StudentMail;
                intern.StudentImg = i.StudentImg;
                intern.StudentId = i.StudentId;
                intern.StudentFather = i.StudentFather;
                intern.StudentDepartment = i.StudentDepartment;
                intern.InternshipTime = i.InternshipTime;
                intern.InternshipStart = i.InternshipStart;
                intern.InternshipEnd = i.InternshipEnd;
                intern.HeadOfDepartmentSignature = i.HeadOfDepartmentSignature;
                intern.HeadOfDepartmentName = i.HeadOfDepartmentName;
                intern.HeadOfDepartmentApproval = i.HeadOfDepartmentApproval;
                intern.FacultyId = i.FacultyId;
                intern.DepartmentId = i.DepartmentId;
                intern.DepartmentCommitteeChairmanSignature = i.DepartmentCommitteeChairmanSignature;
                intern.DepartmentCommitteeChairmanName = i.DepartmentCommitteeChairmanName;
                intern.DepartmentCommitteeChairmanApproval = i.DepartmentCommitteeChairmanApproval;
                intern.DeliveryDateToTheCompany = i.DeliveryDateToTheCompany;
                intern.DeanerySignature = i.DeanerySignature;
                intern.DeaneryName = i.DeaneryName;
                intern.DeaneryApproval = i.DeaneryApproval;
                intern.Date = i.Date;
                intern.Cv = i.Cv;
                intern.CompanyTel = i.CompanyTel;
                intern.CompanyName = i.CompanyName;
                intern.CompanyId = i.CompanyId;
                intern.CompanyGuyTitle = i.CompanyGuyTitle;
                intern.CompanyGuySurname = i.CompanyGuySurname;
                intern.CompanyGuySignature = i.CompanyGuySignature;
                intern.CompanyGuyName = i.CompanyGuyName;
                intern.CompanyGuyMail = i.CompanyGuyMail;
                intern.CompanyApprovalDate = i.CompanyApprovalDate;
                intern.CompanyAddres = i.CompanyAddres;
                intern.AcademicYear = i.AcademicYear;
                intern.IsComplete = i.IsComplete;
                intern.Status = i.Status;
                intern.UserId = i.UserId;

            // Kullanıcıya geri bildirim göndermek için TempData 
            TempData["SuccessMessage"] = "Başvuru fakülteye gönderildi.";
            db.SaveChanges();
            return View("IndexFaculty", "Faculty");


          //  return View(cs.Internships); // Verileri View'a gönder
        }

        

    }
}
