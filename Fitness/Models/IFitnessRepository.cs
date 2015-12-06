using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Models
{
    public interface IFitnessRepository
    {
        IEnumerable<UserProfile> GetAllUserProfiles();
        //void CreateNewUserProfile(UserProfile UserProfileToCreate);
        //void DeleteUserProfile(int id);
        //UserProfile GetUserProfileByID(int id);
        //int SaveChanges();
    }
}