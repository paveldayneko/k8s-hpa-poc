FROM microsoft/dotnet:2.2-sdk as build
LABEL stage=intermediate
WORKDIR /src
COPY . .
RUN dotnet publish message.consumer/message.consumer.csproj -c Release -o out

FROM microsoft/dotnet:2.2-runtime as runtime
COPY --from=build /src/message.consumer/out .
ENTRYPOINT ["dotnet", "message.consumer.dll"]