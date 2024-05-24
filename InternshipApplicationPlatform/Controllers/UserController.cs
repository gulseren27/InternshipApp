using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternshipApplicationPlatform.Controllers
{
    public class UserController : Controller
    {
        // GET: User
      
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UpdateAccountInformation()
        {
            return View();
        }

        public ActionResult AddAccountInformation()
        {
            return View();
        }
        public ActionResult DeleteAccountInformation()
        {
            return View();
        }
    }
}