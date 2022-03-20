# CourseReviewApp

A simple application for reviewing online courses.

## Description

Application is built built according to the MPA approach. You can register a new account with the user type "Course owner" or "Course client".
Course owner can add informations about his course to the application. Course client can review courses available in the application.
Special users like Moderator or Admin manage the content and users.

## Technologies

* ASP.NET Core 5.0 MVC
* ASP.NET Core Identity
* Entity Framework Core
* Automapper
* Bootstrap 4
* Font Awesome 5
* JavaScript and jQuery

## Install

Things you need to install to setup and run the application:

* [.NET SDK 5.0](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
* [SQL Server Express LocalDB 2019](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15)
* [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) or [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)

Steps:

1. Clone or download the repository.

2. Build app and create the database.
   * For Visual Studio:    
      1. Open the CourseReviewApp.sln solution file
      2. Right-click the solution name in Solution Explorer and select the Restore Client-Side Libraries option.
      3. Build the solution
      4. Open Package Manager Console, set default project to the CourseReviewApp.DAL and run the following command:
   
                update-database          

   * For .NET CLI
        1. Open a command prompt in the root folder of the solution and execute the following commands:
   
                dotnet build
                dotnet tool install -g Microsoft.Web.LibraryManager.Cli
                cd CourseReviewApp.Web && libman restore && cd ..
                dotnet tool install --global dotnet-ef
                dotnet ef database update --project .\CourseReviewApp.DAL\CourseReviewApp.DAL.csproj --startup-project .\CourseReviewApp.Web\CourseReviewApp.Web.csproj
3. Provide credentials for a email account that will be used by the email sender service.
   
   To do this please open the appsettings.json file from the CourseReviewApp.Web project and replace **{username}** and **{password}** placeholders with your email account credentials.
   
   You can use Gmail account but don't forget for [enabling less secure apps to access Gmail](https://www.youtube.com/watch?v=Ee7PDsbfOUI). The port and provider setting in the appsettings.json file is already set to Gmail.
4. Insert sample data to the database by executing the script from the **sample_data.sql** file from the project root directory.

   You can execute script in the SSMS or the Azure Data Studio.
   Be sure that you are connected to the **(LocalDB)\MSSQLLocalDB** server.
   
5. Run the application
   * For Visual Studio press CTRL + F5 shortcut
   * For .NET CLI execute the following command:
  
            dotnet run --project ./CourseReviewApp.Web/CourseReviewApp.Web.csproj
   
      URL of hosted application will be shown in the command prompt.

## Running application

