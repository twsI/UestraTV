# UestraTV

[![Docker Pulls](https://img.shields.io/docker/pulls/twsldev/uestra-fahrgastfernsehen)](https://hub.docker.com/r/twsldev/uestra-fahrgastfernsehen)

A quick attempt at hosting my own Üstra Fahrgastfernsehen with Blazor.

## Motivation

I wanted to test out [blazor](https://github.com/dotnet/blazor) and [BlazorStrap](https://github.com/chanan/BlazorStrap), so I needed a small test project.

## Settings

The only setting is currently the update intervall inside the `appsettings.json` with a default value in `ms` of `3600000`, which means an update every hour.

## Build

```bash
docker build . -f ./UestraTV/Dockerfile -t uestratv:latest
```

## Run

To run the container, you should be familiar with ASP.NET Core, Docker and HTTPS. It's documented quite well [here](https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1), but i tried to paste the important commands in here as well.

Add a certificate:

```bash
dotnet dev-certs https -ep ${HOME}/.aspnet/https/uestratv.pfx -p password123
```

Afterwards you can run the container with the following parameter:

to add an external port

```bash
-p 51806:80 -p 44379:443
```

Then mount the storage volume for certificates

```bash
-v "${HOME}/.aspnet/https:/root/.aspnet/https:ro"
```

And then add [secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1) or simply add the password directly

```bash
-e "ASPNETCORE_Kestrel__Certificates__Default__Password=password123"
```

Just to be safe, you can tell Kestrel, where to find the certificate, in case you use any other folder than the default

```
-e "ASPNETCORE_Kestrel__Certificates__Default__Path=/root/.aspnet/https/uestratv.pfx"
```

Requests should be upgraded, but you can specify listening to port 443 as well

```bash
-e "ASPNETCORE_URLS=https://+:443;http://+:80"
```
