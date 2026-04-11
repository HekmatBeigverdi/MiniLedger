# Day 1 - Project Setup

## Goals
- Create the solution
- Create the Web API project
- Remove template files
- Prepare the folder structure
- Verify the project builds and runs

## Commands

```bash
dotnet new sln -n MiniLedger
dotnet new webapi -n MiniLedger.Api -o src/MiniLedger.Api
dotnet sln MiniLedger.sln add src/MiniLedger.Api/MiniLedger.Api.csproj
