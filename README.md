# theWritersRunway
Merging writing with fashion, where books and fashion shows meet.

# ReadingRoom & Fashion Voting System

## 📚 Project Overview

This project combines two systems:

- **ReadingRoom**: A publishing and book management system that allows for managing books, authors, publishers, and shows.
- **Fashion Voting System**: A fashion-focused module contributed by Genevieve that allows designers to participate in shows and get votes from participants. This module was integrated with the ReadingRoom project through shared entities and relationships.

---

## 🧱 Technologies Used

- ASP.NET Core MVC (C#)
- Entity Framework Core (EF Core)
- SQL Server
- Swagger (API Testing)
- Razor Views (for UI)
- Identity (User Authentication)

---

## 🔄 Key Features Implemented

### ReadingRoom Domain (Backend by Adil)

#### ✅ Publishers
- CRUD operations (List, Add, Update, Delete)
- View publisher details (including associated books and shows)
- Link/Unlink Shows from a Publisher
- View all linked shows for a Publisher

#### ✅ Books
- CRUD operations
- Associated with Authors and Publishers
- View Books by Publisher

#### ✅ Authors
- CRUD with many-to-many relation to Books

#### ✅ Shows
- CRUD operations
- Linked to Publishers, Designers, Participants, and Votes
- Identity key (`ShowId`) redefined after migration corruption
- Foreign keys re-established after manual fixes

#### ✅ Participants & Voting
- CRUD for Participants
- Voting logic implemented with unique index on (ParticipantId, DesignerId, ShowId)
- Views to allow voting per show and participant

---

## 🎨 Fashion Voting System Module (by Genevieve)

#### ✅ Designers
- CRUD for Designers
- Many-to-many link to Books through `DesignerBooks`
- Book linking/unlinking for each designer
- View Designer's Books
- View all Designers associated with a specific Book (on the Book details page)

#### 🛠️ Technical Details
- `DesignerBooks` junction table with FK to `Designers` and `Books`
- Model configuration defined in `ApplicationDbContext.OnModelCreating`
- Manual FK constraints applied after broken migrations

---

## 🔗 Entity Relationships Summary

- `Publisher ↔ Show` (many-to-many)
- `Book ↔ Author` (many-to-many)
- `Designer ↔ Book` (many-to-many)
- `Designer ↔ Show` (many-to-many)
- `Participant ↔ Show` (many-to-many)
- `Vote`: maps `Participant ↔ Designer ↔ Show`

---

## 🧪 Testing and Swagger

Swagger is enabled for all APIs. During testing:
- Views were temporarily commented out for full Swagger testing
- Link/unlink and listing actions were verified through API and browser

---

## 🧠 Database Management

- Manual database interventions were done when EF migrations failed due to corrupted identity/primary key constraints
- Foreign keys were recreated manually where necessary
- Old data was backed up and inserted back manually with original IDs

---

## 🗂️ Project Structure

- `/Models`: Entity classes (Show, Book, Designer, etc.)
- `/Controllers`: MVC + API controllers
- `/Views`: Razor views for CRUD
- `/Services`: Business logic, clean separation from controllers
- `/Data`: `ApplicationDbContext` with full model mapping

---

## 👥 Contributors

- **Adil**: Core backend developer for ReadingRoom
- **Genevieve**: Designer management and fashion voting system

---

## 🗓️ Last Updated
April 03, 2025

