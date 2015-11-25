using Fitness.Filters;
using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Fitness.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class TrainerController : Controller
    {
        //Cоздаем контекст данных
        private FitnessEntities db = new FitnessEntities();

        public ActionResult Index()
        {
            return View(db.groups.ToList());
        }

        //Список оформленных абонементов
        public ActionResult ShowFramedSubscription()
        {
            return View(db.subscriptions.Where(subscription => subscription.status.Equals("Оформлен")));
        }

        //Подтвердить абонемент
        public ActionResult SupportSubscription(int id)
        {
            subscription subscription = db.subscriptions.Find(id);
            subscription.status = "Подтвержден";
            db.Entry(subscription).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ShowFramedSubscription");
        }

        //Список замороженных клиентов
        public ActionResult ListFrozenClient()
        {
            List<UserProfile> users = db.UserProfiles.ToList().Where(user => (user.frozen.Equals(true) & Roles.IsUserInRole(user.UserName, "Client"))).ToList();
            return View(users);
        }

        //Заморозить клиента
        public ActionResult FreezeClient(int id)
        {
            UserProfile user = db.UserProfiles.Find(id);
            user.frozen = true;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MarkVisit(int id, int idGroup)
        {
            monitoringvisit visit = new monitoringvisit();
            visit.dateOfVisit = DateTime.Today;
            visit.idUser = id;
            db.monitoringvisits.Add(visit);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = idGroup });
        }

        //Просмотр списка группы
        public ActionResult Details(int id = 0)
        {
            group group = db.groups.Find(id);
            List<UserProfile> users = group.UserProfiles.Where(user => Roles.IsUserInRole(user.UserName, "Client")).ToList();
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //Создание группы
        public ActionResult Create()
        {
            Dictionary<int, string> kinds = new Dictionary<int, string>();
            foreach (trainingprogram tp in db.trainingprograms)
            {
                if (tp.typeTraining.Equals("Общий")) continue;
                kinds.Add(tp.idTraining, tp.typeTraining + " / " + tp.kindTraining);
            }
            ViewBag.kinds = kinds;
            return View();
        }

        [HttpPost]
        public ActionResult Create(group group)
        {
            if (ModelState.IsValid)
            {
                db.groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        //Редактирование группы
        public ActionResult Edit(int id = 0)
        {
            group group = db.groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        //Удаление группы
        public ActionResult Delete(int id = 0)
        {
            group group = db.groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            group group = db.groups.Find(id);
            group.UserProfiles.ToList().ForEach(user =>
            {
                user.frozen = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            });
            db.groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Изменить группу клиенту
        public ActionResult ChangeGroupToClient(int id)
        {
            List<group> groups = new List<group>();
            group group = new group();
            group.nameGroup = "Без группы";
            group.idGroup = 0;
            groups.Add(group);
            groups.AddRange(db.groups.ToList());
            ViewBag.groups = groups;
            return View(db.UserProfiles.Find(id));
        }

        [HttpPost]
        public ActionResult ChangeGroupToClient(FormCollection form)
        {
            UserProfile user = db.UserProfiles.Find(int.Parse(form["idClient"]));
            int? idGroup = int.Parse(form["idGroup"]);
            idGroup = idGroup == 0 ? null : idGroup;
            user.idGroup = idGroup;
            user.frozen = false;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
