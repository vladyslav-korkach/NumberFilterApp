FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY NumberFilterApp.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .
COPY entrypoint.sh /app/entrypoint.sh
COPY init.sh /app/init.sh

# Ensure scripts have execute permissions
RUN chmod +x /app/entrypoint.sh /app/init.sh

ENTRYPOINT ["/app/init.sh"]