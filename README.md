# PricingCalculator
<h1>Demo till ett företag</h1>

<h1>Backend - Evaluation Assignment - Pricing calculator</h1>
<h3>Background</h3>
Company X provides services to other companies in the region. Your job is to develop a Web API within a micro-services solution that is solely responsible for calculating prices and should only be called by other services, not humans. There are three types of services; Service A, Service B and Service C. These services have different prices and the prices also depend on the customer, the time period for which they are charged, possible discount (percentage of total price) and free days. A customer can choose which service they want to use independently of other services and customers.

<h3>User story</h3>
As a calling service it should be possible to call an endpoint with customerId, start and end in PricingService to know what to charge for a specific customer

<h3>Requirements</h3>
Build this Web API (PricingService) with appropriate endpoints and implement the following requirements:

* Base costs are as follows:

  * Service A = € 0,2 / working day (monday-friday)<br>
  * Service B = € 0,24 / working day (monday-friday)<br>
  * Service C = € 0,4 / day (monday-sunday)<br>
  
* Each customer can have specific prices for each service (e.g. Customer A only pays € 0,15 per working day for Service A but pays € 0,25 per working day for Service B).

* Customers can have discounts for each service

* Each customer can have a start date for each service

* Each customer can have a number of free days which are global for all services

<h3>Testing</h3>
Use appropriate methodology to test the following scenarios:

<h3>Test case 1</h3>
Customer X started using Service A and Service C 2019-09-20. Customer X also had an discount of 20% between 2019-09-22 and 2019-09-24 for Service C. What is the total price for Customer X up until 2019-10-01?

<h3>Test case 2</h3>
Customer Y started using Service B and Service C 2018-01-01. Customer Y had 200 free days and a discount of 30% for the rest of the time. What is the total price for Customer Y up until 2019-10-01?
