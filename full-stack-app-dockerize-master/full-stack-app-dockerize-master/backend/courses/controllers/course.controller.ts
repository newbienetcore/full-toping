import { PutCourseDto } from './../dto/put.course.dto';
import { PatchCourseDto } from './../dto/patch.course.dto';
import { CreateCourseDto } from './../dto/create.course.dto';
// import express to add types to the request/response objects from our controller functions
import express from 'express';

// import our services
import coursesService from './../services/courses.service';

// use debug with a custom context
import debug from 'debug';

const log: debug.IDebugger = debug('app:courses-controller');

class CoursesController {

  async listCourses(req: express.Request, res: express.Response) {
    const courses = await coursesService.list(200, 0);
    res.status(200).json(courses);
  }

  async getCourseById(req: express.Request, res: express.Response) {
    const courseId = req.params.courseId || req.body.id;
    const course = await coursesService.getById(courseId);
    res.status(200).json(course);
  }

  async createCourse(req: express.Request, res: express.Response) {
    let resource: CreateCourseDto = req.body;
    resource = await coursesService.create(resource);
    res.status(201).json(resource);
  }

  async patchCourse(req: express.Request, res: express.Response) {
    let resource: PatchCourseDto = req.body;
    resource = await coursesService.patchById(req.body.id, resource);
    log(resource);
    res.status(200).json(resource);
  }

  async putCourse(req: express.Request, res: express.Response) {
    let resource: PutCourseDto = req.body;
    resource = await coursesService.putById(req.params.id, resource)
    log(resource);
    res.status(200).json(resource);
  }

  async removeCourse(req: express.Request, res: express.Response) {
    const course = await coursesService.deleteById(req.body.id)
    log(course);
    res.status(200).json(course);
  }
}

export default new CoursesController();