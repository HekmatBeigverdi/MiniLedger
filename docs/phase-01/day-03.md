# Day 3 - EF Core Setup

## Goal

Connect domain models to the database.

---

## Steps

### 1. Create DbContext
MiniLedgerDbContext

### 2. Add DbSets
- Accounts
- Parties
- JournalEntries
- JournalEntryLines

### 3. Configure Relationships (Fluent API)

### 4. Configure MySQL in Program.cs

---

## Migration

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
