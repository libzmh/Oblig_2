import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Button, Card, Container, Row, Col, Modal } from 'react-bootstrap';
import * as QuizService from '../services/QuizService';
import type { Quiz } from '../types/quiz';

const QuizListPage = () => {
  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [loading, setLoading] = useState(true);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedQuiz, setSelectedQuiz] = useState<Quiz | null>(null);

  useEffect(() => {
    fetchQuizzes();
  }, []);

  const fetchQuizzes = async () => {
    try {
      const data = await QuizService.fetchQuizzes();
      setQuizzes(data);
    } catch (error) {
      console.error('Error fetching quizzes:', error);
    } finally {
      setLoading(false);
    }
  };

  const openDeleteModal = (quiz: Quiz) => {
    setSelectedQuiz(quiz);
    setShowDeleteModal(true);
  };

  const closeDeleteModal = () => {
    setShowDeleteModal(false);
    setSelectedQuiz(null);
  };

  const handleDelete = async () => {
    if (selectedQuiz?.id) {
      try {
        await QuizService.deleteQuiz(selectedQuiz.id);
        setQuizzes(quizzes.filter(q => q.id !== selectedQuiz.id));
        closeDeleteModal();
      } catch (error) {
        console.error('Error deleting quiz:', error);
      }
    }
  };

  if (loading) return <Container className="mt-5"><h3>Loading...</h3></Container>;

  return (
    <Container className="mt-5">
      <div className="text-center mb-5">
        <h1 className="display-4">MyQuiz</h1>
      </div>

      <Row className="g-4">
        <Col md={4}>
          <Card className="h-100 border-success">
            <Card.Body className="d-flex flex-column align-items-center justify-content-center">
              <h3 className="mb-4">Create New Quiz</h3>
              <Link to="/quiz/create">
                <Button variant="success" size="lg">+ Create</Button>
              </Link>
            </Card.Body>
          </Card>
        </Col>

        {quizzes.map((quiz) => (
          <Col md={4} key={quiz.id}>
            <Card className="h-100">
              <Card.Body className="d-flex flex-column">
                <Card.Title>{quiz.title}</Card.Title>
                <Card.Text className="mb-auto">{quiz.description}</Card.Text>
                <div className="d-flex gap-2 mt-3">
                  {quiz.questions && quiz.questions.length > 0 ? (
                    <Link to={`/quiz/take/${quiz.id}`}>
                      <Button variant="primary">Take</Button>
                    </Link>
                  ) : (
                    <Button variant="primary" disabled>No Questions</Button>
                  )}
                  <Link to={`/quiz/update/${quiz.id}`}>
                    <Button variant="warning">Update</Button>
                  </Link>
                  <Button
                    variant="danger"
                    onClick={() => openDeleteModal(quiz)}
                  >
                    Delete
                  </Button>
                </div>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>

      <Modal show={showDeleteModal} onHide={closeDeleteModal}>
        <Modal.Header closeButton className="bg-danger text-white">
          <Modal.Title>Confirm Delete</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>Are you sure you want to delete this quiz?</p>
          {selectedQuiz && <p className="fw-bold">{selectedQuiz.title}</p>}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={closeDeleteModal}>
            Cancel
          </Button>
          <Button variant="danger" onClick={handleDelete}>
            Yes, Delete
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
};

export default QuizListPage;