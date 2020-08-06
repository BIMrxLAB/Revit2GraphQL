# GraphQL for Revit
This project contains a GraphQL endpoint for Revit that can be accessed locally as well as remotely over the web.

![GraphiQL Client](https://github.com/gregorvilkner/Revit2GraphQL/blob/master/graphiql.png)
Check out [BIMrx.Marconi.pdf](https://github.com/gregorvilkner/Revit2GraphQL/blob/master/BIMrx.Marconi%20SinglePage%201.1.pdf) for more information.

## Current Itteration

The current repo includes projects for 2 individual Revit addins:
1) a webapi hosted in Revit that exposes a GraphQL controller at http://localhost:9000/api/graphql. a very basic "about" controller that can be used for simple get requests at http://localhost:9000/api/about
1) a marconi client that allows interoperability with your revit session over the web at https://marconi4revitwebapp.azurewebsites.net/

## Getting Started: Nuget Dance and Versioning

Personally, I always love to get latest. When Nuget suggests consolidations or updates I usually go for it. Unfortunately, this is not possible here. The main components that make this project work are described below. 

Here is what we know should not be updated:

1) GraphQL-Parser - leave it at 3.0.0, which otherwise breakes GraphQL
2) System.Diagnostics.DiagnosticSource - leave it at 4.5.1, which otherwise breakes ServiceBus
3) Newtonsoft.Json - it's a bit of a mess. You want to go low for Revit, and you can't go too high for GraphQL. GraphQL is actively trying to loose the Newtonsoft.Json dependency.

### Shared Infrastructure

The Schema and the Resolver are both .NET Standard 2.0 projects. We're using .NET Standard so we can reference them in .NET Core web projects and in .NET Framework Revit addins. Also, this forces the Revit addins to be .NET Framework 4.7 at least.

#### RevitGraphQLSchema

The Schema defines the various GraphQL object types and holds interfaces for queries and mutations. 

It's sole dependency should be GraphQL 2.4.0.

#### RevitGraphQLResolver

The Resolver is where GraphQL queries are received and resolved against the Revit API. 

Dependencies include GraphQL 2.4.0 and the very awesome WhiteShareq RevitTask 3.0.0.

### Local GraphQL Endpoint for Revit

There are 2 projects that make up the local GraphQL endpoint for Revit: a web server and the command. 

#### RevitWebServer

The library basically builds a selfhosted OWIN web server with 2 controllers: a simple About controller with a single GET method and the GraphQL controller with it's standard sole POST method. I've followed a couple of tutorials for OWIN'ing:

1) https://braincadet.com/category/c-sharp/
1) http://www.learnonlineasp.net/2017/10/use-owin-to-self-host-aspnet-web-api.html

Dependencies include GraphQL 2.4.0, Newtonsoft.Json 12.0.3, RevitTask 3.0.0, and a bunch of OWIN and AspNet.WebApi stuff.

#### RevitGraphQLCommand

This is the external command that hooks into Revit and has some very minimalistic WPF UI.

Dependencies include Newtonsoft.Json 9.0.1 (that's the on Revit likes), RevitTask 3.0.0, and the Owin.Host.HttpListener 4.1.0 (that one trickled over from the webserver)

### Remote GraphQL using BIMrx.Marconi

This external command uses the same resolver, but doesn't build an internal webserver. Instead we use a Azure ServiceBus client to listen and respond to queries from a messaging layer that uses Azure AMQP. The schema is exposed on a web page https://marconi4revitwebapp.azurewebsites.net/ and in the Revit addin. The request and response JObjects are routed through Azure Service Bus queues.

Dependencies do not include OWIN stuff. Instead, we have GraphQL 2.4.0, Newtonsoft JSON 10.0.3 (required by GraphQL, and newer versions won't work, unfortunately), RevitTask 3.0.0, and Microsoft.Azure.ServiceBus 4.1.3 together with Microsoft.Identity.Client 4.17.0.

## GraphQL Routes and Elements

1) hello returns the path of your revit file
1) FamilyCategories, Families, FamilySymbols
1) FamilyInstances with Parameters
1) ViewSchedules with data
1) MEP Systems - entrance only: id, name, mep domain
1) Assemblies - entrance only: id, name
1) Sheets - entrance only: id, name
1) Phases - entrance only: id, name

there is a mutation that let's you change parameters.

## .addin files to load the revit commands

~~~ XML
<?xml version="1.0" encoding="utf-8" standalone="no"?>
<RevitAddIns>
  <AddIn Type="Application">
    <Name>Local GraphQL Endpoint</Name>
    <Assembly>C:\Users\xyz\source\repos\Revit2GraphQL\src\RevitGraphQLCommand\bin\Debug\RevitGraphQLCommand.dll</Assembly>
    <AddInId>73dc677a-5a96-41dd-b3be-ca81d06dfc2c</AddInId>
    <FullClassName>RevitGraphQLCommand.App</FullClassName>
    <VendorId>Microdesk</VendorId>
    <VendorDescription>https://www.microdesk.com/bimrx/</VendorDescription>
  </AddIn>
</RevitAddIns>
~~~

~~~ XML
<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
  <AddIn Type="Application">
    <Name>BIMrx.Marconi</Name>
    <Assembly>C:\Users\xyz\source\repos\Revit2GraphQL\src\RevitMarconiCommand\bin\Debug\RevitMarconiCommand.dll</Assembly>
    <AddInId>26516f0e-cdb2-43d0-9169-b4473db259ac</AddInId>
    <FullClassName>RevitMarconiCommand.App</FullClassName>
    <VendorId>Microdesk</VendorId>
    <VendorDescription>https://www.microdesk.com/bimrx/</VendorDescription>
  </AddIn>
</RevitAddIns>
~~~


