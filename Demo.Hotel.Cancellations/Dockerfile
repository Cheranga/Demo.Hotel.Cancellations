#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src
COPY . .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /cancellationsapi

FROM base as final
WORKDIR /cancellationsapi
COPY --from=build /cancellationsapi .
ENTRYPOINT ["dotnet", "Demo.Hotel.Cancellations.dll"]