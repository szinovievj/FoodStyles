FROM mcr.microsoft.com/dotnet/core/sdk AS build
#ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /project
# copy csproj and restore as distinct layers
COPY ./*.sln .
COPY FoodStyles/*.csproj ./FoodStyles/
RUN dotnet restore
#
# copy everything else and build app
COPY FoodStyles/. ./FoodStyles/
WORKDIR /project/FoodStyles
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /project

# Boot.sh
COPY wait-for-it.sh /usr/local/bin/wait-for-it.sh
RUN chmod +x /usr/local/bin/wait-for-it.sh
#ENV ASPNETCORE_URLS=http://+:5000
#EXPOSE 5000
COPY --from=build /project/FoodStyles/out ./
#ENTRYPOINT ["dotnet", "TestDockerApp.dll"]