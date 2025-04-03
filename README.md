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

### 📚 Book Section 

#### Functional Updates
- **Linked Designers** now display in the book detail view (`Find.cshtml`)
  - If designers exist, they are listed.
  - If none are linked, a message shows: *"No designers linked to this book."*
- **Designer names in book detail view are now clickable**, routing users to the **Designer Details** page.

#### 🛠️ Technical Changes
- Modified `Find` action in `BooksPageController` to:
  - Include linked designers using EF Core joins (`DesignerBooks`)
  - Populate `LinkedDesigners` in `BookWithAuthorsViewModel`
- View (`Views/Books/Find.cshtml`) was updated to:
  - Loop through `Model.LinkedDesigners`
  - Add an anchor tag that links to `Designers/Details/{id}`



### 👗 Designer Section 

#### Functional Updates
- **Books can now be assigned to designers** during:
  - Designer creation
  - Designer update/edit
- **Designer details page** now shows:
  - List of books designed
  - List of assigned shows
  - Optional "Remove" button for book (when `ShowRemoveButton` is authenticated by admin)
- **Designer Index page** updated to:
  - Show all books linked to each designer

### Technical Changes
- **Bridging Table: `DesignerBook`**
  - Created to represent many-to-many relationship between `Designer` and `Book`
  - Registered in `ApplicationDbContext`
- **DTO Changes:**
  - `DesignerCreateDTO` and `DesignerUpdateDTO` updated to include `SelectedBookIds`
- **Controller Logic:**
  - `Create` and `Edit` methods in `DesignersController` updated to:
    - Handle book assignment via `SelectedBookIds`
    - Save `DesignerBook` entries
- **DesignerDetailsViewModel:**
  - Introduced to support complex view needs (Designer + Shows + Books + Remove logic)
- **Details.cshtml** view for Designer:
  - Displays list of assigned shows and books
  - Handles conditional rendering of remove buttons for books


### Testing & UX

- Currently no authentication required for user (to be implemented next)
- Views are functional for all users
- API is being aligned with controller logic for curl/postman testing


### Shows
- CRUD operations
- Linked to Publishers, Designers, Participants, and Votes

### Participants & Voting
- CRUD for Participants
- Voting logic implemented with unique index on (ParticipantId, DesignerId, ShowId)
- Views to allow voting per show from participant

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

