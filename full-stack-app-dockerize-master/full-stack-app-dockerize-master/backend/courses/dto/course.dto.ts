import * as Joi from 'joi';

export interface CourseDto {
  id: string;
  code: string;
  title: string;
  description?: string;
  content: string;
  price: number;
  releasedDate: Date;
}

export function validateCourse(course: CourseDto) {
  const schema = Joi.object({
    id: Joi.string(),
    _id: Joi.string(),
    code: Joi.string().min(5).max(50).required(),
    title: Joi.string().min(5).max(50).required(),
    content: Joi.string().min(10),
    price: Joi.number().min(0),
  });

  return schema.validate(course);
}
