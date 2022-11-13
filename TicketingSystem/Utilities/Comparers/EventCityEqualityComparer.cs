using System.Diagnostics.CodeAnalysis;
using TicketingSytem.Models;

namespace TicketingSystem.Utilities.Comparers
{
    class EventCityEqualityComparer : IEqualityComparer<Event>
    {
        public bool Equals(Event? x, Event? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.City.ToLower() == y.City.ToLower();
        }

        public int GetHashCode([DisallowNull] Event obj)
        {
            if(obj == null)
            {
                return 0; // throw new ArgumentNullException("obj");
            }

            return obj.GetHashCode();
        }
    }
   
}

