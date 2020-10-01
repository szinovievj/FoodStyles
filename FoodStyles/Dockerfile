FROM mcr.microsoft.com/dotnet/core/sdk AS build
#ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /project
# copy csproj and restore as distinct layers
COPY ./*.sln .
COPY PixelStreaming.AWS/*.csproj ./PixelStreaming.AWS/
COPY PixelStreaming.Core/*.csproj ./PixelStreaming.Core/
COPY PixelStreaming.Services/*.csproj ./PixelStreaming.Services/
COPY PixelStreaming.SSH/*.csproj ./PixelStreaming.SSH/
COPY PixelStreaming.SignalR/*.csproj ./PixelStreaming.SignalR/
COPY PixelStreaming.Data.EF.Services/*.csproj ./PixelStreaming.Data.EF.Services/
COPY PixelStreaming.Data.EF/*.csproj ./PixelStreaming.Data.EF/
COPY PixelStreaming.Web/*.csproj ./PixelStreaming.Web/
RUN dotnet restore
#
# copy everything else and build app
COPY PixelStreaming.AWS/. ./PixelStreaming.AWS/
COPY PixelStreaming.Core/. ./PixelStreaming.Core/
COPY PixelStreaming.Services/. ./PixelStreaming.Services/
COPY PixelStreaming.SSH/. ./PixelStreaming.SSH/
COPY PixelStreaming.SignalR/. ./PixelStreaming.SignalR/
COPY PixelStreaming.Data.EF.Services/. ./PixelStreaming.Data.EF.Services/
COPY PixelStreaming.Data.EF/. ./PixelStreaming.Data.EF/
COPY PixelStreaming.Web/. ./PixelStreaming.Web/
WORKDIR /project/PixelStreaming.Web
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /project

# Boot.sh
COPY wait-for-it.sh /usr/local/bin/wait-for-it.sh
RUN chmod +x /usr/local/bin/wait-for-it.sh
#ENV ASPNETCORE_URLS=http://+:5000
#EXPOSE 5000
COPY --from=build /project/PixelStreaming.Web/out ./
#ENTRYPOINT ["dotnet", "TestDockerApp.dll"]