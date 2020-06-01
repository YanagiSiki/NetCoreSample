# NetCoreSample

This is a simple blog using dotnet core.

## SDK

download [SDK 2.1.802](https://dotnet.microsoft.com/download/dotnet-core/2.1)

## Database restore

1. create new file `DBconnection.json`
2. set DB connection, for example

    ``` json
    {
        "HerokuNpg":"User ID=userID;Password=password;Host=127.0.0.1;Port=5432;Database=mypostgre;Pooling=true;SslMode=Require; UseSSLStream=true;Trust Server Certificate=true",
    }
    ```

3. run `dotnet ef migrations add InitialCreate -o ../MultipleDbContexts/ --context HerokuNpgContext`
4. run `dotnet ef database update --context HerokuNpgContext`

## Run

`dotnet run`

## build as docker image

`docker build`
