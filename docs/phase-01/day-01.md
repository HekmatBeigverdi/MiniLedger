# Day 1 - Project Setup

## Goal

Create the base structure of the project.

---

## Steps

### 1. Create Solution
```bash
dotnet new sln -n MiniLedger

### 2. Create API Project
```bash
dotnet new webapi -n MiniLedger.Api -o src/MiniLedger.Api

### 3. Add Project to Solution
```bash
dotnet sln MiniLedger.sln add src/MiniLedger.Api/MiniLedger.Api.csproj

### 4. Build Project
```bash
dotnet build MiniLedger.sln

### 5. Run Project
```bash
dotnet run --project src/MiniLedger.Api


Clean Template

Delete:

WeatherForecast.cs
WeatherForecastController.cs


Create Folder Structure
Common/
Controllers/
Data/
DTOs/
Middleware/
Models/
Repositories/
Services/

Result
Project runs successfully
Swagger is available
Clean structure is ready