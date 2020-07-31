# GraphQL for Revit
a GraphQL endpoint for Revit that can be accessed locally and remotely over the web.

![GraphiQL Client](https://github.com/gregorvilkner/Revit2GraphQL/blob/master/graphiql.png)
Check out [BIMrx.Marconi.pdf](https://github.com/gregorvilkner/Revit2GraphQL/blob/master/BIMrx.Marconi%20SinglePage%201.1.pdf) for more information.

## Current Release

1) a webapi controller you can test at http://localhost:9000/api/about returning the path of your revit file.
1) a graphql controller you can call using graphiql at http://localhost:9000/api/graphql
1) a marconi client that allows interoperability with your revit session over the web at https://marconi4revitwebapp.azurewebsites.net/

## Currently Covered GraphQL Routes and Elements

1) hello returns the path of your revit file
1) FamilyCategories, Families, FamilySymbols
1) FamilyInstances with Parameters
1) ViewSchedules with data
1) MEP Systems - entrance only: id, name, mep domain
1) Assemblies - entrance only: id, name
1) Sheets - entrance only: id, name
1) Phases - entrance only: id, name

there is a mutation that let's you change parameters.

here are two .addin files to load the 2 commands:

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


