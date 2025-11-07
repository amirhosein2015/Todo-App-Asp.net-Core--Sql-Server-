<div align="center">

# 📝 Todo App  
### A Task Management Application with User Accounts, Priorities & Filters  
**Built with ASP.NET Core & SQL Server**

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)  
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)  
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

</div>

---

## 🧩 Project Overview  
This repository contains a modern web application for managing to-do tasks, built using ASP.NET Core and SQL Server. Key features include:

- User registration and login with account management.  
- Ability to create, read, update and delete tasks (CRUD operations).  
- Assigning priorities to tasks (e.g., High, Medium, Low).  
- Filtering and searching tasks based on status, priority, user or date.  
- Responsive UI using HTML5, CSS3 and client-side scripting.  
- Secure authentication and authorization built-in via ASP.NET Core Identity.

---

## ⚙️ Technologies & Tools  
- **ASP.NET Core** (Web MVC) — server-side logic, routing, controllers & views.  
- **C#** — business logic implementation, domain models, view models.  
- **SQL Server** — relational database for storing users, tasks, priorities, filters.  
- **Entity Framework Core** — code-first migrations, database access.  
- **JavaScript / AJAX** for dynamic UI features (optional filtering, real-time updates).  
- **HTML5 & CSS3** for front-end layout and styling.

---

## 🚀 Getting Started  
1. Clone the repository:  
   ```bash
   git clone https://github.com/amirhosein2015/Todo-App-Asp.net-Core--Sql-Server-.git

2.Open the solution in Visual Studio (or VS Code) and restore NuGet packages.

3.Configure the connection string in appsettings.json (or appsettings.Development.json) to point to your local SQL Server instance.

4.Apply migrations to create the database schema:
 ```bash
   dotnet ef database update


5.Run the application:
 ```bash
    dotnet run


 6.Open your browser and navigate to https://localhost:5001 (or the port assigned) to access the application. 


 ## 🛠 Features & Enhancements

✅ User authentication: register, login, logout, manage profile.

✅ Task management: create tasks with description, due date, priority, status.

✅ Priority levels: e.g., High, Medium, Low — to help sorting and focus.

✅ Filtering & search: filter tasks by priority, status, user, or text match.

✅ Responsive UI for desktop and mobile devices.

🔧 Planned improvements: integration of real–time updates using SignalR, user-role management, drag-and-drop task ordering.


## 📚 Directory Structure
/Controllers        – MVC controllers for handling HTTP requests  
/Data               – DbContext and migrations for EF Core  
/Models             – Domain models and view-models  
/Views              – Razor views for UI  
/wwwroot            – Static assets (CSS, JS, images)  
Program.cs          – Entry point and ASP.NET Core setup  
appsettings.json    – Configuration (e.g., connection string)


## 👥 Contributing

Contributions are welcome! Feel free to open issues for bugs or feature requests, or submit pull requests for enhancements. Please follow the repository’s code style and include meaningful commit messages.

## 📄 License

This project is open-source and available under the MIT License
.

## 💡 Why Use This?

If you’re a developer (especially using C# / .NET) and you want a simple but fully-featured starter application for task management, this Todo App is a great base. You can learn by exploring how user authentication is handled, how tasks are modeled and filtered in the database, and how the UI connects to the backend. Also, you can extend it to more complex systems (e.g., team tasks, Kanban board, project management) with minimal effort.
