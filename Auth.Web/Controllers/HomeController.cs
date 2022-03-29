using Auth.Data;
using Auth.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Authorization;Integrated Security=true;";

        public IActionResult Index()
        {

          var repo = new UserRepository(_connectionString);
            var adds = repo.GetAllAdds();
            if (User.Identity.IsAuthenticated)
            {
                foreach(var add in adds)
                {
                    if(User.Identity.Name==add.Email)
                    {
                        add.BelongsToUser = true;
                    }
                    else
                    {
                        add.BelongsToUser = false;
                    }
                }
               
            }
            AddViewModel adv = new AddViewModel();
            adv.adds = adds;
            return View(adv);
        }
        [HttpPost]
        public IActionResult DeleteAdd(int id)
        {
            var repo= new UserRepository(_connectionString);
            repo.DeleteAdd(id);
            return Redirect("/");
        }
        [Authorize]
        public IActionResult NewAdd()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult NewAdd(Add add)
        {
            var repo = new UserRepository(_connectionString);
            var user=repo.GetByEmail(User.Identity.Name);
            add.LoginId = user.Id;
            repo.NewAdd(add);
            return View();
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.GetByEmail(User.Identity.Name);
            var adds = repo.GetAllAdds(user.Id);
            AddViewModel adv = new AddViewModel();
            adv.adds = adds;
            return View(adv);
        }

    }
}
