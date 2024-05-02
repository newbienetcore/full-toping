import express from 'express';
import debug from 'debug';
import coursesService from './../services/courses.service';

const log: debug.IDebugger = debug('app:courses-middleware');

class CoursesMiddleware {
  async validateSameCodeDoesntExist(
    req: express.Request,
    res: express.Response,
    next: express.NextFunction) {
    const course = await coursesService.getCourseByCode(req.body.code);
    if (course) {
      res.status(400).json({ error: `Code is existed` });
    } else {
      next();
    }
  }

  async validateSameCodeBelongToSameCourse(
    req: express.Request,
    res: express.Response,
    next: express.NextFunction) {
    const courseId = req.params.id;
    console.log('courseId', courseId);
    const course = await coursesService.getCourseByCode(req.body.code);
    console.log('course', course);
    if (course === null) return next();
    if (course && String(course._id) === courseId) {
      return next();
    } else {
      res.status(400).json({ error: 'Code is existed'! });
    }
  }

  // we need to use an arrow function to bind `this` correctly
  validatePatchCourse = async (
    req: express.Request,
    res: express.Response,
    next: express.NextFunction) => {
    if (req.body.course) {
      log('Validating title course', req.body.course);
      if (req.body.course.code) this.validateSameCodeBelongToSameCourse(req, res, next);
    } else {
      next();
    }
  };

  async validateCourseExists(
    req: express.Request,
    res: express.Response,
    next: express.NextFunction) {
    const course = await coursesService.getById(req.params.id);
    if (course) {
      return next();
    } else {
      res.status(404).json({
        error: `Course ${req.params.id} not found`,
      });
    }
  }

  async extractCourseId(
    req: express.Request,
    res: express.Response,
    next: express.NextFunction) {
    const courseId = req.params.id;
    req.body._id = req.body.id = courseId;
    next();
  }

}

export default new CoursesMiddleware();