# NetCoreSample

[![Build](https://github.com/YanagiSiki/NetCoreSample/actions/workflows/dotnet-core.yml/badge.svg)](https://github.com/YanagiSiki/NetCoreSample/actions/workflows/dotnet-core.yml)

~~This is a simple blog using dotnet core. You can find demo on [Heroku](https://netcoresample.herokuapp.com/).~~

Sadly, heroku announced that it will stop free plans at November 28th, 2022, so I move to azure web app services.

Thanks for heroku all along web hosting.

Now you can find a new Demo (with old content haha) on [Azure](https://yanagisiki.azurewebsites.net/)

## SDK

download [SDK 6.0.400](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.400-windows-x64-installer)

## Database restore

1. create new file `DBconnection.json`
2. set DB connection, for example

   ```json
   {
     "HerokuNpg": "User ID=userID;Password=password;Host=127.0.0.1;Port=5432;Database=mypostgre;Pooling=true;SslMode=Require;Trust Server Certificate=true"
   }
   ```

3. run `dotnet ef migrations add InitialCreate -o ../MultipleDbContexts/ --context HerokuNpgContext`
4. run `dotnet ef database update --context HerokuNpgContext`

## frontend components restore

`npm install`

## Run

`dotnet run`

## build as docker image

`docker build`

## Using fowllowing components and techs

- FrontEnd
  - jQuery
  - semantic UI
  - bootstrap
  - typehead.js
  - highlight.js
  - simplemde.js
  - anchor-js
  - github-markdown-css
  - typescript
- BackEnd
  - C# (.net core)
  - entity framework
  - linq
- Database
  - mariadb
  - postgresql
- Deployment
  - Heroku
