import { CommonRoutesConfig } from './../common/common.routes.config';
import express from 'express';
import CoursesController from './controllers/course.controller';
import CoursesMiddleware from './middleware/courses.middleware';
import BodyValidationMiddleware from './../common/middleware/body.validation.middleware';
import ObjectIdValidationMiddleware from '../common/middleware/objectId.validation.middleware';

export class CoursesRoutes extends CommonRoutesConfig {
  constructor(app: express.Application) {
    super(app, 'CoursesRoutes');
  }

  configureRoutes(): express.Application {
    this.app.route('/api/courses')
      .get(CoursesController.listCourses)
      .post(
        BodyValidationMiddleware.verifyBodyFieldsErrors,
        CoursesMiddleware.validateSameCodeDoesntExist,
        CoursesController.createCourse
      );

    this.app.param(`id`,
      CoursesMiddleware.extractCourseId,
    );

    this.app.route(`/api/courses/:id`)
      .all([
        ObjectIdValidationMiddleware.verifyObjectId,
        CoursesMiddleware.validateCourseExists,
      ])
      .get(CoursesController.getCourseById)
      .delete(CoursesController.removeCourse);

    this.app.put(`/api/courses/:id`, [
      BodyValidationMiddleware.verifyBodyFieldsErrors,
      CoursesMiddleware.validateSameCodeBelongToSameCourse,
      CoursesController.putCourse
    ]);

    this.app.patch(`/api/courses/:id`, [
      CoursesMiddleware.validatePatchCourse,
      CoursesController.patchCourse,
    ]);

    return this.app;
  }
}