FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

RUN curl -L https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh  | sh

# Copy csproj and restore as distinct layers
COPY /src/Jex.Api/ ./
COPY NuGet.config ./
ARG FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/Jexhq/_packaging/Components/nuget/v3/index.json\", \"username\":\"docker\", \"password\":\"b26cuthe4by442r3fuegkmmkgxgzcftv3hjotzb5mjsdhy7sxspa\"}]}"
RUN dotnet restore

# Copy and publish app and libraries
COPY . .
RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime
WORKDIR /app/
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Jex.Api.dll"]