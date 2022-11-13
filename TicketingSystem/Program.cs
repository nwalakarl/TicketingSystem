// See https://aka.ms/new-console-template for more information
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

Console.WriteLine("Welcome to our Ticketing System");
Console.WriteLine("-------------------------------\n");

int option = -1;

while(option != 0)
{
    // Display title as the C# console calculator app.
    Console.WriteLine("Choose an option from the following list:");
    Console.WriteLine("\t1 - View all event locations");
    Console.WriteLine("\t2 - View all event locations at City");
    Console.WriteLine("\t3 - Email events at Customer Location");
    Console.WriteLine("\t4 - Email 5 closest events to Customer Location");

    Console.WriteLine("\t0 - Quit");
    option = Convert.ToInt32(Console.ReadLine());//Console.Read();

    switch (option)
    {
        case 1:
            Console.WriteLine($"Your result:");
            break;
        case 2:
            Console.WriteLine($"Your result: ");
            break;
        case 3:
            EmailEvents(option);
            break;
        case 4:
            EmailEvents(option);
            break;
    }

  




}

void EmailEvents(int option)
{
    if (option > 2)
    {

        // Declare variables and then initialize to zero.
        string customerName = "";
        string customerCity = "";


        // Prompt to enter customer name.
        Console.WriteLine("Enter customer name, and then press Enter");
        customerName = Console.ReadLine();

        // Promt to enter customer location.
        Console.WriteLine("Enter customer city, and then press Enter");
        customerCity = Console.ReadLine();

        var customer = new Customer { Name = customerName, City = customerCity };

        if (option == 3)
        {
            var events = DataServices.GetEventsByCity(customer.City);

            events.Sort(new EventPriceComparer());

            foreach (var item in events)
            {
                var price = PriceServices.GetPrice(item);
                EmailServices.AddToEmail(customer, item, price);
            }
        }
        else if (option == 4)
        {
            var events = DataServices.GetEvents();

            var closestEvents = SpatialServices.GetNClosestEvents(customer, events);

            foreach (var item in closestEvents)
            {
                var price = PriceServices.GetPrice(item);
                EmailServices.AddToEmail(customer, item, price);
            }
        }
    }
}






namespace TicketingSytem
{

    public class Solution
    {
        public static void Main(string[] args)
        {
            var events = DataServices.GetEvents();

            /***************************************************************************
             * QUESTION 1:
             * Write a code to add all events in the customer's location to the email. 
             * Considering the objects shared above:
             * *************************************************************************/


            //1. find out all events that arein cities of customer
            // then add to email.
            var customer = new Customer { Name = "Mr. Fake", City = "New York" };

            var query = DataServices.GetEventsByCity(customer.City);

            // 1. TASK
            foreach (var item in query)
            {
                EmailServices.AddToEmail(customer, item);
            }
            /*
            *	We want you to send an email to this customer with all events in their city
            *	Just call AddToEmail(customer, event) for each event you think they should get
            */

            /***************************************************************************
             * QUESTION 2:
             * Write a code to add the 5 closest events to the customer's location to 
             * the email:
             * *************************************************************************/

            // Approach 1: Brute force
            var closestEvents = SpatialServices.GetNClosestEvents(customer, events);
            
            foreach (var item in closestEvents)
            {
                EmailServices.AddToEmail(customer, item);
            }

            /***************************************************************************
            * QUESTION 3:
            * If the GetDistance method is an API call which could fail or is too 
            * expensive, how will uimprove the code written in 2? Write the code.
            * 
            * *************************************************************************/

        }

        /// <summary>
        /// Returns the closest n events to customer's city including customer's city
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="events"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        //static List<Event> GetNClosestEvents(Customer customer, List<Event> events, int n = 5)
        //{
        //    List<Event> result = new List<Event>();

        //    PriorityQueue<Event, int> priorityQueue = new PriorityQueue<Event, int>();

        //    foreach (Event e in events.Distinct(new EventCityEqualityComparer()))
        //    {
                
        //        var distance = GetDistance(e.City, customer.City);

        //        priorityQueue.Enqueue(e, distance);
                
        //    }


        //    int count = 0;

        //    while(count <n)
        //    {
        //        result.Add(priorityQueue.Dequeue());

        //        count++;
        //    }
            

        //    /*
        //    Approach 1: 

        //    var eventDistance = new List<EventDistance>();

        //    foreach (Event item in events)
        //    {
        //        if (!eventDistance.Any(e => e.Event.City == item.City))
        //        {
        //            var distance = GetDistance(item.City, customer.City);

        //            eventDistance.Add(new EventDistance() { Event = item, Distance = distance });
        //        }
        //    }

        //    result = eventDistance.OrderBy(e => e.Distance).Select(e => e.Event).Take(5).ToList();*/

        //    result.Sort(new EventPriceComparator());

        //    return result;
        //}

        // You do not need to know how these methods work
        //static void AddToEmail(Customer c, Event e, int? price = null)
        //{
        //    var distance = GetDistance(c.City, e.City);
        //    Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
        //    + (distance > 0 ? $" ({distance} miles away)" : "")
        //    + (price.HasValue ? $" for ${price}" : ""));
        //}
        //public static int GetPrice(Event e)
        //{
        //    return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
        //}
        //public static int GetDistance(string fromCity, string toCity)
        //{
        //    return AlphebiticalDistance(fromCity, toCity);
        //}
        //private static int AlphebiticalDistance(string s, string t)
        //{
        //    var result = 0;
        //    var i = 0;
        //    for (i = 0; i < Math.Min(s.Length, t.Length); i++)
        //    {
        //        // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
        //        result += Math.Abs(s[i] - t[i]);
        //    }
        //    for (; i < Math.Max(s.Length, t.Length); i++)
        //    {
        //        // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
        //        result += s.Length > t.Length ? s[i] : t[i];
        //    }
        //    return result;
        //}
    }

    /*class EventComparator : IComparer<Event>
    {
        public int Compare(Event? x, Event? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            int eventX = Solution.GetPrice(x);
            int eventYPrice = Solution.GetPrice(y);

            if (eventXPrice == eventYPrice)
            {
                int eventXPrice = Solution.GetPrice(x);
                int eventYPrice = Solution.GetPrice(y);

                return eventXPrice - eventYPrice;
            }

            if(x.)
            return Solution.GetPrice(x) - Solution.GetPrice(y);
        }
    }*/
}
/*
var customers = new List<Customer>{
new Customer{ Name = "Nathan", City = "New York"},
new Customer{ Name = "Bob", City = "Boston"},
new Customer{ Name = "Cindy", City = "Chicago"},
new Customer{ Name = "Lisa", City = "Los Angeles"}
};
*/

