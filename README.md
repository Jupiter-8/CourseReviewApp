# CourseReviewApp

A simple application for reviewing online courses.

## Description

Application is built according to the MPA approach. You can register a new account with the user type "Course owner" or "Course client".
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

The main page of the application:

![obraz](https://user-images.githubusercontent.com/77903617/161860165-68afbe64-4867-4332-b1c9-924f0a768177.png)

The first carousel presents last added reviews. Below there are two carousels for presenting last added courses and best rated courses.
In each carousel you can navigate to course by clicking its title.

![obraz](https://user-images.githubusercontent.com/77903617/162018098-496a9564-20de-47ec-94e7-e28e470a2d06.png)


Some users accounts you can log in and test features available for each role:

| Username    | Role         |
| ----------- | -----------  |
| user1       | Course Owner |
| user2       | Course Client|
| user3       | Admin        |
| user4       | Course Owner, Moderator |

Password for each user is Pass1234;

Section with course list. You can view courses from a selected main category or a subcategory. There is a option for sorting
results and filtering by a name. You can navigate to a course's description site by clicking its title. The list displays 10 results, at the bottom
of the list there is a pagination menu.

![obraz](https://user-images.githubusercontent.com/77903617/162021389-de9d291b-e1ce-4a2c-8a28-83259f52e80d.png)

Course details:

At the top there is a navigation bar from where you can navigate to the viewed course's main category or subcategory.
Under the title there is a link for the course's website.

![obraz](https://user-images.githubusercontent.com/77903617/159189307-6cd8b688-a13f-4b38-8c4c-ecdde5ff4f38.png)

You can add a course to the list of observed courses.

![obraz](https://user-images.githubusercontent.com/77903617/162035423-5b3acaec-0c95-4a4e-a066-425f4f67a550.png)

A message after adding a course to the list of observed courses. A notification will be send to a user's email.
 
![obraz](https://user-images.githubusercontent.com/77903617/162035883-d428264f-6a48-452b-b229-497f089a683e.png)


Reviews section:

The currently logged user is an author of the first review on the list. You can edit or delete your review. 
You can also vote or unvote that other user's review is helpfull. There are options for sorting and filtering results on the list.

![obraz](https://user-images.githubusercontent.com/77903617/159189219-63ae2de0-4819-44b5-b284-dc1815018c7d.png)

You can report other user's review for a inappropriate content. By default, review list presents 5 newest results.
You can load next 5 results by clicking the button at the bottom of the list.

![obraz](https://user-images.githubusercontent.com/77903617/159189356-33ad05b1-37a7-4bd1-b83c-7eaf1a5760e1.png)

As an course's author you can add comments to its reviews, you can also edit or remove your existing comment and
report a review for a inappropriate content.

![obraz](https://user-images.githubusercontent.com/77903617/162033958-504f0cf9-f36e-4fa0-95c7-17dcfe8199a4.png)

A form for adding a new review:

![obraz](https://user-images.githubusercontent.com/77903617/162034858-3d8529de-6aab-4c5f-af59-f5c74216f32e.png)

Course management panel for the admin user:

![obraz](https://user-images.githubusercontent.com/77903617/159189469-8c9f5b7e-9f98-4482-9f28-93fa6cc2c5a7.png)

User settings panel:

![obraz](https://user-images.githubusercontent.com/77903617/162036691-0f79701d-f4b7-4be6-a5bd-b7722b1f6bfc.png)

Presented screens don't show all pages and features of the application.
