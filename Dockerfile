FROM node:22-alpine AS frontend
RUN apk add --no-cache git
WORKDIR /frontend
ARG FRONTEND_REF=22d1855582c8e81b94a4bd600286ab46f6c65546
RUN git init && git remote add origin https://github.com/kinemo-cinema/kinemo-cinema-frontend.git && git fetch --depth=1 origin ${FRONTEND_REF} && git checkout FETCH_HEAD
RUN npm ci && npm run build

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json ./
COPY KinemoPlatform.API/KinemoPlatform.API.csproj KinemoPlatform.API/
RUN dotnet restore KinemoPlatform.API/KinemoPlatform.API.csproj
COPY KinemoPlatform.API/ KinemoPlatform.API/
COPY --from=frontend /frontend/dist/ KinemoPlatform.API/wwwroot/
RUN dotnet publish KinemoPlatform.API/KinemoPlatform.API.csproj -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish/ ./
RUN mkdir -p storage/sensory-files && chown -R app:app storage
USER app
EXPOSE 10000
ENTRYPOINT ["sh", "-c", "exec dotnet KinemoPlatform.API.dll --urls http://0.0.0.0:${PORT:-10000}"]
