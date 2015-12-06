using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fitness.Controllers;
using System.Web.Mvc;
using Fitness.Models;
using Fitness.Tests.Models;
using System.Web.Routing;
using System.Security.Principal;
using System.Web;
using System.Collections.Generic;

namespace Fitness.Tests.Controllers
{
    [TestClass]
    public class AdminControllerUnitTest
    {
        [TestMethod]
        public void IndexView()
        {
            IFitnessRepository repo = new InMemoryFitnessRepository();
            AdminController contr = new AdminController(repo);
            var result = contr.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        //private static AdminController GetAdminController(IFitnessRepository fitrepository)
        //{
        //    AdminController admcontroller = new AdminController(fitrepository);
        //    admcontroller.ControllerContext = new ControllerContext()
        //    {
        //        Controller = admcontroller,
        //        RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
        //    };
        //    return admcontroller;
        //}

        //[TestMethod]
        //public void GetAllUserProfileFromRepository()
        //{
        //    // Arrange
        //    UserProfile user1 = GetUserProfileName("2", "2", DateTime.Today , "2@2.ru", true, 2);
        //    UserProfile user2 = GetUserProfileName("3", "3", DateTime.Today, "3@3.ru", true, 3);
        //    InMemoryFitnessRepository fitrepository = new InMemoryFitnessRepository();
        //    fitrepository.Add(user1);
        //    fitrepository.Add(user2);
        //    var controller = GetAdminController(fitrepository);
        //    var result = controller.Index();
        //    var datamodel = GetAdminController(fitrepository).Index();
        //    Assert.AreEqual(datamodel, user1);
        //    Assert.AreEqual(datamodel, user2);
        //}

        //UserProfile GetUserProfileName(string userName, string name, DateTime dateBirth, string email, bool frozen, int telephone)
        //{
        //    return new UserProfile
        //    {
        //        UserName = userName,
        //        fullName = name,
        //        dateOfBirth = dateBirth,
        //        email = email,
        //        frozen = frozen,
        //        telephone = telephone            
        //    };
        //}

        // private class MockHttpContext : HttpContextBase
        //{
        //    private readonly IPrincipal user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);
 
        //    public override IPrincipal User
        //    {
        //        get
        //        {
        //            return user;
        //        }
        //        set
        //        {
        //            base.User = value;
        //        }
        //    }
        //}
    }

}

