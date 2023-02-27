FROM mcr.microsoft.com/dotnet/sdk:6.0  AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Restore npm package
# REF by https://stackoverflow.com/questions/45880460/enable-docker-support-for-angular-project
# REF by http://kevintsengtw.blogspot.com/2018/08/aspnet-core-21-docker-image-nodejs.html
COPY package*.json ./
# COPY .bowerrc bower.json ./
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y build-essential nodejs
RUN npm install -s --ignore-scripts
# RUN npm install -g gulp bower
# RUN bower install --allow-root

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /dotnetapp

COPY --from=build /app/out .
# COPY --from=build /app/wwwroot/lib ./wwwroot/lib

# RUN mv -n wwwroot/* .
# RUN rm -rf wwwroot/

ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "NetCoreSample.dll"]

# podman run -d -p 5000:5000 --name demo localhost/netcoresample:latest