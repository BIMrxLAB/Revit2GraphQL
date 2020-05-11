# Revit2GraphQL
a local GraphQL endpoint for Revit.

here's what you should have if you click this correctly:

1) a webapi controller you can call at http://localhost:9000/api/about returning the path of your revit file.
1) a graphql controller you can call using graphiql at http://localhost:9000/api/graphql


the only thing this needs in the Revit.exe.config file's //runtime//assemblyBinding section is

~~~ XML
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Owin" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.1.0.0" newVersion="4.1.0.0" />
      </dependentAssembly>
~~~

we keep the various class library projects separated to:

1) accomodate cloud based routing engines such as BIMrx Marconi
1) get arround Revit 2020 not liking Newtonsoft.Json past version 9.0.1 - OWIN pulls Newtonsoft 6.0.1. so it's not so bad. But GraphQL is needy and wants 10.0.1. at the least.

here's an .addin file to get this loaded:

~~~ XML
<?xml version="1.0" encoding="utf-8" standalone="no"?>
<RevitAddIns>
  <AddIn Type="Command">
    <Name>Revit GraphQL Server</Name>
    <Assembly>"C:\Users\xyz\Source\Repos\Revit2GraphQL\src\RevitCommand\bin\Debug\RevitCommand.dll"</Assembly>
    <AddInId>73dc677a-5a96-41dd-b3be-ca81d06dfc2c</AddInId>
    <FullClassName>RevitCommand.Entry</FullClassName>
    <VendorId>DynamoChild</VendorId>
    <VendorDescription>https://children.dynamo.com/Got2GoSocial</VendorDescription>
  </AddIn>
</RevitAddIns>
~~~


We would like to thank the academy.

And Jeremy Tammik (aka The Building Coder) - who has helped folks get around CS red-tape for a long, long time.

And Ian Keough - who can explain to AECO folks why open source matters. https://youtu.be/wigb6X1b9Hw

And Miroslav Radojević - who dumbed OWIN down so duct tape coders could get past middleware. https://braincadet.com/category/c-sharp/
