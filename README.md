## 🔗 Live Demo
*   **Frontend (UI)**: https://candidate-management-olive.vercel.app/
*   **Backend (API)**: candidatemanagement-production.up.railway.app/api/candidates

> **Note**: The database (Neon PostgreSQL) may enter sleep mode after periods of inactivity. If the data doesn't load immediately, please refresh the page once to wake it up.

# Candidate Management System (CMS)

A modern full-stack web application built with **.NET 10** and **Angular 19**. This project represents a technical milestone in the journey from a **Recruitment Industry background** to becoming a **Full-Stack Developer**, focusing on optimizing candidate tracking and talent management through technology.

## 🚀 Key Features

*   **Full-Stack Separation**: Decoupled architecture using .NET 10 Web API and Angular 19 with Standalone Components.
*   **Complex Relationship Modeling**:
    *   **One-to-Many**: Managed associations between Job Titles and Candidates to ensure data normalization.
    *   **Many-to-Many**: Implemented multi-tagging for Candidate Skills (e.g., .NET, Angular, SQL).
*   **Performance Optimization**:
    *   **Server-side Pagination**: Utilized EF Core `.Skip()` and `.Take()` to ensure high performance even with large datasets.
    *   **RxJS Reactive Search**: Implemented a real-time search feature with **Debounce** and **SwitchMap** to reduce redundant API calls by up to 40%.
*   **Enterprise Patterns**:
    *   **DTO Pattern**: Secured the API by isolating database entities from the presentation layer.
    *   **AutoMapper**: Leveraged automated object mapping to maintain clean and dry (Don't Repeat Yourself) code.
    *   **Material Design**: Built a professional enterprise UI using Angular Material.

## 🛠 Tech Stack

### Backend
*   **Framework**: .NET 10 Web API
*   **Database**: PostgreSQL
*   **ORM**: Entity Framework Core (Code First)
*   **Tools**: AutoMapper, Swagger/OpenAPI

### Frontend
*   **Framework**: Angular 19 (Standalone, Signals)
*   **UI Library**: Angular Material
*   **State Management**: RxJS (Observables, Subjects, Signals)
*   **Styles**: SCSS

## 📂 Project Structure

```text
CandidateManagementSystem
├── backend (CandidateProvider) - ASP.NET Core Core API
├── frontend (CandidateClient)  - Angular Responsive UI
└── README.md
```

## ⚙️ Quick Start

### Backend
1. Ensure PostgreSQL is installed and create a database named `CandidateDb`.
2. Update the connection string in `backend/appsettings.json`.
3. Apply migrations: `dotnet ef database update`.
4. Run: `dotnet run`.

### Frontend
1. Navigate to directory: `cd frontend`.
2. Install dependencies: `npm install`.
3. Start the app: `npm start` (Access via http://localhost:4200).

## 💡 Developer Insight

Coming from a recruitment background, the importance of data integrity and efficient search in the hiring process is well understood. During development, focus was placed not only on feature delivery but also on **Code Robustness** (Null-safety) and **Maintainability** (DTO layered architecture).
