# <img src = "http://www.garcia.com.tr/assets/base/img/layout/logos/logo-3.png" width = 200 alt = "Garcia"/>

Garcia is a .net framework that includes many integrations and utility services. It provides developers to develop fast and powerful web apis without wasting any time with integrations.

## Project Architecture:

The architecture is based on the Onion Architecture, so it can be easily used in DDD projects. 
You can take a look at what <a href= "https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/">
Onion Architecture </a> is.

## Which Project Layer Contains What ?
<i> The project basically consists of 4 layers. These are </i> :

| Layer  | Content |
| ------ | ------- |
| Domain: | Contains different types of base entity classes and interfaces.|
| Application:    | Contains some application services , infrastructure services contracts and persistence services contracts. |
| Infrastructure: | The layer of integration and infrastructural services. It contains external service registrations, integration adapter services, some settings classes and interfaces, different kind of base controllers.
| Persistence : | The layer of persistency. It contains repository services, db context classes and registration services of them.

## Table of Contents:

### Repositories:
<ul>
    <li> EntityFramework </li>
    <li> MongoDb </li>
    <li> Cassandra </li>
</ul>

### Message Brookers(AMQP, Pub/Sub):
Garcia has own service bus system. Take a look at <a>here</a>.

You can use the following technologies either with garcia service bus or with their own services.

<ul>
    <li> RabbitMQ </li>
    <li> Redis Pub/Sub </li>
    <li> Kafka: <i>Coming Soon</i> </li>
</ul>

### Caching:
<ul>
    <li> GarciaCache </li>
    <li> Redis </li>
</ul>

### Search Engines:
<ul>
    <li> ElasticSearch </li>
</ul>

### Real Time:
<ul>
    <li> SignalR </li>
    <li> WebSocket: <i>Coming Soon</i> </li>
</ul>

### Push Notification:
<ul>
    <li> Firebase </li>
    <li> Expo Push Notification: <i>Coming Soon</i> </li>
</ul>

### API Gateway / Service Discovery:
<ul>
    <li> Ocelot </li>
    <li> Consul </li>
    <li> Eureka (with Ocelot) </li>
</ul>

### Logging:
<ul>
    <li> Serilog </li>
    <li> Serilog.Elasticsearch </li>
    <li> Serilog.Graylog </li>
</ul>

### Email:
<ul>
    <li> Email </li>
    <li> Email.Mandrill </li>
</ul>

### Localization:
<ul>
    <li> Localization </li>
</ul>

### File Upload:
<ul>
    <li> FileUpload </li>
    <li> FileUpload.AmazonS3 </li>
</ul>

### Marketing:
<ul>
    <li> Marketing.Mailchimp </li>
</ul>
<br></br>

## Installation:

Intall Garcia and it's dependencies using NuGet:

 <code>NuGet</code>

 ```ps
 > Install-Package Garcia
 ```

 Or:

<code>.NET CLI: </code>

 ```ps
 > dotnet add package Garcia
 ```

 ## Quick Start:

Let's create a simple crud service with Entity Framework for a quick start.

### Creating a Project:

 ```ps
 > dotnet new webapi --name QuickStart 
 ```

### Add Garcia To The Project :

 ```ps
 > dotnet add package Garcia
 ```

### Create Sample Entity:

```cs
using Garcia.Domain;

namespace QuickStart;

public class Sample : Entity<long>
{
    public string Name { get; set; }
}
```

### Create SampleDto: 

```cs
public class SampleDto
{
    public long Id { get; private set; }
    public string Name { get; set; }
}
```

### Create SampleController:

```cs
using Garcia.Application.Services;
using Garcia.Infrastructure.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace QuickStart.Controllers;

[ApiController]
public class SampleController : BaseController<IBaseService<Sample, SampleDto, long>, Sample, SampleDto, long>
// BaseController has 4 generic parameters. These are a BaseService, an Entity<TKey>, a Dto of the Entity and the Id type of the Entity in question.
{
    public SampleController(IBaseService<Sample, SampleDto, long> services) : base(services)
    {
    }
}
```

