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
    public class StudentController : Controller
    {
        // GET: Student
        ETUEntities db = new ETUEntities();
        Class cs = new Class();
       
        public ActionResult IndexStudent()
        {
            cs.Users = db.User.ToList();
            cs.Students = db.Student.ToList();
            return View(cs);
        }

        [HttpGet]
        public ActionResult NewApplication()
        {
            return View();
        }


        [HttpPost]
        public ActionResult NewApplication(HttpPostedFileBase StudentImg, HttpPostedFileBase imza, HttpPostedFileBase transcriptFile, Student c, User u)
        {
            // Yeni bir başvuru oluştur
            var newApp = new Internship();

            var user = db.User.Find(u.UserId);
            if (user == null)
            {
                return HttpNotFound("Kullanıcı bulunamadı.");
            }

            // Öğrenci resmini yükle ve kaydet
            if (StudentImg != null && StudentImg.ContentLength > 0)
            {
                var studentImgFileName = Path.GetFileName(StudentImg.FileName);
                var studentImgPath = Path.Combine(Server.MapPath("~/Image/Student"), studentImgFileName);
                StudentImg.SaveAs(studentImgPath);
                newApp.StudentImg = studentImgFileName;
            }

            // İmza dosyasını yükle ve kaydet
            if (imza != null && imza.ContentLength > 0)
            {
                var signatureFileName = Path.GetFileName(imza.FileName);
                var signaturePath = Path.Combine(Server.MapPath("~/Image/Student"), signatureFileName);
                imza.SaveAs(signaturePath);
                user.Signature = signatureFileName;
            }

            // Kullanıcı bilgilerini güncelle
            user.FullName = $"{u.Name} {u.Surname}";
            user.Email = u.Email;
            user.Tel = u.Tel;
            user.Adress = u.Adress;
            user.TC = u.TC;
            db.SaveChanges();

            // Öğrenci bilgilerini güncelle
            var student = db.Student.Find(c.StudentId);
            if (student == null)
            {
                return HttpNotFound("Öğrenci bulunamadı.");
            }
            student.CV = c.CV;
            student.FatherName = c.FatherName;
            student.HealthInsurance = c.HealthInsurance;
            student.MotherName = c.MotherName;
            student.No = c.No;
            student.PlaceOfBirth = c.PlaceOfBirth;
            student.SSK = c.SSK;
            student.DateOfBirth = c.DateOfBirth;
            db.SaveChanges();

            // Transcript dosyasını yükle ve kaydet
            if (transcriptFile != null && transcriptFile.ContentLength > 0)
            {
                var transcriptFileName = Path.GetFileName(transcriptFile.FileName);
                var transcriptPath = Path.Combine(Server.MapPath("~/Transcript"), transcriptFileName);
                transcriptFile.SaveAs(transcriptPath);
                newApp.Transcript = transcriptFileName;
            }

            // Başvuru bilgilerini doldur ve veritabanına kaydet
            newApp.StudentId = c.StudentId;
            newApp.UserId = u.UserId;
            // Diğer gerekli başvuru bilgilerini doldurun
            db.Internship.Add(newApp);
            db.SaveChanges();

            return RedirectToAction("IndexStudent");
        }


        public ActionResult GetListApplication()
        {
            cs.Internships = db.Internship.ToList();
            return View(cs);
        
    }
        public ActionResult GetListPastApp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateInfo(int id)
        {
            // Kullanıcı bilgisini çek
            User user = db.User.FirstOrDefault(u => u.UserId == id);

            // Öğrenci bilgisini çek
            Student student = db.Student.FirstOrDefault(s => s.UserId == id);

            var c = db.User.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }

            return View(c);
        }

        [HttpPost]

        public ActionResult UpdateInfo(HttpPostedFileBase StudentImg, HttpPostedFileBase imza, Student c, User u)
        {

            var student = db.Student.Find(c.StudentId);
            var user = db.User.Find(u.UserId);
            if (student == null)
            {
                return HttpNotFound();
            }

            if (StudentImg != null && StudentImg.ContentLength > 0)
            {
                var fileName = Path.GetFileName(StudentImg.FileName);
                var path = Path.Combine(Server.MapPath("~/Image/Student"), fileName);
                StudentImg.SaveAs(path);

                student.StudentImg = fileName;
            }
            if (imza != null && imza.ContentLength > 0)
            {
                var fileName = Path.GetFileName(imza.FileName);
                var path = Path.Combine(Server.MapPath("~/Image/Student"), fileName);
                imza.SaveAs(path);

                user.Signature = fileName;
            }

            user.FullName = u.Name + " " + u.Surname;
            user.Email = u.Email;
            user.Tel = u.Tel;
            user.Adress = u.Adress;
            user.TC = u.TC;
            student.CV = c.CV;
            student.FatherName = c.FatherName;
            student.HealthInsurance = c.HealthInsurance;
            student.MotherName = c.MotherName;
            student.No = c.No;
            student.PlaceOfBirth = c.PlaceOfBirth;
            student.SSK = c.SSK;
            student.Transcript = c.Transcript;
            student.DateOfBirth = c.DateOfBirth;
            

            db.SaveChanges();
            return RedirectToAction("IndexStudent");

        }


    }
}