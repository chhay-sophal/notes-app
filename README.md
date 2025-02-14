# NotesApp

## Overview
NotesApp is a web application for creating, editing, and managing notes. The backend is built with ASP.NET Core, and the frontend is developed using Vue.js.

## Technologies Used
- **Backend:** ASP.NET Core 9.0.1
- **Frontend:** Vue.js 3.5.13
- **Database:** SQL Server 

## Features
- User authentication and authorization
- Create, edit, and delete notes
- Search functionality

## Getting Started

### Prerequisites
- .NET SDK 9.0.102
- Node.js 22.13.0
- npm 11.1.0
- SQL Server

### Database Setup
1. Ensure SQL Server is installed and running on your machine.
2. Create a new database for the application.
3. Run the SQL scripts to create the tables and insert testing data:
    - `create_tables.sql`

### Configuration
1. Copy `appsettings.json.example` file and rename to `appsettings.json` in the backend directory.
2. Add the connection string for SQL Server:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
      },
      "JWT": {
        "Key": "your_jwt_secret_key",
        "Issuer": "your_jwt_issuer",
        "Audience": "your_jwt_audience"
      }
    }
    ```

### Backend Run
1. Navigate to the backend directory:
    ```sh
    cd backend
    ```
2. Run the application:
    ```sh
    dotnet run
    ```

### Frontend Setup
1. Navigate to the frontend directory:
    ```sh
    cd frontend
    ```
2. Install the dependencies:
    ```sh
    npm install
    ```
3. Start the frontend:
    ```sh
    npm run dev
    ```
