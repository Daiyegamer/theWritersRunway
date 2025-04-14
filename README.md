# ✨ theWritersRunway  
Merging writing with fashion — where books meet fashion shows.

---

## 📚 Project Overview

This unified platform integrates two major systems:

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

## 👗 Fashion Voting Module by Genevieve

### 📘 Designer ↔ Book Linking
- Link books during **designer create/edit**
- View books on designer detail page
- Optional “Remove Book” (if Admin)

### ✨ Designer View Enhancements
- DesignerDetailsViewModel shows:
  - Assigned Shows
  - Assigned Books
- Clean UI with conditional button rendering

### 📖 Book Enhancements
- Book details now show linked designers
- Designer names link to their profile

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

## 🧪 Testing & Swagger

- Swagger enabled at `/swagger`
- All API endpoints tested via Swagger/Postman
- Frontend UI tested for both reading and voting modules

---

## 🧠 Database Management
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

---

## 👥 Contributors
- **Adil**: Core backend dev for ReadingRoom, authors, publishers, book logic, UI enhancements, basic authentication and image upload feature
- **Genevieve**: core backend dev for Designer and Fashion Voting system logic, role-based authentication, UI enhancements

---

## 📆 Last Updated
**April 03, 2025**
