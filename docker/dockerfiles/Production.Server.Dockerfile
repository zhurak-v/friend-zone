FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY publisher/ .

EXPOSE 7777

ENTRYPOINT ["dotnet", "Application.dll"]
