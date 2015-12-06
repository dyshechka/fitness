using Fitness.Filters;
using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;


namespace Fitness.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class AdminController : Controller
    {
        private IFitnessRepository repository;
        
        //Для тестов откомментировать строку ниже
        //private FitnessEntities db;
        
        //Создаем контекст данных (для описания тестов строку ниже закомментировать)
        private FitnessEntities db = new FitnessEntities();

        public AdminController() : this(new FitnessRepository()) { }

        public AdminController(IFitnessRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            return View("Index", repository.GetAllUserProfiles());
        }

        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public ActionResult Create(RegisterModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password,
                    propertyValues: new
                        {
                            fullName = model.fullName,
                            dateOfBirth = model.dateOfBirth,
                            email = model.email,
                            telephone = model.telephone,
                            frozen = true
                        });
                Roles.AddUserToRole(model.UserName, form["role"]);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id = 0)
        {
            ViewBag.roles = Roles.GetAllRoles().OfType<String>().ToList();
            return View(db.UserProfiles.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                UserProfile user = db.UserProfiles.Find(id);
                if (user != null)
                {
                    Roles.RemoveUserFromRole(user.UserName, Roles.GetRolesForUser(user.UserName)[0]);
                    string role = form["role"];
                    Roles.AddUserToRole(user.UserName, role);
                }
                return RedirectToAction("Index");
            }
            return View(form);
        }

        public ActionResult Delete(int id = 0)
        {
            UserProfile user = db.UserProfiles.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            UserProfile user = db.UserProfiles.Find(id);
            if (user != null)
            {
                Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                Membership.DeleteUser(user.UserName, true);
            }
            return RedirectToAction("Index");
        }

    }
}
