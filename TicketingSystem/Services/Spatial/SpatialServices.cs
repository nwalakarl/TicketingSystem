using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Utilities.Comparers;
using TicketingSytem;
using TicketingSytem.Models;

namespace TicketingSystem.Services.Spatial
{
    public static class SpatialServices
    {
        private static Dictionary<string, int> CachedDistances = new Dictionary<string, int>();
        public static List<Event> GetNClosestEvents(string customerCity, List<Event> events, int n = 5)
        {
            List<Event> result = new List<Event>();

            /***********************************************************************************
            * 
            * Approach 1: 
            * 
            * var eventDistance = new List<EventDistance>();
            * 
            * foreach (Event item in events)
            * {
            *       if (!eventDistance.Any(e => e.Event.City == item.City))
            *       {
            *           var distance = GetDistance(item.City, customer.City);
            *
            *           eventDistance.Add(new EventDistance() { Event = item, Distance = distance });
            *       }
            * }               
            *
            * result = eventDistance.OrderBy(e => e.Distance).Select(e => e.Event).Take(5).ToList();
            * 
            *              
            */

            PriorityQueue<Event, int> priorityQueue = new PriorityQueue<Event, int>();

            foreach (Event e in events)
            {
                var distance = GetDistance(e.City, customerCity);

                priorityQueue.Enqueue(e, distance);
            }


            int count = 0;

            while (count < n)
            {
                result.Add(priorityQueue.Dequeue());

                count++;
            }           

            result.Sort(new EventDistanceComparer(customerCity));

            return result;
        }

        /*****************************************************************
         *  
         *  IMPROVED THE GetDistance Method by using a dictionary.
         *  
         * public static int GetDistance(string fromCity, string toCity)
         * {
         *    return AlphebiticalDistance(fromCity, toCity);
         * }
         * ***************************************************************/

        public static int GetDistance(string fromCity, string toCity)
        {
            // Assuming Idempotent calls, retries for 404, timeouts, etc.
            int numberOfTries = 5;


            while (numberOfTries > 0)
            {
                numberOfTries--;

                try
                {
                    if (fromCity == null || toCity == null)
                    {
                        return 0;
                    }

                    if (fromCity.ToLower() == toCity.ToLower())
                    {
                        return 0;
                    }

                    // Computes a bidirectional key for the cache.
                    string[] citiesArray = { fromCity, toCity };

                    Array.Sort(citiesArray, (x, y) => x.CompareTo(y));

                    string distanceCacheKey = String.Join('-', citiesArray);

                    if (CachedDistances.ContainsKey(distanceCacheKey))
                    {
                        return CachedDistances[distanceCacheKey];
                    }
                    else
                    {
                        int computedDistance = AlphabeticalDistance(fromCity, toCity);
                        CachedDistances.Add(distanceCacheKey, computedDistance);

                        return computedDistance;
                    }


                }
                catch (Exception)
                {
                    // Log exception
                }

            }

            return 0;

        }

        public static int AlphabeticalDistance(string s, string t)
        {
            var result = 0;
            for (int i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                result += Math.Abs(s[i] - t[i]);
            }

            for (int i = 0; i < Math.Max(s.Length, t.Length); i++)
            {
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}
