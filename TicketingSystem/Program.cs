using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TicketingSystem.Services.Data;
using TicketingSystem.Services.Email;
using TicketingSystem.Services.Price;
using TicketingSystem.Services.Spatial;
using TicketingSystem.Utilities.Comparers;
using TicketingSytem.Models;


namespace TicketingSytem 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to our Ticketing System");
            Console.WriteLine("-------------------------------\n");

            int option = -1;

            while (option != 0)
            {
                // Display ticketing system action options.
                Console.WriteLine("Choose an option from the following list:");
                Console.WriteLine("\t1 - View all event locations");
                Console.WriteLine("\t2 - View all event locations at City");
                Console.WriteLine("\t3 - Email events at Customer Location");
                Console.WriteLine("\t4 - Email 5 closest events at Customer Location");

                Console.WriteLine("\t0 - Quit");

                Int32.TryParse(Console.ReadLine(), out option);

                switch (option)
                {
                    case 1:
                        DisplayEvents(option);
                        break;
                    case 2:
                        DisplayEvents(option);
                        break;
                    case 3:
                        EmailEvents(option);
                        break;
                    case 4:
                        EmailEvents(option);
                        break;
                }
            }
        }

        static void EmailEvents(int? option)
        {
            if (option > 2)
            {

                // Prompt to enter customer name.
                Console.WriteLine("Enter customer name, and then press Enter");
                string customerName = Console.ReadLine();

                // Promt to enter customer location.
                Console.WriteLine("Enter customer city, and then press Enter");
                string customerCity = Console.ReadLine();

                var customer = new Customer { Name = customerName, City = customerCity };

                if (option == 3)
                {
                    List<Event> events = DataServices.GetEventsByCity(customer.City);

                    events.Sort(new EventPriceComparer());

                    foreach (var item in events)
                    {
                        int? price = PriceServices.GetPrice(item);
                        EmailServices.AddToEmail(customer, item, price);
                    }
                }
                else if (option == 4)
                {
                    List<Event> events = DataServices.GetEvents();

                    List<Event> closestEvents = SpatialServices.GetNClosestEvents(customer.City, events);

                    foreach (var item in closestEvents)
                    {
                        var price = PriceServices.GetPrice(item);
                        EmailServices.AddToEmail(customer, item, price);
                    }
                }
            }
        }

        static void DisplayEvents(int? option)
        {
            var events = DataServices.GetEvents();

            if (option == 1)
            {
                events.Sort(new EventPriceComparer());
                int count = 0;

                foreach (var item in events)
                {
                    int? price = PriceServices.GetPrice(item);
                    count++;
                    Console.Out.WriteLine($"{count} {item.Name} in {item.City}"
                    + (price.HasValue ? $" for ${price}" : ""));
                }
            }
            else if (option == 2)
            {

                // Promt to enter customer city.
                Console.WriteLine("Enter customer city, and then press Enter");
                string customerCity = Console.ReadLine();

                var eventsAtCity = DataServices.GetEventsByCity(customerCity);

                eventsAtCity.Sort(new EventPriceComparer());
                int count = 0;
                foreach (var item in eventsAtCity)
                {
                    int distance = SpatialServices.GetDistance(item.City, customerCity);

                    int? price = PriceServices.GetPrice(item);
                    count++;
                    Console.Out.WriteLine($"{count} {item.Name} in {item.City}"
                        + (distance > 0 ? $" ({distance} miles away)" : "")
                    + (price.HasValue ? $" for ${price}" : ""));
                }
            }
        }
    }
}