### Register Services To The Program:

Let's register the EntityFramework, repository services and IBaseService.

Add the following statements to the program class:

```cs
using Garcia.Application;
using Garcia.Application.Contracts.Persistence;
using Garcia.Persistence.EntityFramework;
using QuickStart;

var builder = WebApplication.CreateBuilder(args);

// Other configurations ...

builder.Services
    .AddEfCoreInMemory<BaseContext>("Sample")
    .AddEfCoreRepository();

builder.Services.AddBaseService<IAsyncRepository<Sample>, Sample, SampleDto, long>();
// BaseService also has a 4 generic parameters. These are a IAsyncRepository implemented from IAsyncRepository<TEntity, TKey>, an Entity<TKey>, a Dto of the Entity and the Id type of the Entity in question.
```

## Why Not Add An Authentication While You Are At It ?

You need a just few things adding an authentication.

### You Must Create The User:

```cs
using System.ComponentModel.DataAnnotations.Schema;
using Garcia.Domain;
using Garcia.Domain.Identity;

public class User : Entity<long>, IUserEntity<long> // User must be an entity and implemented from IUserEntity<TKey>
{
    public string Username { get; set; }
    public string Password { get; set; }

    [NotMapped]
    public List<string> Roles { get; set; } = new List<string>(); 
    // The role field comes from IUserEntity. If you want, you can customize the Context and define it as the owned entity type. 
    // If you are using MongoDB or Cassandra for persistence of the User you don't need [NotMapped] attribute.
}
```
### Create UserDto:

```cs
using Garcia.Application.Contracts.Identity;

public class UserDto : IUser
{
    public string Id { get; set; }
    public string Username { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}
```

### Create AuthenticationController:

```cs
using Garcia.Application.Identity.Services;
using Garcia.Infrastructure.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace QuickStart.Controllers;

[ApiController]
public class AuthenticationController : BaseAuthController<IAuthenticationService<User, UserDto, long>, User, UserDto, long>
/* BaseAuthController has also 4 generic parameters like a BaseController. 
These are an AuthenticationService, a User BaseType is Entity<TKey> and implemented from IUserEntity<TKey>, 
a UserDto implemented from IUser and the Id type of the User. */
{
    public AuthenticationController(IAuthenticationService<User, UserDto, long> service) : base(service)
    {
    }
}
```


### Register Services To The Program:

Add the following statement to the Program.cs:

```cs
builder.Services.AddAuthenticationService<IAsyncRepository<User>, User, UserDto, long>(builder.Configuration);
/* AuthenticationService also has a 4 generic parameters like BaseService. 
These are a IAsyncRepository implemented from IAsyncRepository<TEntity, TKey>, 
a User BaseType is Entity<TKey> and implemented from IUserEntity<TKey>, a UserDto implemented from IUser and the Id type of the User.*/
```

### Add JwtIssuerOptions To appsettings.json For AuthenticationService:

AuthenticationService uses JwtService for generate and verify a new jwt. JwtService needs JwtIssuerOptions to do this jobs.

```json
  "JwtIssuerOptions": {
    "Issuer": "garcia",
    "Audience": "garcia",
    "SecretKey": "2c47231f-9da0-45a6-b6ea-4cf89281b63d",
    "ValidFor": 7200
  }
```

### Protect Your Apis: 

To protect your controller add Authorize attribute top of the controller.

```cs
using Garcia.Application.Services;
using Garcia.Infrastructure.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuickStart.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class SampleController : BaseController<IBaseService<Sample, SampleDto, long>, Sample, SampleDto, long>
{
    public SampleController(IBaseService<Sample, SampleDto, long> services) : base(services)
    {
    }
}
```

### Configure Your Swagger:

```cs
using Garcia.Infrastructure.Api;

// Other Configurations...

builder.Services.AddSwaggerWithAuthorization();
```

### To Run Your Application:

```ps
> dotnet run
```

<i>You can find complete project in <a href = "https://github.com/GarciaTechnology/garcia/tree/dev/samples/QuickStart">here.</a></i>

<b>And Thats It. Welcome to Garcia.</b>