# Technical Requirement Document (TRD) – POC Version  
**Project**: Payment Gateway Management System (POC)  
**Audience**: Developers & Architects  
**Purpose**: To define technical specifications, architecture, and implementation details for the proof-of-concept system.  

---

## 1. Technology Stack  
- **Backend Framework**: .NET Core (latest LTS version)  
- **Language**: C#  
- **Database**: SQLite  
- **ORM**: Entity Framework Core  
- **Frontend**: ASP.NET Core MVC / Razor Pages (minimal UI)  
- **API Layer**: RESTful APIs using ASP.NET Core Web API  
- **Authentication**: Basic Authentication (POC only, non-secure)  
- **Testing Frameworks**: xUnit / NUnit for unit testing, Postman for API testing  
- **Deployment**: Local IIS / Kestrel server (POC environment)  

---

## 2. System Architecture  
- **Presentation Layer**  
  - Minimal UI for dashboards, reporting, and CRUD management  
  - Razor Pages or MVC Views  

- **Application Layer**  
  - Business logic for gateway, organization, and application management  
  - Service classes for CRUD operations  

- **Data Access Layer**  
  - Entity Framework Core for ORM  
  - Repository pattern for database operations  

- **Database Layer**  
  - SQLite database with tables for Organizations, Applications, Gateways, Transactions, and Logs  

---

## 3. Database Schema (Simplified)  

### Tables  

**Organizations**  
- OrganizationId (PK, GUID)  
- Name (TEXT)  
- CreatedDate (DATETIME)  

**Applications**  
- ApplicationId (PK, GUID)  
- OrganizationId (FK)  
- ClientId (TEXT)  
- AccessLocation (TEXT)  
- CreatedDate (DATETIME)  

**Clients**  
- ClientId (PK, GUID)  
- OrganizationId (FK)  
- ApplicationId (FK)
- AccessToken (TEXT)  
- SecretToken (TEXT)  
- CreatedDate (DATETIME)  
- ExpiryDate (DATETIME)  

**Gateways**  
- GatewayId (PK, GUID)  
- Name (TEXT)  
- Status (TEXT)  

**Transactions**  
- TransactionId (PK, GUID)  
- ApplicationId (FK)
- GatewayId (FK)
- Amount (REAL)  
- Status (TEXT)  
- CreatedDate (DATETIME)  

**TransactionDetails**  
- TransactionDetailId (PK, GUID)  
- TransactionId (FK)  
- Status (TEXT)  
- Message (TEXT)  
- Data (TEXT, nullable)  
- CreatedDate (DATETIME)  

---

## 4. API Specifications  

**Base URL**: `/api/v1/`  

### Endpoints  

**Gateway Management**  
- `GET /gateways` → List gateways  
- `GET /gateways/{id}` → View gateway details  
- `POST /gateways` → Add dummy gateway  
- `PUT /gateways/{id}` → Update gateway status  

**Organization Management**  
- `GET /organizations`  
- `POST /organizations`  
- `PUT /organizations/{id}`  
- `DELETE /organizations/{id}`  

**Application Management**  
- `GET /applications`  
- `POST /applications`  
- `PUT /applications/{id}`  
- `DELETE /applications/{id}`  

**Reporting**  
- `GET /reports/transactions` → Transaction summaries  
- `GET /reports/errors` → Error logs  
- `GET /reports/usage` → Organization/application usage  

**Webhook Simulation**  
- `POST /webhooks/notify` → Simulated notification for transaction events  

---

## 5. User Roles & Access Control  

**Admins**  
- Full CRUD access to organizations, applications, and gateways  
- Manage client IDs and access locations  

**Managers**  
- CRUD access limited to applications  
- Read-only access to reports and gateway status  

*(Role-based authorization implemented via ASP.NET Core Identity or custom role middleware – simplified for POC)*  

---

## 6. Testing & Sandbox  
- **Unit Tests**: xUnit/NUnit for service and repository layers  
- **API Tests**: Postman collections for CRUD and reporting endpoints  
- **UI Tests**: Selenium (optional, minimal)  
- **Sandbox**: Dummy gateway with simulated transactions and error events  

---

## 7. Reporting Module  
- **Transaction Summaries**: Aggregated by application and organization  
- **Error Reports**: Logged errors with timestamps  
- **Usage Statistics**: Number of applications per organization, transaction counts  

---

## 8. Acceptance Criteria (Technical)  
- REST APIs functional with CRUD operations  
- Entity Framework Core successfully maps SQLite schema  
- Admin role can manage organizations, applications, and gateways  
- Manager role restricted to application CRUD and reporting  
- Dummy gateway available for sandbox testing  
- Automated tests executed successfully  
