using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PG.BLL;
using PG.Model;
using PG.Repository;

namespace PG.BLLTest
{
    [TestClass]
    public class FacilityServiceTest
    {
        //All method should be test at least once
        List<Facility> data;
        Mock<IFacilityRepository> facilityRepository;

        [TestInitialize]
        public void Setup()
        {
            facilityRepository = new Mock<IFacilityRepository>();
            facilityRepository.Setup(a => a.Get(It.IsAny<int>())).Returns((int a) => data.FirstOrDefault(ab => ab.Id == a));

            data = new List<Facility>
            {
                new Facility
                {
                    Id = 1,
                    Name = "Facitlity 1"
                }
            };
        }

        [TestMethod]
        public void Create_ValidModel_ReturnFacilityId()
        {

            //arrange
            facilityRepository.Setup(a => a.Create(It.IsAny<Facility>())).Returns(2).Callback((Facility c) =>
            {
                c.Id = data.Count + 1;
                data.Add(c);
            });
            var model = new Facility();

            //act
            var service = new FacilityService(facilityRepository.Object);
            service.Create(model);

            //assert
            Assert.IsNotNull(model.Created);
            Assert.IsNotNull(model.Id);
            Assert.IsNotNull(data.Count == 2);
        }

        [TestMethod]
        public void Update_ValidModel_ReturnFacilityId()
        {

            //arrange
            facilityRepository.Setup(a => a.Update(It.IsAny<Facility>())).Callback((Facility c) =>
            {
                var getData = data.FirstOrDefault(a => a.Id == c.Id);
                data.Remove(getData);
                data.Add(c);
            });
            var model = new Facility()
            {

                Id = 1,
                Name = "Facitlity 2"
            };

            //act
            var service = new FacilityService(facilityRepository.Object);
            service.Update(model);
            var updatedData = data.FirstOrDefault(a => a.Id == model.Id);

            //assert
            Assert.AreEqual(updatedData.Name, model.Name);
        }

        [TestMethod]
        public void Delete_ValidId_DataDeleted()
        {

            //arrange
            facilityRepository.Setup(a => a.Delete(It.IsAny<int>())).Callback((int c) =>
            {
                var deleteddata = data.FirstOrDefault(a => a.Id == c);
                data.Remove(deleteddata);
            });
            var model = new Facility();

            //act
            var service = new FacilityService(facilityRepository.Object);
            service.Delete(1);

            //assert
            Assert.IsTrue(data.Count == 0);
        }

        [TestMethod]
        public void GetById_ReturnFacilityIncludingSite()
        {

            //arrange
            facilityRepository.Setup(a => a.Get(It.IsAny<int>(), It.IsAny<Expression<Func<Facility, object>>[]>())).Returns((int a, Expression<Func<Facility, object>>[] expressions) => data.FirstOrDefault(ab => ab.Id == a));

            //act
            var service = new FacilityService(facilityRepository.Object);
            var model = service.GetById(1);

            //assert
            Assert.AreEqual(model.Id, 1);
        }
    }
}
