FROM microsoft/dotnet:2.2-sdk as build
LABEL stage=intermediate
WORKDIR /src
COPY . .
RUN dotnet publish message.producer/message.producer.csproj -c Release -o out

FROM microsoft/dotnet:2.2.0-aspnetcore-runtime as runtime
COPY --from=build /src/message.producer/out .
ENTRYPOINT ["dotnet", "message.producer.dll"]