FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OnlineCoursesApp.Monolith.WebApi/OnlineCoursesApp.Monolith.WebApi.csproj", "OnlineCoursesApp.Monolith.WebApi/"]
COPY ["OnlineCoursesApp.Monolith.Application/OnlineCoursesApp.Monolith.Application.csproj", "OnlineCoursesApp.Monolith.Application/"]
COPY ["OnlineCoursesApp.Monolith.Domain/OnlineCoursesApp.Monolith.Domain.csproj", "OnlineCoursesApp.Monolith.Domain/"]
COPY ["OnlineCoursesApp.Monolith.Infrastructure/OnlineCoursesApp.Monolith.Infrastructure.csproj", "OnlineCoursesApp.Monolith.Infrastructure/"]

RUN dotnet restore "OnlineCoursesApp.Monolith.WebApi/OnlineCoursesApp.Monolith.WebApi.csproj"

COPY . .
WORKDIR "/src/OnlineCoursesApp.Monolith.WebApi"
RUN dotnet publish "OnlineCoursesApp.Monolith.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "OnlineCoursesApp.Monolith.WebApi.dll"]