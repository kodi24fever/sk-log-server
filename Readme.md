# Welcome to Setting Up .NET Server!

  

Shark Valley Intro blah blah

  
  

## Required Software

  

`dotnet sdk version >= 7.x` [Download](https://dotnet.microsoft.com/en-us/download)

  

A Text Editor of your choice (preferably Visual Studio Code):

- [Visual Studio Code](https://code.visualstudio.com/download)

  

After Installing dotnet open command prompt and type:

`dotnet --version`

  

Result should be something like this:

`7.x.x`



## Important Packages

1. [Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio)

  

## Installation

  

1. After installing dotnet sdk, travel to root folder in the SharkValleyServer

2. Use the command `dotnet watch` to start the server.

3. If not automatically redirected go to `http://localhost:5043/`



## Install Trust Certificate
`dotnet dev-certs https --trust`



## Installing SQL Server Express

  

1. Go to [link](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) and download SQL Express Server (free version of SQL Server).

2. Open installer in downloads folder.

3. After installing SQL Server Express, download and install [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16&redirectedfrom=MSDN)

## Setting Up SQL Server Express

  In order to get the app working we need to create a new database and user with owner privileges to the database.

### Creating the database using SSMS

1. Open SSMS and connect using default authentication (Windows Authentication)

2. Right click Databases folder and add New Databases.


In order to restore the database from the Backup.sql file, you have to right click the database and click New Query. Then, copy the content from the Backup.sql file and paste it inside the query created in SSMS. Then, execute the query. All tables should be created in the database.

### Creating a new user with owner privileges

1. Expand Security folder at root file system and right click Logins folder to add New Login.
2. Write a login name and select SQL server authentication.
3. Write your new password and confirm it.
4. Unselect Enforce password expiration.
5. Click User Mapping on left side "Select a page" section.
6. Select the database name you created and select db_owner option, then click OK.

Right now we created a new user that has owner privileges in the database that we created before.

**Note:** You can check the new user if you enter to Security folder and Users in your newly created database.

### Changing Server Authentication to Both Windows and SQL Authentication

1. Right click on your server and select Properties.
2. Select Security and then select SQL Server and Windows Authentication Mode.
3. Click OK.

In order to apply this changes we have to restart the server.

### Restarting the SQL Server

1. Open SQL Server Configuration Console using Windows search bar.
2. Select SQL Server Services.
3. Right click SQL Server (running) and click restart.

So far, you should be able to login to the server with the new user using SQL Authentication Mode.


### Connecting String
`"Server=SERVER_NAME;Database=tDB_NAME;User Id=NEW_USER;Password=PASSWORD;Encrypt=False"`

### How admin users work

Basically after a new user registers. They have to verify their email. After main admin can give priviliges to edit content, and ti shoudl work the new account.


### Command to run API
`dotnet run --launch-profile https`
