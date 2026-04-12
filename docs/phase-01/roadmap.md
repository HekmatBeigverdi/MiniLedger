# Phase 1 Roadmap

## Overview

Phase 1 is organized into 7 learning days.

Each day focuses on one major step of building the first working version of MiniLedger.

---

## Day 1 - Project Setup

### Goals
- create the solution
- create the Web API project
- prepare the folder structure
- remove template files
- make sure the project runs

### Outcome
A clean project foundation ready for development.

---

## Day 2 - Domain Modeling

### Goals
- add enums
- add domain entities
- define relationships between models

### Outcome
The accounting domain is represented in code.

---

## Day 3 - EF Core Setup

### Goals
- add DbContext
- configure entity mappings
- configure database provider
- add initial migration

### Outcome
The project can store and retrieve data from the database.

---

## Day 4 - Account Module

### Goals
- add Account DTOs
- implement Account repository
- implement Account service
- implement Accounts controller

### Outcome
The API supports account management.

---

## Day 5 - Party Module

### Goals
- add Party DTOs
- implement Party repository
- implement Party service
- implement Parties controller

### Outcome
The API supports party management.

---

## Day 6 - Journal Entry Module

### Goals
- add Journal Entry DTOs
- implement Journal Entry service
- implement Journal Entries controller
- apply accounting business rules

### Outcome
The API supports journal entry creation and retrieval.

---

## Day 7 - Middleware and Cleanup

### Goals
- add global exception handling middleware
- add request timing middleware
- verify project build and run
- finalize documentation

### Outcome
The API becomes cleaner, safer, and easier to debug.

---

## Final Deliverables of Phase 1

- project structure completed
- entities and relationships implemented
- EF Core configured
- Accounts module completed
- Parties module completed
- Journal Entry module completed
- middleware added
- Swagger working
- GitHub documentation completed