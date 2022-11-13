using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketingSystem.Services.Data;
using TicketingSystem.Services.Spatial;
using TicketingSytem.Models;

namespace TrackingSystemTest
{
    [TestClass]
    public class TicketingSystemTests
    {
     
        [TestMethod]
        public void GetEvents_AtCustomerCity_CheckCount()
        {
            // Arrange
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };            

            // Act
            var eventsAtCity = DataServices.GetEventsByCity(customer.City);

            // Assert
            double actual = 3;
            Assert.AreEqual(eventsAtCity.Count, actual, $"Unable to get the events at {customer.City}");
        }

        [TestMethod]
        public void Get_5_Events_ClosestToCustomerCity_CheckCount()
        {
            // Arrange
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };
            var events = DataServices.GetEvents();

            // Act
            var eventsAtCity = SpatialServices.GetNClosestEvents(customer,events,5);

            // Assert
            double actual = 5;
            Assert.AreEqual(eventsAtCity.Count, actual, $"Unable to get the 5 closest events to {customer.City}");
        }

        [TestMethod]
        public void Get_Distance_BetweenSameCity()
        {
            // Arrange
            string city1 = "New York";
            string city2 = "New York";

            // Act         

            var distance = SpatialServices.GetDistance(city1, city2);

            // Assert
            double actual = 0;
            Assert.AreEqual(distance, actual, $"Distance between same city, {city1} is not 0");
        }

        [TestMethod]
        public void Get_Distance_Between_Different_Cities()
        {
            // Arrange
            string city1 = "New York";
            string city2 = "Chicago";

            // Act         

            var distance = SpatialServices.GetDistance(city1, city2);

            // Assert
            double actual = 0;
            Assert.IsTrue(distance > actual, $"Distance between same city, {city1} is not 0");
        }


    }
}