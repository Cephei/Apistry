Apistry
========
Apistry provides you the ability to document your existing ASP.NET Web API using static typing.

But that's not all...
Document your DTOs and ApiControllers using a fluent, composable API. Based upon this metadata, Apistry will generate 
documentation - creating: resource, method, parameter, property documentation and request/response examples. 

Apistry will even auto-generate documention for all un-documented DTOs and ApiControllers using intelligent reflection 
techniques and leveraging AutoFixture and ObjectHydrator to create meaningful example data. You can then use this 
finalized documentation to bind as an MVC model and render it out however you like.


Continuous Delivery
-------------------
Current efforts are underway to bring Apistry to a production-ready state. This 
includes specification tests and an efficient build and deployment strategy.  To 
accomplish this, I have set up the following environments for the community to use:

**CI TeamCity Server:** *https://teamcity.wakingventure.com/*  
**CI NuGet Package Source:** *https://nuget.wakingventure.com/*  
**CI Symbol Source Server:** *http://symbolsource.wakingventure.com/*  
