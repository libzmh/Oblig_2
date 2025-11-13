# Quiz Application

A web-based quiz creation and taking tool with two modes: quiz creation mode (CRUD operations) and quiz taking mode (interactive quizzes with scoring).

## Tech Stack
- **Backend:** .NET Core 8.0 Web API
- **Frontend:** React 18 with TypeScript, Vite, React Bootstrap
- **Database:** SQLite with Entity Framework Core

## Requirements
- .NET 8.0 SDK
- Node.js v22.18.0
- npm

## Project Structure
```
/QuizAPI          - Backend API (.NET Core)
/QuizFrontend     - Frontend SPA (React + TypeScript)
```

## How to Start the Application

### Backend Setup
1. Navigate to backend folder:
```bash
cd QuizAPI
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Update database:
```bash
dotnet ef database update
```

4. Run the backend:
```bash
dotnet run
```
Backend runs at: http://localhost:5186

### Frontend Setup
1. Open new terminal and navigate to frontend folder:
```bash
cd QuizFrontend
```

2. Install dependencies:
```bash
npm install
```

3. Run the frontend:
```bash
npm run dev
```
Frontend runs at: http://localhost:5173

## Features

### Quiz Creation Mode
- Create new quizzes with title and description
- Add multiple questions with different types:
  - Multiple Choice (radio buttons)
  - Checkbox (multiple correct answers)
  - Short Answer (text input)
  - Dropdown (select menu)
- Dynamic options management (add/remove options)
- Set points per question
- Update existing quizzes
- Delete quizzes

### Quiz Taking Mode
- Take quizzes interactively
- Submit answers
- View detailed results with:
  - Total score and percentage
  - Question-by-question breakdown
  - Correct vs user answers
- Retake quizzes

## API Endpoints
- `GET /api/quiz` - Get all quizzes
- `GET /api/quiz/{id}` - Get quiz by ID
- `POST /api/quiz` - Create new quiz
- `PUT /api/quiz/{id}` - Update quiz
- `DELETE /api/quiz/{id}` - Delete quiz
- `POST /api/quiz/{id}/submit` - Submit quiz answers