import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavMenu from './shared/NavMenu';
import QuizListPage from './quiz/QuizListPage';
import QuizCreatePage from './quiz/QuizCreatePage';
import QuizUpdatePage from './quiz/QuizUpdatePage';
import QuizTakePage from './quiz/QuizTakePage';
import QuizResultPage from './quiz/QuizResultPage';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <Router>
      <NavMenu />
      <Routes>
        <Route path="/" element={<QuizListPage />} />
        <Route path="/quiz/create" element={<QuizCreatePage />} />
        <Route path="/quiz/update/:id" element={<QuizUpdatePage />} />
        <Route path="/quiz/take/:id" element={<QuizTakePage />} />
        <Route path="/quiz/result" element={<QuizResultPage />} />
      </Routes>
    </Router>
  );
}

export default App;