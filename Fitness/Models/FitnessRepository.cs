using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public class FitnessRepository : IFitnessRepository
    {
        private FitnessEntities db = new FitnessEntities();

        public IEnumerable<UserProfile> GetAllUserProfiles() {
            return db.UserProfiles.ToList();
        }

        //public UserProfile GetUserProfileByID(int id)
        //{
        //    return db.UserProfiles.FirstOrDefault(d => d.UserId == id);
        //}

        //public void CreateNewUserProfile(UserProfile UserProfileToCreate)
        //{
        //    db.UserProfiles.Add(UserProfileToCreate);
        //    db.SaveChanges();
        //}

        //public int SaveChanges()
        //{
        //    return db.SaveChanges();
        //}

        //public void DeleteUserProfile(int id)
        //{
        //    var conToDel = GetUserProfileByID(id);
        //    db.UserProfiles.Remove(conToDel);
        //    db.SaveChanges();
        //}
    }
}