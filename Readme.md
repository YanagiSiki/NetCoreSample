# NetCoreSample

![.NET Core](https://github.com/YanagiSiki/NetCoreSample/workflows/.NET%20Core/badge.svg)

This is a simple blog using dotnet core.

## SDK

download [SDK 3.1.10](https://dotnet.microsoft.com/download/dotnet-core/3.1)

## Database restore

1. create new file `DBconnection.json`
2. set DB connection, for example

    ``` json
    {
        "HerokuNpg":"User ID=userID;Password=password;Host=127.0.0.1;Port=5432;Database=mypostgre;Pooling=true;SslMode=Require;Trust Server Certificate=true",
    }
    ```

3. run `dotnet ef migrations add InitialCreate -o ../MultipleDbContexts/ --context HerokuNpgContext`
4. run `dotnet ef database update --context HerokuNpgContext`

## Run

`dotnet run`

## build as docker image

`docker build`

## Demo

Demo on [Heroku](https://netcoresample.herokuapp.com/)
