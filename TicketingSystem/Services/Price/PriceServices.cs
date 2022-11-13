using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Services.Spatial;
using TicketingSytem.Models;

namespace TicketingSystem.Services.Price
{
    public static class PriceServices
    {
        public static int GetPrice(Event e)
        {
            return (SpatialServices.AlphabeticalDistance(e.City, "") + SpatialServices.AlphabeticalDistance(e.Name, "")) / 10;
        }
    }
}
