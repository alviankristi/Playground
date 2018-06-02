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
        private List<Facility> _data;
        private Mock<IFacilityRepository> _facilityRepository;

        [TestInitialize]
        public void Setup()
        {
            _facilityRepository = new Mock<IFacilityRepository>();
            _facilityRepository.Setup(a => a.Get(It.IsAny<int>())).Returns((int a) => _data.FirstOrDefault(ab => ab.Id == a));

            _data = new List<Facility>
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
            _facilityRepository.Setup(a => a.Create(It.IsAny<Facility>())).Returns(2).Callback((Facility c) =>
            {
                c.Id = _data.Count + 1;
                _data.Add(c);
            });
            var model = new Facility();

            //act
            var service = new FacilityService(_facilityRepository.Object);
            service.Create(model);

            //assert
            Assert.IsNotNull(model.Created);
            Assert.IsNotNull(model.Id);
            Assert.IsTrue(_data.Count == 2);
        }

        [TestMethod]
        public void Update_ValidModel_ReturnFacilityId()
        {

            //arrange
            _facilityRepository.Setup(a => a.Update(It.IsAny<Facility>())).Callback((Facility c) =>
            {
                var getData = _data.FirstOrDefault(a => a.Id == c.Id);
                _data.Remove(getData);
                _data.Add(c);
            });
            var model = new Facility()
            {

                Id = 1,
                Name = "Facitlity 2"
            };

            //act
            var service = new FacilityService(_facilityRepository.Object);
            service.Update(model);
            var updatedData = _data.First(a => a.Id == model.Id);

            //assert
            Assert.IsNotNull(updatedData);
            Assert.AreEqual(updatedData.Name, model.Name);
        }

        [TestMethod]
        public void Delete_ValidId_DataDeleted()
        {

            //arrange
            _facilityRepository.Setup(a => a.Delete(It.IsAny<int>())).Callback((int c) =>
            {
                var deleteddata = _data.First(a => a.Id == c);
                _data.Remove(deleteddata);
            });

            //act
            var service = new FacilityService(_facilityRepository.Object);
            service.Delete(1);

            //assert
            Assert.IsTrue(_data.Count == 0);
        }

        [TestMethod]
        public void GetById_ReturnFacilityIncludingSite()
        {

            //arrange
            _facilityRepository.Setup(a => a.Get(It.IsAny<int>(), It.IsAny<Expression<Func<Facility, object>>[]>())).Returns((int a, Expression<Func<Facility, object>>[] expressions) => _data.FirstOrDefault(ab => ab.Id == a));

            //act
            var service = new FacilityService(_facilityRepository.Object);
            var model = service.GetById(1);

            //assert
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Id, 1);
        }
    }
}
