
# Dovecord

Chat application inspired by Discord made with Blazor, Entity Framework, SignalR with Azure Active Drirectory Authentication.

## Demo

https://user-images.githubusercontent.com/8531546/147357258-adfe0132-3f72-41cd-8b13-37d2646969d7.mp4


## Features

- Account login / registration with Azure Active Directoty
- Ability to Send/edit/delete messages
- Ability to Create/edit/delete channels

## Plannes Features
- Roles (admin, user)
- Ability to send private messages
- Tagging ability
- Notifications
- User profile management (bio, profile picture, nickname)

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
