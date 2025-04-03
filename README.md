# theWritersRunway
Merging writing with fashion, where books and fashion shows meet.




<!-- 
  dotnet ef migrations add InitialCreate
  dotnet ef database update
 -->


 <!-- 
  1. change the sqllite to sqlserver in program.cs
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

  2. update the appsetting default connection to
  "Data Source=AdilBooks.db"
 
  3. add package for sqllite to .csproj
  <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.8"/>

  4. dotnet ef migrations add InitialCreate
  dotnet ef database update

  5. dotnet run
  -->