Apistry
========  

Apistry is in short, an ASP.NET Web API documentation provider. However, it is a smart alternative to your typical weakly-typed
XML or DSL-based documentation. Apistry provides you the ability to document your existing ASP.NET Web API using static typing.

But that's not all...  
Document your DTOs and ApiControllers using a fluent, composable API. Based upon this metadata, Apistry will generate 
documentation - creating: resource, method, parameter, property documentation and request/response examples. 

Apistry will even auto-generate documention for all un-documented DTOs and ApiControllers using reflection - leveraging 
AutoFixture and ObjectHydrator to create meaningful example data. You can then use this finalized documentation to bind as an 
MVC model and render it out however you like.

Example
=======
```csharp
WebApiDocumentationMetadata metadata =
    new WebApiDocumentationMetadataBuilder()
        .DocumentDto<UserDto>("System user")
            .For(u => u.Id)
                .Description("A unique identifier for a user used by the application.")
                .Example(53)
            .For(u => u.FirstName)
                .Description("A user's first name.")
                .Example("Daniel")
            .For(u => u.LastName)
                .Description("A user's last name.")
                .Example("Gioulakis")
            .For(u => u.Address)
            .For(u => u.Profile)
            .For(u => u.Friends)
        .DocumentDto<AddressDto>("A user's address.")
            .For(a => a.AddressLine1)
                .Description("The street number.")
                .Example("150 E Robinson St")
            .For(a => a.City)
                .Description("The city name.")
                .Example("Orlando")
            .For(a => a.State)
                .Description("The state.")
                .Example("Florida")
            .For(a => a.ZipCode)
                .Description("The zip code.")
                .Example("32801")
            .For(a => a.Country)
                .Description("The country.")
                .Example("United States")
        .DocumentDto<ProfileDto>("A user's profile.")
            .For(p => p.About)
                .Description("About user profile section.")
                .Example("A developer originally from South Florida.")
            .For(p => p.Picture)
                .Description("The profile picture.")

        .DocumentController<UsersController>()
            .Resource("Users")
            .Summary("User-related API operations.")
            .DescribeAction(c => c.DeleteUser(default(int)))
                .Name("Delete a User")
                .Summary("Delete a user and all associated data.")
                .DescribeParameter<int>("userId", "The user's identity.")
                .Returns(HttpStatusCode.NoContent)
            .DescribeAction(c => c.GetUsers())
                .Name("Get all Users")
                .Summary("Returns all user's for your site.")
                .Returns<IEnumerable<UserDto>>()
            .DescribeAction(c => c.GetUser(default(int)))
                .Name("Get a User")
                .Summary("Gets a specific user.")
                .Returns<UserDto>()
            .DescribeAction(c => c.PatchUser(default(int), default(PatchRequest<UserDto>)))
                .Name("Modify a User")
                .Summary("Modify specific attributes about a user.")
                .DescribeParameter<int>("userId", "The user's identity.")
                .DescribeParameter<PatchRequest<UserDto>>("patchRequest", "The key-value user properties to patch.")
                .Returns(HttpStatusCode.NoContent)
            .DescribeAction(c => c.PostUser(default(UserDto)))
                .Name("Create a new user account.")
                .Summary(@"This is will create a new user with associated profile information.")
                .DescribeParameter<UserDto>("userDto")
                .Returns(HttpStatusCode.Created)
                .Alert("This endpoint is only accessible by administrators.")
                .Information("Important information regarding this endpoint.")
            .DescribeAction(c => c.PutUser(default(int), default(UserDto)))
                .Name("Update a User")
                .Summary("Update the entire user entity.")
                .Returns(HttpStatusCode.NoContent)
```

Sample UI Generated Documentation
---------------------------------
![](http://imageshack.com/a/img661/9889/L13U46.png "Example UI Mashup of Box.com and Google+ APIs")  


Roadmap
=======
**IMPORTANT:** *Apistry was originally written as a proof-of-concept.  The codebase will be re-written again from scratch quite soon.  The current state of this project is NOT production-ready.*

- [x] add support for convention-based extensibility point for request & reponse property inclusion/exclusion
- [ ] refactor initial version codebase for maintainability
- [ ] refactor documentation provider to support overriding of default behavior
- [ ] fix lingering bugs
- [ ] fix rendering of IEnumerable<> request & response examples (currently generates example as single)
- [ ] add intellisense documentation already!
- release v1.0.0
- [ ] add support for DTO documentation via XML and attributes
- [ ] add support for required & optional headers for both request & response
- release v1.1.x
- [ ] add Apistry UI - Apistry's UI starter template
- release v1.2.x
- [ ] add support for Hydra / JSON-LD serialization
- [ ] add support for a TryItNow feature (in-browser Hydra client api explorer)
- release v2.0.0

Continuous Delivery
===================
The following environments are setup for on-demand build and deployments. To reduce AWS costs, the following servers are only online during deployments.  All NuGet packages are officially hosted on NuGet servers.

**CI TeamCity Server:** *https://teamcity.wakingventure.com/*  
**CI NuGet Package Source:** *https://nuget.wakingventure.com/*  
**CI Symbol Source Server:** *http://symbolsource.wakingventure.com/*  
