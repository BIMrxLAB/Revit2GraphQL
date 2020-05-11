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
