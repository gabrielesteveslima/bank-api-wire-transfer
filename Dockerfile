### Stage: Run Continuous Integration procedures (SonarQube)
FROM example.com.br/base-images/dotnet-sdk-3.1:latest AS ci
# Arguments
ARG sonar_key
ARG sonar_name
ARG label_version
ARG sonar_url
ARG sonar_token
# Set the workdir for the application
WORKDIR /app
# Copy the repository files into the workdir
COPY . /app
# Run the SonarQube scanner
RUN dotnet restore
#RUN dotnet sonarscanner begin /k:"$sonar_key" /n:"$sonar_name" /v:"$label_version" /d:sonar.host.url="$sonar_url" /d:sonar.login="$sonar_token" /d:sonar.cs.opencover.reportsPaths=/app/coverage.opencover.xml
#RUN dotnet test
#RUN dotnet sonarscanner end /d:sonar.login="$sonar_token"
### Stage: Build the production files
FROM example.com.br/base-images/dotnet-sdk-3.1:latest AS build
WORKDIR /app
# Copy csproj and restore
COPY . /app
RUN dotnet restore
RUN dotnet publish Wire.Transfer.In.API -c Release -o Wire.Transfer.In.API/out/
# Change release artifacts owner permissions
RUN chown -R app:app .
### Stage: Copy the production artifacts to a runtime image
FROM example.com.br/base-images/dotnet-runtime-3.1:latest AS runtime
USER app
WORKDIR /app
# Copy the production artifacts to the workdir
COPY --from=build /app/Wire.Transfer.In.API/out .
# Set the port that the application will run on
ENV ASPNETCORE_URLS=http://+:8080
# Set the command that will run the API
ENTRYPOINT ["dotnet", "Wire.Transfer.In.API.dll"]
# Expose the port that the application will run on
EXPOSE 8080
