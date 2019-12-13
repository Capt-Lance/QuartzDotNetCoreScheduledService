FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore ScheduledJobTest

# Copy everything else and build
COPY . ./
RUN dotnet publish ScheduledJobTest -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "scheduledjobtest.dll"]