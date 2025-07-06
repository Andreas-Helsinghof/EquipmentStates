FROM mcr.microsoft.com/dotnet/sdk:8.0 
WORKDIR /src
COPY . .
EXPOSE 5003
CMD ["dotnet", "run", "--project", "EquipmentStates.OrdersAPI.csproj", "--urls", "http://0.0.0.0:5003"]
