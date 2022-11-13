# Ticketing System
So we have a website where we are selling tickets for live events. The specific business scenario here is that we want to do an email campaign.
So imagine where building a service (component) for this email campaign what we want to do is to compose an email with the list of all the live events and send it to the customer.


Assumptions Made:
1. Since the scope does not include ticket sales, there was no need for a *Ticket* Model.
2. A customer can get to any event city directly. This can be visualised as a weighted graph with all noded connected.
3. Expectation is to work with the class definitions for `Customer` and `Event`.


Write a code to add all events in the customer's location to the email. Considering the objects shared above:
1.	What should be your approach to getting the list of events?
2.	How would you call the AddToEmail method in order to send the events in an email?
3.	What is the expected output if we only have the client John Smith?
4.	Do you believe there is a way to improve the code you first wrote?
