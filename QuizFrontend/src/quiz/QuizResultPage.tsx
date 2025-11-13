import { useLocation, useNavigate, Link } from 'react-router-dom';
import { Container, Card, Button, Badge } from 'react-bootstrap';

const QuizResultPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { result } = location.state || {};

  if (!result) {
    return (
      <Container className="mt-5">
        <h3>No result data available</h3>
        <Link to="/">
          <Button variant="primary">Back to Home</Button>
        </Link>
      </Container>
    );
  }

  const { quizTitle, totalScore, maxScore, percentage, questionResults, quizId } = result;

  return (
    <Container className="mt-5">
      <div className="col-md-8 mx-auto">
        <Card className="mb-4 text-center">
          <Card.Header className="bg-success text-white">
            <h2 className="mb-0">Quiz Completed!</h2>
          </Card.Header>
          <Card.Body>
            <h3 className="mb-3">{quizTitle}</h3>
            <div className="display-4 mb-3">
              <span className="text-primary">{totalScore}</span> / {maxScore}
            </div>
            <h4 className="mb-3">
              {percentage >= 80 ? (
                <Badge bg="success" className="fs-5">Excellent! {percentage.toFixed(1)}%</Badge>
              ) : percentage >= 60 ? (
                <Badge bg="warning" className="fs-5">Good! {percentage.toFixed(1)}%</Badge>
              ) : (
                <Badge bg="danger" className="fs-5">{percentage.toFixed(1)}%</Badge>
              )}
            </h4>
            <p className="text-muted">
              You answered {questionResults.filter((q: any) => q.isCorrect).length} out of {questionResults.length} questions correctly.
            </p>
          </Card.Body>
        </Card>

        <h4 className="mb-3">Detailed Results</h4>

        {questionResults.map((result: any, index: number) => (
          <Card key={index} className={`mb-3 ${result.isCorrect ? 'border-success' : 'border-danger'}`}>
            <Card.Header className={result.isCorrect ? 'bg-success bg-opacity-10' : 'bg-danger bg-opacity-10'}>
              <div className="d-flex justify-content-between align-items-center">
                <h6 className="mb-0">Question {index + 1}</h6>
                <div>
                  {result.isCorrect ? (
                    <Badge bg="success">✓ Correct (+{result.points} pts)</Badge>
                  ) : (
                    <Badge bg="danger">✗ Incorrect</Badge>
                  )}
                </div>
              </div>
            </Card.Header>
            <Card.Body>
              <p className="fw-bold mb-3">{result.questionText}</p>
              <div className="row">
                <div className="col-md-6">
                  <p className="mb-1">
                    <strong>Your Answer:</strong>{' '}
                    <span className={result.isCorrect ? 'text-success' : 'text-danger'}>
                      {Array.isArray(result.userAnswers) ? result.userAnswers.join(', ') : result.userAnswers}
                    </span>
                  </p>
                </div>
                <div className="col-md-6">
                  <p className="mb-1">
                    <strong>Correct Answer:</strong>{' '}
                    <span className="text-success">
                      {Array.isArray(result.correctAnswers) ? result.correctAnswers.join(', ') : result.correctAnswers}
                    </span>
                  </p>
                </div>
              </div>
            </Card.Body>
          </Card>
        ))}

        <div className="d-flex gap-2 mb-5">
          <Button variant="primary" size="lg" onClick={() => navigate(`/quiz/take/${quizId}`)}>
            Retake Quiz
          </Button>
          <Button variant="secondary" size="lg" onClick={() => navigate('/')}>
            Back to Home
          </Button>
        </div>
      </div>
    </Container>
  );
};

export default QuizResultPage;