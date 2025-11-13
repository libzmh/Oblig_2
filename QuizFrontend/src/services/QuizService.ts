import type { Quiz } from '../types/quiz';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5186';

const handleResponse = async (response: Response) => {
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  
  if (response.status === 204) {
    return null;
  }
  
  const text = await response.text();
  return text ? JSON.parse(text) : null;
};

const headers = {
  'Content-Type': 'application/json',
};

export const fetchQuizzes = async (): Promise<Quiz[]> => {
  const response = await fetch(`${API_URL}/api/quiz`);
  return handleResponse(response);
};

export const fetchQuizById = async (id: string): Promise<Quiz> => {
  const response = await fetch(`${API_URL}/api/quiz/${id}`);
  return handleResponse(response);
};

export const createQuiz = async (quiz: Quiz): Promise<Quiz> => {
  const response = await fetch(`${API_URL}/api/quiz`, {
    method: 'POST',
    headers,
    body: JSON.stringify(quiz),
  });
  return handleResponse(response);
};

export const updateQuiz = async (id: number, quiz: Quiz): Promise<Quiz> => {
  const response = await fetch(`${API_URL}/api/quiz/${id}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(quiz),
  });
  return handleResponse(response);
};

export const deleteQuiz = async (id: number): Promise<void> => {
  const response = await fetch(`${API_URL}/api/quiz/${id}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};

export const submitQuiz = async (id: number, answers: Record<number, string[]>) => {
  const response = await fetch(`${API_URL}/api/quiz/${id}/submit`, {
    method: 'POST',
    headers,
    body: JSON.stringify(answers),
  });
  return handleResponse(response);
};