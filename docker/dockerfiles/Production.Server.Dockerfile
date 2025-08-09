FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY publisher/ .
COPY configs ./configs

EXPOSE 7777

ENV ASPNETCORE_ENVIRONMENT=production
ENV ASPNETCORE_URLS=http://+:7777

ENTRYPOINT ["dotnet", "Application.dll"]
