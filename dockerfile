FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build-env
WORKDIR /app
RUN apk add --update nodejs npm

# Copy csproj and restore as distinct layers
COPY StepChallenge/StepChallenge.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish StepChallenge/StepChallenge.csproj -c Release -o ../out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-alpine
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "StepChallenge.dll"]
