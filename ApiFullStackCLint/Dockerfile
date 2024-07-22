FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProfessionalBlogAndPortfolio/ProfessionalBlogAndPortfolio.csproj", "ProfessionalBlogAndPortfolio/"]
RUN dotnet restore "ProfessionalBlogAndPortfolio/ProfessionalBlogAndPortfolio.csproj"
COPY . .
WORKDIR "/src/ProfessionalBlogAndPortfolio"
RUN dotnet build "ProfessionalBlogAndPortfolio.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProfessionalBlogAndPortfolio.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProfessionalBlogAndPortfolio.dll"]
