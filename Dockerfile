# Build the existing .NET 10 project from the repository root.
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /src

# Restore first so Docker can cache the NuGet restore layer.
COPY ["chess 2.0/chess 2.0.csproj", "chess 2.0/"]
RUN dotnet restore "chess 2.0/chess 2.0.csproj"

# Copy the rest of the repository and publish the existing console app.
COPY . .
RUN dotnet publish "chess 2.0/chess 2.0.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore

# Runtime image: same Alpine family so the .NET app runs on musl Linux.
FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine AS final

RUN apk add --no-cache ttyd

WORKDIR /app
COPY --from=build /app/publish/ ./
COPY start.sh /start.sh
RUN chmod +x /start.sh

ENV TERM=xterm-256color

# Render supplies PORT. ttyd serves the interactive terminal on that port.
CMD ["sh", "-c", "exec ttyd --port \"${PORT:-10000}\" --writable --max-clients 1 --terminal-type xterm-256color sh /start.sh"]
