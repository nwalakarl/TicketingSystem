using TicketingSystem.Services.Price;
using TicketingSytem.Models;

namespace TicketingSystem.Utilities.Comparers
{
    class EventPriceComparer : IComparer<Event>
    {
        public int Compare(Event? x, Event? y)
        {
            if(x == null || y == null)
            {
                return 0;
            }

            return PriceServices.GetPrice(x) - PriceServices.GetPrice(y);
        }
    }
    
}
