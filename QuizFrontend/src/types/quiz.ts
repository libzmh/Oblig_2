import type { Question } from './question';

export interface Quiz {
  id?: number;
  title: string;
  description?: string;
  createdDate?: Date;
  questions: Question[];
}