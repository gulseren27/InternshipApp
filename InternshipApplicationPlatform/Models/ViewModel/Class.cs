using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternshipApplicationPlatform.Models.ViewModel
{
    public class Class
    {
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Faculty> Faculties { get; set; }
        public IEnumerable<Internship> Internships { get; set; }
        public IEnumerable<Student> Students { get; set; }

        public IEnumerable<User> Users { get; set; }

    }
}