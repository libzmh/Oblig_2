# Quiz Application

## Requirements
- .NET 8.0 SDK
- Node.js v22.18.0 (installed but not used in this MVC project - will be needed for React frontend in exam project)

## How to start the application

1. Open terminal/command prompt
2. Navigate to project folder (where .csproj file is located)
3. Restore dependencies:
```
   dotnet restore
```
4. Update database:
```
   dotnet ef database update
```
5. Run the application:
```
   dotnet run
```
6. Open browser at: https://localhost:5001 (or the URL shown in terminal)

## Features
- Create, Read, Update, Delete quizzes
- Take quizzes and see results