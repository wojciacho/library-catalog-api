# --- Build stage: full SDK, used only to compile and publish ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy just the project file first so `dotnet restore` is cached
# separately from the rest of the source code.
COPY LibraryCatalog.csproj .
RUN dotnet restore LibraryCatalog.csproj

# Now copy everything else and publish.
COPY . .
RUN dotnet publish LibraryCatalog.csproj -c Release -o /app/publish --no-restore

# --- Final stage: small runtime image, no build tools ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "LibraryCatalog.dll"]
