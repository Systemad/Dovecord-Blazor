
# Dovecord

Chat application inspired by Discord made with Blazor, Entity Framework, SignalR with Azure Active Drirectory Authentication.

## Proper readme is in heavy progress 

## Features

- Account login / registration
- Ability to Send / Edit / Delete messages
- Ability to Create / Edit / Delete channels

## Plannes Features
- Roles (admin, user)
- Ability to send private messages
- Tagging ability
- Notifications
- User profile management (bio, profile picture, nickname)

## Demo

Insert gif or link to demo

## Run Locally

Clone the project
```bash
  git clone https://github.com/Systemad/Dovecord
```
Go to the project directory
```bash
  cd Dovecord
```
Change Azure details inside and follow this https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-azure-active-directory-b2c?view=aspnetcore-6.0
```bash
Server/appsettings.json
Client/www/appsettings.json
```
Start the server
```bash
  dotnet run
```


## Deployment
