# Day 6 - Journal Entry Module

## Goal

Implement Journal Entry with business rules.

---

## Components

### DTOs
- JournalEntryCreateDto
- JournalEntryLineCreateDto
- JournalEntryDto

### Service
- IJournalEntryService
- JournalEntryService

### Controller
- JournalEntriesController

---

## Business Rules

- Must have at least one line
- Debit = Credit
- Only debit or credit per line
- Account must exist
- Party must exist if provided
- Some accounts require Party

---

## Endpoints

- GET /api/journalentries
- GET /api/journalentries/{id}
- POST /api/journalentries

---

## Result

Real accounting logic implemented
