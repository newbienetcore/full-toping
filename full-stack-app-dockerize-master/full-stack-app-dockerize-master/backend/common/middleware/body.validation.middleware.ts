import express from 'express';
import { validateCourse } from '../../courses/dto/course.dto';

class BodyValidationMiddleware {
  verifyBodyFieldsErrors(req: express.Request, res: express.Response, next: express.NextFunction) {
    const { error } = validateCourse(req.body);
    if (error) return res.status(400).json({ error: error.details[0].message });

    next();
  }
}

export default new BodyValidationMiddleware();