using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Services.Spatial;
using TicketingSytem.Models;

namespace TicketingSystem.Services.Email
{
    public static class EmailServices
    {
        public static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = SpatialServices.GetDistance(c.City, e.City);

            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
    }
}
