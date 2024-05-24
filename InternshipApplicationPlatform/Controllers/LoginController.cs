using InternshipApplicationPlatform.Models;
using InternshipApplicationPlatform.Models.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace InternshipApplicationPlatform.Controllers
{
    public class LoginController : Controller
    {
        ETUEntities db = new ETUEntities();
        Class cs = new Class();

        // Giriş
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
       public ActionResult SignIn(User u)
        {
            if (ModelState.IsValid)
            {
                var user = db.User.FirstOrDefault(x => x.Email == u.Email.Trim() &&
                 x.Password == u.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    if (user.Role == "Bölüm Staj Komitesi Başkanı")
                    {
                        return RedirectToAction("Department", "Index");
                    }
                    else if (user.Role == "Bölüm Başkanı")
                    {
                        return RedirectToAction("Department", "Index");
                    }
                    else if (user.Role == "Dekan Yardımcısı")
                    {
                        return RedirectToAction("Faculty", "Index");
                    }
                    else if (user.Role == "Öğrenci")
                    {
                        return RedirectToAction("IndexStudent", "Student");
                    }
                    else if (user.Role == "Staj Yeri Yetkilisi")
                    {
                        return RedirectToAction("Company", "Index");
                    }

                }
            }

            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            return View();
        }

        // Çıkış
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut(); 

            return RedirectToAction("SignIn", "Login"); 
        }

        // Kayıt
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User u)
        {
            if (string.IsNullOrEmpty(u.Name) || string.IsNullOrEmpty(u.Surname) ||
                string.IsNullOrEmpty(u.TC) || string.IsNullOrEmpty(u.Email) ||
                string.IsNullOrEmpty(u.Tel) || string.IsNullOrEmpty(u.Password))
            {
                ModelState.AddModelError("", "Tüm alanlar doldurulmalıdır.");
                return View(u);
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(u.TC, @"^\d{11}$"))
            {
                ModelState.AddModelError("TC", "T.C. Kimlik Numarası 11 haneli bir sayı olmalıdır.");
                return View(u);
            }

            if (!u.Email.Contains("@"))
            {
                ModelState.AddModelError("Email", "Geçerli bir e-posta adresi giriniz.");
                return View(u);
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(u.Tel, @"^\d+$"))
            {
                ModelState.AddModelError("Tel", "Telefon numarası sadece rakamlardan oluşmalıdır.");
                return View(u);
            }

            db.User.Add(u);
            db.SaveChanges();
            return RedirectToAction("SignIn", "Login");
        }


        //// Şifre sıfırlama



        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = model.Email;

                // Rastgele bir şifre oluştur
                string newPassword = Guid.NewGuid().ToString().Substring(0, 8);

                // Şifreyi e-posta ile birlikte gönderilecek metin içeriği
                string emailBody = $@"
            Merhaba,
            <br/><br/>
            Yeni şifreniz: <b>{newPassword}</b>
            <br/><br/>
            Bu şifreyi kullanarak giriş yapabilirsiniz.
            <br/><br/>
            İyi günler dileriz.";

                // Mail gönderme işlemi
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("sedayazici66@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "Şifre Sıfırlama";
                    mail.Body = emailBody;
                    mail.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("sedayazici66@gmail.com", "njcd aggz ksnr ydxm"); // Uygulama şifresi veya normal şifre
                    smtp.EnableSsl = true;

                    smtp.Send(mail);

                    TempData["Message"] = "Şifre sıfırlama e-postası gönderildi.";
                }
                catch (SmtpException smtpEx)
                {
                    TempData["Error"] = $"E-posta gönderme hatası (SMTP): {smtpEx.Message}";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"E-posta gönderme hatası: {ex.Message}";
                }

                return RedirectToAction("SignIn");
            }
            else
            {
                TempData["Error"] = "Geçerli bir e-posta adresi giriniz.";
                return View(model);
            }
        }

    }






    //[HttpGet]
    //public ActionResult ForgotPassword()
    //{
    //    return View();
    //}

    //[HttpPost]
    //public ActionResult ForgotPassword(Help model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        string email = model.Email;

    //        // Rastgele bir şifre oluştur
    //        string newPassword = Guid.NewGuid().ToString().Substring(0, 8);

    //        // Şifreyi e-posta ile birlikte gönderilecek metin içeriği
    //        string emailBody = $@"
    //    Merhaba,
    //    <br/><br/>
    //    Yeni şifreniz: <b>{newPassword}</b>
    //    <br/><br/>
    //    Bu şifreyi kullanarak giriş yapabilirsiniz.
    //    <br/><br/>
    //    İyi günler dileriz.";

    //        // Mail gönderme işlemi
    //        MailService _mailService = new MailService("smtp.gmail.com", 587, "sedayazici66@gmail.com", "zsrz fpob pfdi sbvf");
    //        _mailService.SendMail(email, "Şifre Sıfırlama", emailBody);

    //        return RedirectToAction("SignIn");
    //    }
    //    else
    //    {
    //        return Content("Error From Google ReCaptcha : ");
    //    }
    //}





}
