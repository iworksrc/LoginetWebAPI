using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoginetWebAPI.Controllers;
using LoginetWebAPI.Services;
using LoginetWebAPI.DTO;

namespace LoginetWebAPIUnitTests.Controllers
{
    [TestClass]
    public class AlbumControllerTests
    {
        private AlbumController controller;
        private IDataService dataservice;

        public AlbumControllerTests()
        {
            dataservice = new DataService();
            controller = new AlbumController(dataservice);
        }

        [TestInitialize]
        public void SetupContext()
        {
            dataservice = new DataService();
            controller = new AlbumController(dataservice);
        }

        [TestMethod]
        public void Get_CollectionAlbums_IsNotNull()
        {
            var result = controller.Get() as List<Album>;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Get_AllAlbumsInCollection_IsNotNull()
        {
            var result = controller.Get() as List<Album>;

            CollectionAssert.AllItemsAreNotNull(result);
        }

        [TestMethod]
        public void GetId_Album_IsNotNull()
        {
            Album result = controller.Get(8);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetByUserId_Albums_IsAsExpected()
        {
            int userId = 3;

            var result = controller.Get(userId.ToString()) as List<Album>;

            bool check = result.TrueForAll(a => a.userId == userId);

            Assert.IsTrue(check);
        }


    }
}
