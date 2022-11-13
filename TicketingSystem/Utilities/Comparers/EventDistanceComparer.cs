using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Services.Price;
using TicketingSystem.Services.Spatial;
using TicketingSytem.Models;

namespace TicketingSystem.Utilities.Comparers
{
    public class EventDistanceComparer:IComparer<Event>
    {
        private string _relativeEventCity;
        public EventDistanceComparer(string relativeEventCity)
        {
            _relativeEventCity = relativeEventCity;
        }


        public int Compare(Event? x, Event? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            int eventXDistance = 0;
            int eventYDistance = 0;

            if (_relativeEventCity != null)
            {
                eventXDistance = SpatialServices.GetDistance(_relativeEventCity, x.City);
                eventYDistance = SpatialServices.GetDistance(_relativeEventCity, y.City);
            }

            if (eventXDistance == eventYDistance)
            {
                int eventXPrice = PriceServices.GetPrice(x);
                int eventYPrice = PriceServices.GetPrice(y);

                return eventXPrice - eventYPrice;
            }

            
            return eventXDistance - eventYDistance;
        }
    }
}
