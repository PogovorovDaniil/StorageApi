dotnet ef migrations add InitialCreate --context AuthorizationContext
dotnet ef database update InitialCreate --context AuthorizationContext

dotnet ef migrations add InitialCreate --context StorageContext
dotnet ef database update InitialCreate --context StorageContext