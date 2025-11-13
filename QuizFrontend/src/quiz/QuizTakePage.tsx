import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Container, Card, Form, Button, Alert } from 'react-bootstrap';
import * as QuizService from '../services/QuizService';
import type { Quiz } from '../types/quiz';
import { QuestionType } from '../types/question';

const QuizTakePage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState<Quiz | null>(null);
  const [answers, setAnswers] = useState<Record<number, string[]>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchQuiz();
  }, [id]);

  const fetchQuiz = async () => {
    try {
      const data = await QuizService.fetchQuizById(id!);
      setQuiz(data);
    } catch (err) {
      setError('Failed to load quiz');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleAnswerChange = (questionId: number, value: string, isCheckbox = false) => {
    if (isCheckbox) {
      const current = answers[questionId] || [];
      const updated = current.includes(value)
        ? current.filter(v => v !== value)
        : [...current, value];
      setAnswers({ ...answers, [questionId]: updated });
    } else {
      setAnswers({ ...answers, [questionId]: [value] });
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const result = await QuizService.submitQuiz(quiz!.id!, answers);
      navigate('/quiz/result', { state: { result } });
    } catch (err) {
      setError('Failed to submit quiz');
      console.error(err);
    }
  };

  if (loading) return <Container className="mt-5"><h3>Loading...</h3></Container>;
  if (error) return <Container className="mt-5"><Alert variant="danger">{error}</Alert></Container>;
  if (!quiz) return <Container className="mt-5"><Alert variant="warning">Quiz not found</Alert></Container>;

  return (
    <Container className="mt-5">
      <div className="col-md-8 mx-auto">
        <Card className="mb-4">
          <Card.Header className="bg-primary text-white">
            <h2 className="mb-0">{quiz.title}</h2>
            {quiz.description && <p className="mb-0 mt-2">{quiz.description}</p>}
          </Card.Header>
        </Card>

        <Form onSubmit={handleSubmit}>
          {quiz.questions.map((question, index) => {
            const options = JSON.parse(question.optionsJson);
            const isCheckbox = question.type === QuestionType.Checkbox;

            return (
              <Card key={question.id} className="mb-4">
                <Card.Header className="bg-light">
                  <h5 className="mb-0">
                    Question {index + 1}{' '}
                    <span className="badge bg-secondary">{question.points} pts</span>
                  </h5>
                </Card.Header>
                <Card.Body>
                  <p className="fw-bold mb-3">{question.questionText}</p>

                  {question.type === QuestionType.ShortAnswer ? (
                    <Form.Control
                      type="text"
                      onChange={(e) => handleAnswerChange(question.id!, e.target.value)}
                      required
                    />
                  ) : question.type === QuestionType.Dropdown ? (
                    <Form.Select
                      onChange={(e) => handleAnswerChange(question.id!, e.target.value)}
                      required
                    >
                      <option value="">Select an answer</option>
                      {options.map((opt: string, i: number) => (
                        <option key={i} value={opt}>{opt}</option>
                      ))}
                    </Form.Select>
                  ) : (
                    options.map((opt: string, i: number) => (
                      <Form.Check
                        key={i}
                        type={isCheckbox ? 'checkbox' : 'radio'}
                        name={`question-${question.id}`}
                        id={`q${question.id}-${i}`}
                        label={opt}
                        value={opt}
                        onChange={() => handleAnswerChange(question.id!, opt, isCheckbox)}
                        required={!isCheckbox}
                      />
                    ))
                  )}
                </Card.Body>
              </Card>
            );
          })}

          <div className="d-flex gap-2 mb-5">
            <Button type="submit" variant="primary" size="lg">Submit Quiz</Button>
            <Button variant="secondary" size="lg" onClick={() => navigate('/')}>Cancel</Button>
          </div>
        </Form>
      </div>
    </Container>
  );
};

export default QuizTakePage;