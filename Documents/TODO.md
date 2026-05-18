
## TODO
Update TransactionService model to use LastUpdatedDate mapping
Add cancelled state to transaction status
Change clientid in application model from string to guid

## Done

- Create service layer
- Set up the repository pattern
- Implement repository pattern for data access
- Set up the database models (Organization, Application, Gateway etc.)
- Create Entity Framework DbContext
- Implement API endpoints
- Create API controllers for CRUD operations
- Configure database migrations
- Add last updated date to transaction model

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
- Update Migration for changing Gateway status from string to enum
- TransactionStatus
  - Create enum TransactionStatus with values Pending, InPayment, Paid, Refunded, Failed & Error. Change Status in Models\Transaction.cs model from string to TransactionStatus. Update Migration and api models in controller accordingly.
  - Instead of creating new migration modify existing once
  - Update existing migration to int to store enum instead of string
  - Change 20260511121933_InitialCreate.Designer.cs to use int for Status in Transaction
- Transaction model modification
  - Add LastUpdatedDate to transaction model. Also update existing migration accordingly. Don't create new migration.
  - Update changes for LastUpdatedDate in Transaction in designer file
- dotnet new blazor -n BlazorUITemplate
