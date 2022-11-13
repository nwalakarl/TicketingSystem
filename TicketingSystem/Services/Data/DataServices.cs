using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Utilities.Comparers;
using TicketingSytem.Models;

namespace TicketingSystem.Services.Data
{
    public static class DataServices
    {
        private static List<Event> Events = new List<Event>();

        static void InitEventsData()
        {
            if (Events.Count == 0)
            {
                Events = new List<Event>{ 
                    new Event { Name = "Phantom of the Opera", City = "New York" },
                    new Event { Name = "Metallica", City = "Los Angeles" },
                    new Event { Name = "Metallica", City = "New York" },
                    new Event { Name = "Metallica", City = "Boston" },
                    new Event { Name = "LadyGaGa", City = "New York" },
                    new Event { Name = "LadyGaGa", City = "Boston" },
                    new Event { Name = "LadyGaGa", City = "Chicago" },
                    new Event { Name = "LadyGaGa", City = "San Francisco" },
                    new Event { Name = "LadyGaGa", City = "Washington" }
                };
            }
        }

        public static List<Event> GetEvents()
        {
            InitEventsData();

            return Events;
        }

        public static List<Event> GetEventsByCity(string city)
        {
            InitEventsData();

            var queryResult = from result in Events
                        where result.City.ToLower() == city.ToLower()
                        select result;
                   

            return queryResult.ToList();
        }
        public static void AddEvent(Event eventRecord)
        {
            Events.Add(eventRecord);
        }
    }
}
