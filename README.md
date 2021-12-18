# broker

To generate test report run following commands
1. dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=.\TestResults\Coverage\
2. reportgenerator -reports:".\**\TestResults\coverage\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
