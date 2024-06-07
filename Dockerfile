FROM mcr.microsoft.com/devcontainers/dotnet:1-8.0-bookworm AS development
ARG BUILDKIT_INLINE_CACHE=0

ENV DOTNET_ENVIRONMENT=Development
COPY .devcontainer/entrypoint.sh /usr/local/bin/entrypoint.sh
RUN chmod +x /usr/local/bin/entrypoint.sh

USER vscode

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH
WORKDIR /source

COPY MachinePark.UI/*.csproj .
RUN dotnet restore -a $TARGETARCH

COPY . .
RUN dotnet publish -c Release --no-restore -a $TARGETARCH -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS publish
ENV DOTNET_ENVIRONMENT=Production
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
# Uncomment to enable non-root user
# USER $APP_UID
ENTRYPOINT ["dotnet", "Tournaments.API.dll"]