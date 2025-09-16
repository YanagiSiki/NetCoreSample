# NetCoreSample

[![Build](https://github.com/YanagiSiki/NetCoreSample/actions/workflows/dotnet-core.yml/badge.svg)](https://github.com/YanagiSiki/NetCoreSample/actions/workflows/dotnet-core.yml)

- 當初用來練功的專案，簡單來說就是從頭寫一個部落格。N百年沒有更新程式碼 or 內容了 ~~對，我就懶(x~~
- Demo可以看[這裡](https://netcoresample.onrender.com/)。因為是免費仔，render啟動網站時需要一點時間，請耐心等待。
- DB是架在免費的oracle機器上(2vcpu + 1G ram)。資源很少，mariadb偶爾會當機，有壞掉可以開issue給我。~~(但我不一定會看，看到也不一定會處理)~~


## SDK

download [SDK 6.0.400](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.400-windows-x64-installer)

很想找時間升上dotnet 10，無奈沒時間((躺

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

