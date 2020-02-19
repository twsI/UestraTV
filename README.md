# UestraTV

A quick attempt at hosting my own Üstra Fahrgastfernsehn.

## Motivation

I wanted to test out [blazor](https://github.com/dotnet/blazor) and [BlazorStrap](https://github.com/chanan/BlazorStrap), so I needed a small test project.

## Settings

The only setting is currently the update intervall inside the `appsettings.json` with a default value in `ms` of `3600000`, which means an update every hour.

## Build

```bash
docker build . -f ./UestraTV/Dockerfile -t uestratv:latest
```
