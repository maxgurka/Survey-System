# Survey
Base project to kick start the Tengella survey test.
## Setup
Feel free to use whatever editors/IDEs.
However we strongly recommend *Visual Studio*.

 1. Download and install *Visual Studio Community Edition* - https://visualstudio.microsoft.com/free-developer-offers/
 2. Make sure to install the following workloads (during installation or after using *Visual Studio Installer*) 
	  - *ASP.NET and web development*
	  - *Data storage and processing*
 3. After loading the solution open the *Package Manager Console* and runt the command `Update-Database`

You should now have a working project that can be debugged by hitting F5.
## Information
The project *Tengella.Survey.Data* contains an example DbContext using EF Core. Confgured to use the database server *(localdb)\MSSQLLocalDB* which is installed along with Visual Studio from the workload *Data storage and processing*.
The  project *Tengella.Survey.WebApp* is the default example for a new ASP.NET MVC project. The Home controller has the DbContext injected for an example on how to use it.

See the test document for more information.

### Links

ASP.NET - https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-7.0
EF Core - https://learn.microsoft.com/en-us/ef/core/