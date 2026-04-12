# Phase 1 - Web API Fundamentals

## Goal

Build the first working version of **MiniLedger** as a clean ASP.NET Core Web API project.

Phase 1 focuses on the technical foundations of the system:
- project structure
- domain modeling
- Entity Framework Core setup
- CRUD endpoints
- DTOs
- services and repositories
- middleware
- Swagger documentation

---

## Why This Phase Matters

This phase creates the base of the entire project.

Without a solid Phase 1, the later phases such as authentication, clean architecture, CQRS, testing, and performance optimization will become harder to implement and harder to explain.

The purpose of this phase is to move from an empty repository to a structured backend project with real accounting concepts.

---

## Main Topics Covered

- ASP.NET Core Web API project structure
- Domain entities and relationships
- DTO design
- Repository pattern
- Service layer
- EF Core configuration
- Validation using data annotations
- Global exception handling
- Basic logging
- Swagger UI

---

## Modules Included

### Accounts
Used to represent accounting accounts such as:
- Cash
- Bank
- Revenue
- Expense

### Parties
Used to represent customers, suppliers, and other related entities.

### Journal Entries
Used to record accounting transactions.

### Journal Entry Lines
Used to define debit and credit lines for each journal entry.

---

## Expected Outcome

At the end of Phase 1, the project should provide:

- a working ASP.NET Core Web API
- database persistence with EF Core
- CRUD for Accounts
- CRUD for Parties
- Journal Entry creation and retrieval
- accounting validation rules
- middleware-based exception handling
- basic logging
- Swagger-based API exploration

---

## Endpoints Expected by the End of Phase 1

### Accounts
- `GET /api/accounts`
- `GET /api/accounts/{id}`
- `POST /api/accounts`
- `PUT /api/accounts/{id}`
- `DELETE /api/accounts/{id}`

### Parties
- `GET /api/parties`
- `GET /api/parties/{id}`
- `POST /api/parties`
- `PUT /api/parties/{id}`
- `DELETE /api/parties/{id}`

### Journal Entries
- `GET /api/journalentries`
- `GET /api/journalentries/{id}`
- `POST /api/journalentries`

---

## Business Rules Introduced in Phase 1

The Journal Entry module includes a few important accounting rules:

1. A journal entry must have at least one line.
2. Total debit must equal total credit.
3. Each line must contain either debit or credit, not both.
4. Accounts must exist before they can be used in journal entry lines.
5. Parties must exist if referenced.
6. If an account requires a party, `PartyId` must be provided.
7. `EntryNumber` must be unique.

---

## Result

Phase 1 transforms MiniLedger from a repository skeleton into a functional backend learning project.