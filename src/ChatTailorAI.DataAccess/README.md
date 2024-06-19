# ChatTailor.DataAccess

This project contains the data access layer for the ChatTailor project. It is a .NET Standard 2.0 library.

## Getting Started

### Running migrations from this directory

```dotnet ef migrations add Initial -s ../ChatTailorAI.Migrations```

#### Running Specific Migrations
`dotnet ef migrations add Initial -s ../ChatTailorAI.Migrations --context SQLiteDb --output-dir Database/Migrations/SQLite`

### Running from the root directory
`dotnet ef migrations add InitialCreate --context ChatTailorAI.DataAccess.Database.SQLite.SQLiteDb --startup-project ../ChatTailorAI.Migrations --output-dir Database/Migrations/SQLite`