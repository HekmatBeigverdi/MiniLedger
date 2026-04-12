# Day 2 - Domain Modeling

## Goal

Define the core business entities.

---

## Enums

### AccountType
- Asset
- Liability
- Equity
- Revenue
- Expense

### PartyType
- Customer
- Supplier
- Other

---

## Entities

### Account
- Id
- Code
- Name
- AccountType
- RequiresParty

### Party
- Id
- Name
- PartyType
- PhoneNumber

### JournalEntry
- Id
- EntryNumber
- Date
- Description

### JournalEntryLine
- Id
- JournalEntryId
- AccountId
- PartyId
- Debit
- Credit

---

## Relationships

- JournalEntry → many JournalEntryLines
- Account → many JournalEntryLines
- Party → many JournalEntryLines

---

## Result

- Domain is fully modeled
- Ready for database mapping
