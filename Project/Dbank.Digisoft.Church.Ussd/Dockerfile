#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Dbank.Digisoft.Church.Ussd.csproj", ""]
RUN dotnet restore "./GreatIdeas.Ussd.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Dbank.Digisoft.Church.Ussd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dbank.Digisoft.Church.Ussd.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dbank.Digisoft.Church.Ussd.dll"]