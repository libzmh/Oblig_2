import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, Card, Button, Form, Alert } from 'react-bootstrap';
import * as QuizService from '../services/QuizService';
import type { Quiz } from '../types/quiz';
import type { Question } from '../types/question';
import { QuestionType } from '../types/question';

interface QuestionWithOptions extends Question {
    options: string[];
    correctAnswers: string[];
}

const QuizCreatePage = () => {
    const navigate = useNavigate();
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [questions, setQuestions] = useState<QuestionWithOptions[]>([]);
    const [error, setError] = useState('');

    const addQuestion = () => {
        const newQuestion: QuestionWithOptions = {
            questionText: '',
            type: QuestionType.MultipleChoice,
            optionsJson: JSON.stringify([]),
            correctAnswersJson: JSON.stringify([]),
            points: 1,
            options: ['', ''],
            correctAnswers: [''],
        };
        setQuestions([...questions, newQuestion]);
    };

    const removeQuestion = (index: number) => {
        setQuestions(questions.filter((_, i) => i !== index));
    };

    const updateQuestion = (index: number, field: keyof Question, value: any) => {
        const updated = [...questions];
        updated[index] = { ...updated[index], [field]: value };
        setQuestions(updated);
    };

    const addOption = (qIndex: number) => {
        const updated = [...questions];
        updated[qIndex].options.push('');
        setQuestions(updated);
    };

    const removeOption = (qIndex: number, optIndex: number) => {
        const updated = [...questions];
        updated[qIndex].options = updated[qIndex].options.filter((_, i) => i !== optIndex);
        setQuestions(updated);
    };

    const updateOption = (qIndex: number, optIndex: number, value: string) => {
        const updated = [...questions];
        updated[qIndex].options[optIndex] = value;
        setQuestions(updated);
    };

    const updateCorrectAnswer = (qIndex: number, value: string, isCheckbox: boolean) => {
        const updated = [...questions];
        if (isCheckbox) {
            const current = updated[qIndex].correctAnswers;
            updated[qIndex].correctAnswers = current.includes(value)
                ? current.filter(a => a !== value)
                : [...current, value];
        } else {
            updated[qIndex].correctAnswers = [value];
        }
        setQuestions(updated);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        /* 
        Initial implementation: Prevent saving quiz without questions
        Later changed approach: Allow empty quiz creation, but disable "Take" button if no questions
        
        if (questions.length === 0) {
           setError('Please add at least one question');
          return;
        } 
        */
        try {
            const quiz: Quiz = {
                title,
                description,
                questions: questions.map(q => ({
                    questionText: q.questionText,
                    type: q.type,
                    optionsJson: JSON.stringify(q.options.filter(o => o.trim())),
                    correctAnswersJson: JSON.stringify(q.correctAnswers.filter(a => a.trim())),
                    points: q.points,
                })),
            };
            await QuizService.createQuiz(quiz);
            navigate('/');
        } catch (err) {
            setError('Failed to create quiz');
            console.error(err);
        }
    };

    return (
        <Container className="mt-5">
            <div className="col-md-8 mx-auto">
                <h2 className="mb-4">Create New Quiz</h2>

                {error && <Alert variant="danger">{error}</Alert>}

                <Form onSubmit={handleSubmit}>
                    <Card className="mb-4">
                        <Card.Header className="bg-primary text-white">
                            <h5 className="mb-0">Quiz Information</h5>
                        </Card.Header>
                        <Card.Body>
                            <Form.Group className="mb-3">
                                <Form.Label>Quiz Title</Form.Label>
                                <Form.Control
                                    type="text"
                                    value={title}
                                    onChange={(e) => setTitle(e.target.value)}
                                    required
                                />
                            </Form.Group>

                            <Form.Group className="mb-3">
                                <Form.Label>Description</Form.Label>
                                <Form.Control
                                    as="textarea"
                                    rows={3}
                                    value={description}
                                    onChange={(e) => setDescription(e.target.value)}
                                />
                            </Form.Group>
                        </Card.Body>
                    </Card>

                    <Card className="mb-4">
                        <Card.Header className="bg-success text-white">
                            <h5 className="mb-0">Questions</h5>
                        </Card.Header>
                        <Card.Body>
                            {questions.length === 0 && (
                                <div className="text-muted text-center py-4">
                                    No questions added yet. Click "Add Question" to start.
                                </div>
                            )}

                            {questions.map((q, qIndex) => (
                                <div key={qIndex} className="mb-4 p-3 border rounded">
                                    <div className="d-flex justify-content-between mb-3">
                                        <h6>Question {qIndex + 1}</h6>
                                        <Button
                                            variant="danger"
                                            size="sm"
                                            onClick={() => removeQuestion(qIndex)}
                                        >
                                            Remove
                                        </Button>
                                    </div>

                                    <Form.Group className="mb-3">
                                        <Form.Label>Question Text</Form.Label>
                                        <Form.Control
                                            type="text"
                                            value={q.questionText}
                                            onChange={(e) =>
                                                updateQuestion(qIndex, 'questionText', e.target.value)
                                            }
                                            required
                                        />
                                    </Form.Group>

                                    <Form.Group className="mb-3">
                                        <Form.Label>Question Type</Form.Label>
                                        <Form.Select
                                            value={q.type}
                                            onChange={(e) =>
                                                updateQuestion(qIndex, 'type', Number(e.target.value))
                                            }
                                        >
                                            <option value={QuestionType.MultipleChoice}>Multiple Choice</option>
                                            <option value={QuestionType.Checkbox}>Checkbox</option>
                                            <option value={QuestionType.ShortAnswer}>Short Answer</option>
                                            <option value={QuestionType.Dropdown}>Dropdown</option>
                                        </Form.Select>
                                    </Form.Group>

                                    {(q.type === QuestionType.MultipleChoice ||
                                        q.type === QuestionType.Checkbox ||
                                        q.type === QuestionType.Dropdown) && (
                                            <div className="mb-3">
                                                <Form.Label>Options</Form.Label>
                                                {q.options.map((opt, optIndex) => (
                                                    <div key={optIndex} className="d-flex gap-2 mb-2">
                                                        <Form.Control
                                                            type="text"
                                                            value={opt}
                                                            onChange={(e) => updateOption(qIndex, optIndex, e.target.value)}
                                                            placeholder={`Option ${optIndex + 1}`}
                                                            required
                                                        />
                                                        {q.options.length > 2 && (
                                                            <Button
                                                                variant="danger"
                                                                size="sm"
                                                                onClick={() => removeOption(qIndex, optIndex)}
                                                            >
                                                                Remove
                                                            </Button>
                                                        )}
                                                    </div>
                                                ))}
                                                <Button
                                                    variant="secondary"
                                                    size="sm"
                                                    onClick={() => addOption(qIndex)}
                                                >
                                                    + Add Option
                                                </Button>
                                            </div>
                                        )}

                                    {q.type === QuestionType.ShortAnswer ? (
                                        <Form.Group className="mb-3">
                                            <Form.Label>Correct Answer</Form.Label>
                                            <Form.Control
                                                type="text"
                                                value={q.correctAnswers[0] || ''}
                                                onChange={(e) => updateCorrectAnswer(qIndex, e.target.value, false)}
                                                required
                                            />
                                        </Form.Group>
                                    ) : (q.type === QuestionType.MultipleChoice || q.type === QuestionType.Dropdown) ? (
                                        <Form.Group className="mb-3">
                                            <Form.Label>Correct Answer</Form.Label>
                                            <Form.Select
                                                value={q.correctAnswers[0] || ''}
                                                onChange={(e) => updateCorrectAnswer(qIndex, e.target.value, false)}
                                                required
                                            >
                                                <option value="">Select correct answer</option>
                                                {q.options.filter(o => o.trim()).map((opt, i) => (
                                                    <option key={i} value={opt}>{opt}</option>
                                                ))}
                                            </Form.Select>
                                        </Form.Group>
                                    ) : (
                                        <Form.Group className="mb-3">
                                            <Form.Label>Correct Answers (select all that apply)</Form.Label>
                                            {q.options.filter(o => o.trim()).map((opt, i) => (
                                                <Form.Check
                                                    key={i}
                                                    type="checkbox"
                                                    label={opt}
                                                    checked={q.correctAnswers.includes(opt)}
                                                    onChange={() => updateCorrectAnswer(qIndex, opt, true)}
                                                />
                                            ))}
                                        </Form.Group>
                                    )}

                                    <Form.Group className="mb-3">
                                        <Form.Label>Points</Form.Label>
                                        <Form.Control
                                            type="number"
                                            min="1"
                                            max="100"
                                            value={q.points}
                                            onChange={(e) =>
                                                updateQuestion(qIndex, 'points', Number(e.target.value))
                                            }
                                        />
                                    </Form.Group>
                                </div>
                            ))}

                            <div className="text-center mt-3">
                                <Button variant="success" onClick={addQuestion}>
                                    + Add Question
                                </Button>
                            </div>
                        </Card.Body>
                    </Card>

                    <div className="d-flex gap-2 mb-5">
                        <Button type="submit" variant="primary" size="lg">
                            Save Quiz
                        </Button>
                        <Button variant="secondary" size="lg" onClick={() => navigate('/')}>
                            Cancel
                        </Button>
                    </div>
                </Form>
            </div>
        </Container>
    );
};

export default QuizCreatePage;