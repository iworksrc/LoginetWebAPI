using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginetWebAPI.Controllers;
using System.Collections.Generic;
using LoginetWebAPI.DTO;
using System.Configuration;
using LoginetWebAPI.Services;
using LoginetWebAPI.Helpers;
using Newtonsoft.Json;
using System.Linq;

namespace LoginetWebAPIUnitTests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private UserController controller;
        private IDataService dataservice;
        private Encryptor encryptor;

        [TestInitialize]
        public void SetupContext()
        {
            dataservice = new DataService();
            controller = new UserController(dataservice);
            encryptor = new Encryptor();
        }

        [TestMethod]
        public void Get_AllUsersEmails_IsNotNull()
        {
            List<User> result = controller.Get() as List<User>;
            List<string> emails = (from u in result select u.email).ToList();
            CollectionAssert.AllItemsAreNotNull(emails);
        }

        [TestMethod]
        public void Get_CollectionUsers_IsNotNull()
        {
            var users = controller.Get() as List<User>;

            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void Get_AllUsersInCollection_IsNotNull()
        {
            var users = controller.Get() as List<User>;

            CollectionAssert.AllItemsAreNotNull(users);
        }

        [TestMethod]
        public void GetId_User_NotNull()
        {
            User user = controller.Get(5);

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void GetId_UserEmail_NotNull()
        {
            User user = controller.Get(1);

            Assert.IsNotNull(user.email);
        }


        [TestMethod]
        public void Get_AllUsersEmails_DecryptedAsWell()
        {
            IEnumerable<User> vanilleCollection = JsonConvert.DeserializeObject<IEnumerable<User>>(dataservice.getAllUsers());
            List<string> vanilleEmails = (from vu in vanilleCollection select vu.email).ToList();
            List<string> decryptedEmails = new List<string>();

            IEnumerable<User> users = controller.Get();
            
            foreach(User u in users)
            {
                decryptedEmails.Add(encryptor.Decrypt(u.email));
            }

            CollectionAssert.AreEquivalent(vanilleEmails, decryptedEmails);
        }
    }
}
