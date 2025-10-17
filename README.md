# 📚 Smart Student Organizer

## 🎯 Overview
**Smart Student Organizer** is a modern, user-friendly desktop application designed to help students manage their academic life efficiently. It combines class schedules, assignments, and personal to-do lists in one clean interface.

## ✨ Features

### 1. 📅 **Main Dashboard (Weekly Timetable View)**
- Visual weekly class timetable (Monday-Saturday, 8AM-6PM)
- Color-coded subjects for easy identification
- Today's assignments preview
- Personal to-do list preview
- Quick navigation to all modules

### 2. 📅 **Class Schedule Management**
- Add/Edit/Delete classes
- Set class times, locations, and instructors
- Link classes to subjects with codes
- View weekly study hours

### 3. 📝 **Assignment Management**
- Track assignments with deadlines
- Priority levels (High, Medium, Low)
- Subject categorization with dropdown
- Filter by status and priority
- Visual due date indicators

### 4. ✓ **Personal To-Do List**
- Simple checkbox-based to-do tracking
- Category organization
- Quick add/complete functionality
- Separate from academic assignments

## 🛠️ Technical Stack

- **Platform:** .NET Framework 4.8 (Windows Forms)
- **Database:** SQLite with Dapper ORM
- **Architecture:** Layered (Models, Services, Forms, Utils)
- **Design Patterns:** Repository Pattern, Interface Segregation
- **Query Language:** LINQ for data filtering and sorting

## 🎨 Design Principles

- **Simple & Modern UI:** Clean layouts with intuitive navigation
- **User-Friendly:** Minimal clicks to access any feature
- **Visual Clarity:** Color coding and icons for quick recognition
- **Responsive Layout:** Organized panels and grouped information

## 📁 Project Structure

```
UniPlanner/
├── Models/
│   ├── SubjectItem.cs      # Course/Subject entity
│   ├── TaskItem.cs          # Assignment entity
│   ├── ScheduleItem.cs      # Class schedule entity
│   ├── TodoItem.cs          # Personal to-do entity
│   └── Interfaces/
├── Services/
│   ├── SubjectService.cs    # Subject CRUD + sample data
│   ├── TaskService.cs       # Assignment management with LINQ
│   ├── ScheduleService.cs   # Schedule management
│   ├── TodoService.cs       # To-do list management
│   └── DbBootstrap.cs       # Database initialization
├── Forms/
│   ├── MainForm.cs          # Main dashboard with timetable
│   ├── ScheduleForm.cs      # Class schedule management
│   ├── TaskForm.cs          # Assignment management (renamed)
│   └── TodoForm.cs          # Personal to-do management
└── Utils/
    ├── Extensions.cs        # Helper extension methods
    └── ValidationHelper.cs  # Input validation
```

## 🚀 Getting Started

### Prerequisites
- Windows 10/11
- .NET Framework 4.8
- Visual Studio 2022 (recommended)

### Installation

1. **Open Solution**
   ```
   Open UniPlanner.sln in Visual Studio 2022
   ```

2. **Restore NuGet Packages**
   ```
   Right-click solution → Restore NuGet Packages
   ```

3. **Build Solution**
   ```
   Build → Rebuild Solution (Ctrl + Shift + B)
   ```

4. **Run Application**
   ```
   Press F5 or Ctrl + F5
   ```

### First Run
- Database (`uni.db`) is created automatically in `Data/` folder
- Sample subjects are pre-loaded for testing
- All tables are initialized on first launch

## 📊 Database Schema

### Subjects Table
Stores course information with codes and colors.

### Tasks Table
Academic assignments with deadlines and priorities.

### Schedule Table
Weekly class timetable with time slots.

### Todos Table
Personal to-do items separate from academics.

## 💡 How to Use

### Main Dashboard
1. View your weekly timetable at a glance
2. Check today's assignments in the sidebar
3. See active personal to-dos
4. Click navigation buttons to manage data

### Managing Classes
1. Click "📅 Manage Class Schedule"
2. Select day, subject, time, and location
3. Add to timetable
4. View on main dashboard instantly

### Managing Assignments
1. Click "📝 Manage Assignments"
2. Enter assignment title and details
3. Select subject from dropdown (linked to your classes)
4. Set due date and priority
5. Track completion status

### Managing To-Dos
1. Click "✓ Manage Personal To-Do"
2. Add quick tasks
3. Check off when complete
4. Keep personal tasks separate from academic work

## 🎓 Academic Features Demonstrated

### Object-Oriented Programming (OOP)
- ✅ **Inheritance & Interfaces:** `IRepository<T>`, `IUserSettings`
- ✅ **Polymorphism:** Generic repository pattern
- ✅ **Encapsulation:** Service layer abstracts data access

### LINQ Queries
- Filter upcoming/overdue tasks
- Group schedules by day
- Sort by priority and date
- Aggregate statistics

### Database Integration
- SQLite for local persistence
- Dapper ORM for efficient queries
- Automatic schema creation
- Sample data initialization

## 📈 Project Goals

Our project aims to:
1. **Reduce Student Stress:** One place for all academic tasks
2. **Improve Productivity:** Visual timetables and reminders
3. **Simplify Organization:** Intuitive, clean interface
4. **Demonstrate Technical Skills:** OOP, LINQ, Database, UI Design

## 🤝 Team Contribution

This project demonstrates modern software development practices:
- Layered architecture for maintainability
- Repository pattern for data abstraction
- LINQ for powerful data queries
- User-centered design principles

## 📝 License

Educational project for UTS .NET Framework course.

## 📅 Version

Version 2.0 - Redesigned UI - October 2025

---

**Smart Student Organizer** - Keeping students organized, productive, and stress-free! 📚✨
