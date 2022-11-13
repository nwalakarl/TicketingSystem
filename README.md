# Ticketing System
So we have a website where we are selling tickets for live events. The specific business scenario here is that we want to do an email campaign.
So imagine where building a service (component) for this email campaign what we want to do is to compose an email with the list of all the live events and send it to the customer.


Assumptions Made:
1. Since the scope does not include ticket sales, there was no need for a `Ticket` Model.
2. A customer can get to any event city directly. This can be visualised as a weighted graph with all noded connected.
3. Expectation is to work with the class definitions for `Customer` and `Event`.
4. There's only one event per city.
5. There is no time/schedule for the events; Events returned are only retrieved based on *distance* and *price*.


A. Write a code to add all events in the customer's location to the email. Considering the objects shared above:
1.	What should be your approach to getting the list of events?
```C#
 var queryResult =  from result in Events
                    where result.City.ToLower() == city.ToLower()
                    select result;
                   
```
2.	How would you call the AddToEmail method in order to send the events in an email?
```C#
List<Event> events = DataServices.GetEventsByCity(customer.City);

events.Sort(new EventPriceComparer());

foreach (var item in events)
{
    int? price = PriceServices.GetPrice(item);
    EmailServices.AddToEmail(customer, item, price);
}
                   
```
3.	What is the expected output if we only have the client John Smith?
4.	Do you believe there is a way to improve the code you first wrote?

B. Write a code to add the 5 closest events to the customer's location to the email.
1.	What should be your approach to getting the distance between the customer’s city and the other cities on the list?
Two options were considered here:
- a) To define a 2-dimensional grid (e.g. 100 x 100) and use Pythagorean Theorem to compute the distance between City A and City B. The events are randomly placed on the grid (then transforming the x, y coordinates to fit within the 100 x 100 grid). With a larger grid size, this would be a more realistic approach.
- b) To use a 1-Dimensional space, where distances are directly computed using the character-length of the `City` and `Name` of the events. This appeard to be a more straightforward approach for the scale of this project.

2.	How would you get the 5 closest events and how would you send them to the client in an email?
To iterate through the events using the PriorityQueue data structure (Dictionary without the values), using the distance as the Priority parameter and 'taking' the top 5 results.
```C#
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

```
3.	What is the expected output if we only have the client John Smith?
If we only have John Smith, then the distance would be computed only one and maybe made a part of the Event class as a property. 

4.	Do you believe there is a way to improve the code you first wrote?
The above code can be made more efficient by caching the computed distances using the Cities as key. This is done in the 'GetDistance()' method.


C.	If the GetDistance method is an API call which could fail or is too expensive, how will uimprove the code written in 2? Write the code.
```C#
try
{
    if (fromCity == null || toCity == null)
    {
        return 0;
    }

    if(fromCity == toCity)
    {
        return 0;
    }

    string distanceCacheKey = fromCity + toCity;

    if (CachedDistances.ContainsKey(distanceCacheKey))
    {
        return CachedDistances[distanceCacheKey];
    }

    return AlphebiticalDistance(fromCity, toCity);
}
catch (Exception)
{
    // Returns a zero for the distance
    return 0;
}
```
D. If the GetDistance method can fail, we don't want the process to fail. What can be done?
Code it. (Ask clarifying questions to be clear about what is expected business-wise).
To ensure the process does not fail, we wrap the logic for computing distance in a `try-catch` and return 0 as the default. This allows us to still display the results to the Customers, but then sorted by `Price`.

E. If we also want to sort the resulting events by other fields like price, etc. to determine whichones to send to the customer, how would you implement it? Code it.
- We can add a `Price` field to Event class
- Write a Price Comparer which is passed to Sort method.
```C#
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
```
- The Comparer for sorting by distace uses the price as a fallback for situations where the distances are the same or events are in the same city
```C#
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
```
F. One of the questions is: how do you verify that what you’ve done is correct.
By using TDD. This approach was used for this task using `TicketingSystem` Project.




