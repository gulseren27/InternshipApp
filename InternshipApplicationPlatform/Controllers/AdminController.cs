using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternshipApplicationPlatform.Models;
using InternshipApplicationPlatform.Models.ViewModel;
using OfficeOpenXml;
namespace InternshipApplicationPlatform.Controllers
{
    public class AdminController : Controller
    {
        ETUEntities db = new ETUEntities();
        Class cs = new Class();

        public ActionResult NewRegisters()
        {
            cs.Users = db.User.Where(u => u.Role == null).ToList();

            return View(cs);
        }

        [HttpGet]
        public ActionResult UpdateRole(int id)
        {
            var user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateRole(User u, string role)
        {
            var userToUpdate = db.User.Find(u.UserId);
            if (userToUpdate == null)
            {
                return HttpNotFound();
            }

            userToUpdate.Role = role;
            db.SaveChanges();
            return RedirectToAction("NewRegisters");
        }

        [HttpGet]
        public ActionResult UploadUsers()
        {
            return View();
        }

        // POST: Admin/UploadUsers
        [HttpPost]
        public ActionResult UploadUsers(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0)
            {
                TempData["Error"] = "Yüklenen dosya boş veya geçersiz.";
                return RedirectToAction("NewRegisters", "Admin");
            }

            try
            {
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var workbook = package.Workbook;
                    if (workbook == null)
                    {
                        TempData["Error"] = "Excel dosyası işlenemedi: Boş çalışma kitabı.";
                        return RedirectToAction("NewRegisters", "Admin");
                    }

                    var worksheet = workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        TempData["Error"] = "Excel dosyası işlenemedi: Çalışma sayfası bulunamadı.";
                        return RedirectToAction("NewRegisters", "Admin");
                    }

                    int successfulInserts = 0;
                    int failedInserts = 0;

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            var name = worksheet.Cells[row, 1].Text?.Trim();
                            var surname = worksheet.Cells[row, 2].Text?.Trim();
                            var tc = worksheet.Cells[row, 3].Text?.Trim();
                            var email = worksheet.Cells[row, 4].Text?.Trim();
                            var password = worksheet.Cells[row, 5].Text?.Trim();
                            var role = worksheet.Cells[row, 6].Text?.Trim();

                            // Log values for debugging
                            System.Diagnostics.Debug.WriteLine($"Row {row} - Name: {name}, Surname: {surname}, TC: {tc}, Email: {email}, Password: {password}, Role: {role}");

                            // Validate required fields
                            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(tc) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                            {
                                System.Diagnostics.Debug.WriteLine($"Row {row} skipped due to missing required fields.");
                                failedInserts++;
                                continue;
                            }

                            var newUser = new User
                            {
                                Name = name,
                                Surname = surname,
                                TC = tc,
                                Email = email,
                                Password = password,
                                Role = role,
                            };

                            db.User.Add(newUser);
                            successfulInserts++;
                        }

                        db.SaveChanges();
                        transaction.Commit();

                        TempData["Success"] = $"{successfulInserts} kullanıcı başarıyla eklendi. {failedInserts} kullanıcı eklenemedi.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Excel dosyası işlenirken bir hata oluştu: " + ex.Message;
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.ToString());
            }

            return RedirectToAction("NewRegisters", "Admin");
        }


    }
}



