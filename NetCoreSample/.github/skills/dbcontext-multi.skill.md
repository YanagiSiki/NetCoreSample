## Skill: Multi-DbContext Pattern

- Support multiple database backends by defining separate DbContext classes in DBContext/.
- Examples: HerokuNpgContext (PostgreSQL), MySQLContext, OracleMySQLContext, UnitTestContext.
- Register the appropriate DbContext in Startup.cs based on configuration.
- Use dependency injection to inject the correct context into controllers/services.
- Keep connection strings in DBconnection.json or appsettings.json.
