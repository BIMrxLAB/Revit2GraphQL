# Revit2GraphQL
a local GraphQL endpoint for Revit


the only thing this needs in the Revit.exe.config file's //runtime//assemblyBindg section is

~~~ XML
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Owin" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.1.0.0" newVersion="4.1.0.0" />
      </dependentAssembly>
~~~

we keep the various class libraries separated to:

1) accomodate cloud based routing engines such as BIMrx Marconi
1) get arround Revit not liking any Newtonsoft.Json past version 9.0.1

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
