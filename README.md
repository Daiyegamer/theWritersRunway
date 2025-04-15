# ✨ theWritersRunway  
Merging writing with fashion — where books meet fashion shows.
# ✨ theWritersRunway  
Merging writing with fashion — where books meet fashion shows.

---
---

## 📚 Project Overview

This unified platform integrates two major systems:
This unified platform integrates two major systems:

- **ReadingRoom**: Book and publishing management system with authors, publishers, and shows.
- **Fashion Voting System**: A stylish voting module by Genevieve that manages designers, fashion shows, participants, and votes — integrated with ReadingRoom through many-to-many relationships.
- **ReadingRoom**: Book and publishing management system with authors, publishers, and shows.
- **Fashion Voting System**: A stylish voting module by Genevieve that manages designers, fashion shows, participants, and votes — integrated with ReadingRoom through many-to-many relationships.

---

## 🧱 Technologies Used
- ASP.NET Core MVC (C#)
- Entity Framework Core (EF Core)
- SQL Server
- Razor Views
- Swagger for API testing
- Identity for authentication & roles
- Role-based authorization (Admin / Participant)

---

## 🔐 Authentication & Authorization (NEW)
✅ Role-based login system integrated by Genevieve using ASP.NET Identity.  
- `Admin` users can manage content (CRUD, linking, deleting).
- `Participant` users can vote in fashion shows.
- Identity UI enabled, with registration & login pages.

---

## 📸 Image Upload Feature (NEW)
✅ Image upload added for Authors by Adil:  
- Authors can upload profile pictures when created or edited.  
- Images are saved to `/wwwroot/uploads/authors/` with unique file names.  
- Displayed in Find/List views  
- Old images are deleted on update  
- FileService handles save & cleanup  

---

## 🔄 Core Features

### ✅ Publishers
- Full CRUD
- Link/unlink shows
- View linked shows

### ✅ Books
- Full CRUD
- Link/unlink authors
- View by publisher

### ✅ Authors
- Full CRUD with **image support**
- Link/unlink books
- Upload profile picture
- View authors with images

### ✅ Shows
- Full CRUD
- Linked to:
  - Publishers
  - Designers
  - Participants
  - Votes

### ✅ Participants & Voting
- Vote for designers in a show
- Unique constraint per (Participant, Designer, Show)

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


### Shows
- CRUD operations
- Linked to Publishers, Designers, Participants, and Votes

### Participants & Voting
- CRUD for Participants
- Voting logic implemented with unique index on (ParticipantId, DesignerId, ShowId)
- Views to allow voting per show from participant

---

## Extra Feature for Fashion Vote

### Role-Based Access Control
This project implements **role-based access control** using ASP.NET Core Identity. There are two user roles:

- **Admin**: Can create, edit, and delete shows, view all participants and votes.
- **Participant**: Can register for shows, vote for designers, upload outfit images, and manage their own participation.

Roles are assigned during user registration based on where the user signs up:
- If a user registers through the general navigation/register page, they are assigned the **Admin** role.
- If a user registers through the **Upcoming Shows** page, they are automatically assigned the **Participant** role and redirected to view their shows.

Protected endpoints are secured with role-specific `[Authorize(Roles = "...")]` attributes to ensure appropriate access control.

---

### Image Upload for Votes
After a **Participant** casts a vote for a designer in a show, they can upload an outfit image to represent their look.

Features:
- **Upload Image**: Each vote record allows uploading one image (e.g., outfit worn).
- **View Image**: If an image is uploaded, it is displayed alongside the designer's vote entry.
- **Update Image**: Participants can replace their uploaded image.
- **Delete Image**: Images can be deleted, which also removes the image file from the server.

Uploaded images are stored in the `wwwroot/uploads/` directory, and their URLs are saved in the database.

---

## 🧪 Testing & Swagger

- Swagger enabled at `/swagger`
- All API endpoints tested via Swagger/Postman
- Frontend UI tested for both reading and voting modules

---

## 🔗 Entity Relationship Summary

| Relationship               | Type            |
|---------------------------|-----------------|
| Publisher ↔ Show          | Many-to-Many    |
| Book ↔ Author             | Many-to-Many    |
| Designer ↔ Book           | Many-to-Many    |
| Designer ↔ Show           | Many-to-Many    |
| Participant ↔ Show        | Many-to-Many    |
| Vote                      | Composite FK    |

---

## 🧠 Database Management
- Identity key corruption in Shows resolved manually
- Original Show IDs preserved
- Manual FK recreation
- Data restored from backups
- Entity Framework migrations carefully managed
- Identity key corruption in Shows resolved manually
- Original Show IDs preserved
- Manual FK recreation
- Data restored from backups
- Entity Framework migrations carefully managed

---

## 🗂️ Project Structure
```
/Models         → Data models
/Controllers    → MVC controllers
/Views          → Razor pages
/Services       → Business logic
/Data           → ApplicationDbContext & config
```
```
/Models         → Data models
/Controllers    → MVC controllers
/Views          → Razor pages
/Services       → Business logic
/Data           → ApplicationDbContext & config
```

---

## 👥 Contributors
- **Adil**: Core backend dev for ReadingRoom, authors, publishers, book logic, UI enhancements, basic authentication and image upload feature
- **Genevieve**: core backend dev for Designer and Fashion Voting system logic, role-based authentication, Image upload for voted designer, and UI enhancements

---

## 📆 Last Updated
**April 15, 2025**