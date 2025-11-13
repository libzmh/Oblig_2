export const QuestionType = {
  MultipleChoice: 0,
  Checkbox: 1,
  ShortAnswer: 2,
  Dropdown: 3
} as const;

export type QuestionType = typeof QuestionType[keyof typeof QuestionType];

export interface Question {
  id?: number;
  questionText: string;
  type: QuestionType;
  optionsJson: string;
  correctAnswersJson: string;
  points: number;
  quizId?: number;
}