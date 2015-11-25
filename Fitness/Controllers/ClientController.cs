using Fitness.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.Security;
using Fitness.Filters;
using WebMatrix.WebData;

namespace Fitness.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class ClientController : Controller
    {
        //Создаем контекст данных
        private FitnessEntities db = new FitnessEntities();

        public ActionResult Index()
        {
            UserProfile user = db.UserProfiles.Find(WebSecurity.CurrentUserId);
            ViewBag.user = user;
            return View(user.subscription);
        }

        //Создать абонемент
        public ActionResult Create(int id)
        {
            if (db.UserProfiles.Find(id).idSubscription == null)
            {
                Dictionary<int, string> kinds = new Dictionary<int, string>();
                foreach (trainingprogram tp in db.trainingprograms)
                {
                    kinds.Add(tp.idTraining, tp.typeTraining + " / " + tp.kindTraining + " / " + tp.price + " .руб");
                }
                ViewBag.kinds = kinds;
                ViewBag.id = id;
                return View();
            }
            return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            subscription sub = new subscription();
            sub.duration = int.Parse(formCollection["duration"]);
            sub.idTraining = int.Parse(formCollection["kind"]);
            sub.dateOfPurchase = DateTime.Now;
            sub.status = "Оформлен";
            db.subscriptions.Add(sub);
            db.SaveChanges();
            UserProfile u = db.UserProfiles.Find(int.Parse(formCollection["id"]));
            u.idSubscription = sub.idSubscription;
            db.Entry(u).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = u.UserId });
        }

        //Изменить данные о себе
        public ActionResult Edit()
        {
            UserProfile user = db.UserProfiles.Find(WebSecurity.CurrentUserId);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(UserProfile user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //Удалить абонемент
        public ActionResult Delete(int id = 0)
        {
            subscription subscription = db.subscriptions.Find(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            subscription subscription = db.subscriptions.Find(id);
            int idUser = subscription.UserProfiles.First().UserId;
            db.subscriptions.Remove(subscription);
            db.SaveChanges();
            if (Roles.IsUserInRole("Client"))
            {
                return RedirectToAction("Index", new { id = idUser });
            }
            else 
            {
                return RedirectToAction("Index", "Trainer", new { id = WebSecurity.CurrentUserId });
            }
        }

        //Оплатить абонемент
        public ActionResult PaySubscription(int id)
        {
            subscription subscription = db.subscriptions.Find(id);
            if (subscription != null && !subscription.status.Equals("Оплачен"))
            {
                if (!subscription.trainingprogram.typeTraining.Equals("Общий"))
                {
                    Dictionary<int, string> trainers = new Dictionary<int, string>();
                    foreach (UserProfile u in db.UserProfiles)
                    {
                        if (Roles.IsUserInRole(u.UserName, "Trainer"))
                        {
                            trainers.Add(u.UserId, u.fullName);
                        }
                    }
                    ViewBag.trainers = trainers;
                }

                return View(subscription);
            }
            else
            {
                return RedirectToAction("Index", new { id = subscription.UserProfiles.First().UserId });
            }
        }

        [HttpPost]
        public ActionResult PaySubscription(FormCollection form)
        {
            subscription subscription = db.subscriptions.Find(int.Parse(form["id"]));
            int? idTrainer = int.Parse(form["idTrainer"]);
            UserProfile user = subscription.UserProfiles.First();
            if (!idTrainer.HasValue)
            {
                user.idGroup = null;
            }
            else
            {
                UserProfile trainer = db.UserProfiles.Find(idTrainer);
                user.idGroup = trainer.idGroup;
            }
            subscription.status = "Оплачен";
            db.Entry(user).State = EntityState.Modified;
            db.Entry(subscription).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = user.UserId });
        }

        //Продлить абонемент
        public ActionResult ExtendSubscription(int id = 0)
        {
            subscription subscription = db.subscriptions.Find(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        [HttpPost]
        public ActionResult ExtendSubscription(subscription subscription, int duration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscription);
        }

        //Посмотреть статус абонемента
        public ActionResult ViewStatusSubscription(int id)
        {
            return View(db.subscriptions.Find(id));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
