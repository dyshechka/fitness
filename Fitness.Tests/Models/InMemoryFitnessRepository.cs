using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Tests.Models
{
    class InMemoryFitnessRepository : IFitnessRepository
    {
        private List<UserProfile> list = new List<UserProfile>();

        //public Exception ExceptionToThrow { get; set; }

        public IEnumerable<UserProfile> GetAllUserProfiles()
        {
            return list;
        }

        //public UserProfile GetUserProfileByID(int id)
        //{
        //    return list.FirstOrDefault(d => d.UserId == id);
        //}

        //public void CreateNewUserProfile(UserProfile UserProfileToCreate)
        //{
        //    if (ExceptionToThrow != null)
        //        throw ExceptionToThrow;

        //    list.Add(UserProfileToCreate);
        //}

        //public void SaveChanges(UserProfile userProfileToUpdate)
        //{

        //    foreach (UserProfile userProfile in list)
        //    {
        //        if (userProfile.UserId == userProfileToUpdate.UserId)
        //        {
        //            list.Remove(userProfile);
        //            list.Add(userProfileToUpdate);
        //            break;
        //        }
        //    }
        //}

        //public void Add(UserProfile userProfileToAdd)
        //{
        //    list.Add(userProfileToAdd);
        //} 

        //public int SaveChanges()
        //{
        //    return 1;
        //}

        //public void DeleteUserProfile(int id)
        //{
        //    list.Remove(GetUserProfileByID(id));
        //}
    }
}
