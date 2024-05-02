import { CourseDto } from '../dto/course.dto';
import { CRUD } from '../../common/interfaces/crud.interface';
import { CreateCourseDto } from '../dto/create.course.dto';
import { PutCourseDto } from '../dto/put.course.dto';
import { PatchCourseDto } from '../dto/patch.course.dto';
import CourseDao from '../daos/courses.dao';
import debug from 'debug';

const log: debug.IDebugger = debug('app:course-service');

class CoursesService implements CRUD<CourseDto, CreateCourseDto, PutCourseDto, PatchCourseDto> {

  async create(resource: CreateCourseDto) {
    log('creating course', resource);
    const course = await CourseDao.addCourse(resource);
    return course;
  }

  async deleteById(id: string) {
    log(`delete course by id: ${id}`);
    return CourseDao.removeCourseById(id);
  }

  async list(limit: number, page: number) {
    log(`list courses with limit: ${limit}, page: ${page}`);
    return CourseDao.getCourses(limit, page);
  }

  async patchById(id: string, resource: PatchCourseDto) {
    log('patch course', resource);
    return CourseDao.updateCourseById(id, resource);
  }

  async getById(id: string) {
    log('read course by id', id);
    return CourseDao.getCourseById(id);
  }

  async putById(id: string, resource: PutCourseDto) {
    log('put course', resource);
    return CourseDao.updateCourseById(id, resource);
  }

  async getCourseByCode(code: string) {
    log('get course by code', code);
    return CourseDao.getCourseByCode(code);
  }
}

export default new CoursesService();