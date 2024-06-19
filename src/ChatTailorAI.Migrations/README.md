# ChatTailorAI.Migrations

This .NET 6.0 console solution serves as an entry point to run EF Core 3.1 migrations with UWP

## How to run migrations
Running from the DataAccess solution folder, run the following command: 
```dotnet ef migrations add Initial -s ../ChatTailorAI.Migrations```

```dotnet ef dbcontext list -s ../ChatTailorAI.Migrations --context SQLiteDb```

dotnet ef database update -s ../ChatTailorAI.Migrations --context SQLiteDb

dotnet ef migrations add Initial -s ../ChatTailorAI.Migrations --context SQLiteDb --output-dir Database/Migrations/SQLite