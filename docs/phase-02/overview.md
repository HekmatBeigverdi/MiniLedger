# Phase 2 - Business Rules and Querying

## Goal

The goal of Phase 2 is to improve MiniLedger by adding more practical querying capabilities and cleaner API response models.

This phase focuses on making the API more realistic and easier to consume.

---

## What Is Added

- common API response models
- paged response model
- query DTOs for Accounts
- query DTOs for Parties
- query DTOs for Journal Entries
- filtering
- sorting
- pagination
- lighter journal entry list model

---

## Why Phase 2 Matters

Phase 1 provides the basic API.

Phase 2 improves usability and moves the project closer to real-world backend behavior.

Instead of returning simple raw lists, the API now supports:

- searching by text
- filtering by business fields
- sorting by common columns
- paging through results
- consistent list responses

---

## Expected Outcome

By the end of Phase 2, MiniLedger should provide a cleaner and more practical API for list endpoints.