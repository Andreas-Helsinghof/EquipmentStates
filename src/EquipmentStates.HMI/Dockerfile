FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src
COPY . .
# Expose both common ports for HMI instances
EXPOSE 5001
EXPOSE 5002
CMD ["dotnet", "run", "--project", "EquipmentStates.HMI.csproj"]
