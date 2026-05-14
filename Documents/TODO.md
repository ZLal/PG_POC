
## TODO


## Done

- Create service layer
- Set up the repository pattern
- Implement repository pattern for data access
- Set up the database models (Organization, Application, Gateway etc.)
- Create Entity Framework DbContext
- Implement API endpoints
- Create API controllers for CRUD operations
- Configure database migrations

## Used prompts
- dotnet new webapi -n PaymentGatewayPOC --no-https
- Convert project from using minimal API to old style controller structure.
- Modify this document to use SQLite instead of MSSQL database
- Set up the database models
- Clients model is missing in db context. Create model and update db context. Reanalize the technical requirement document for this.
- Implement repository pattern for data access
- Create service layer with logging
- Create API controllers for services
- Add migration for database
- Write code to run migration on application start
- Create enum GatewayStatus with values Active, InActive and Disabled. Update Gateway model to use it.
- In GatewaysController modify Create and Update models to use GatewayStatus enum
