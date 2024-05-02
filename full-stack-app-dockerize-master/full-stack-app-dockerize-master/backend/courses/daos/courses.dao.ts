import { CreateCourseDto } from '../dto/create.course.dto';
import { PatchCourseDto } from '../dto/patch.course.dto';
import { PutCourseDto } from '../dto/put.course.dto';
import debug from 'debug';
import mongooseService from './../../common/services/mongoose.service';

const log: debug.IDebugger = debug('app:courses-dao');

class CoursesDao {

  Schema = mongooseService.getMongoose().Schema;

  courseSchema = new this.Schema({
    code: { type: String, trim: true, required: true, unique: true },
    title: { type: String, trim: true, required: true },
    description: { type: String, trim: true, default: '' },
    content: {
      type: String,
      trim: true,
      required: true,
      minlength: 5,
    },
    price: { type: Number, default: 0, min: 0 },
    releasedDate: {
      type: Date,
      required: true,
      default: Date.now
    },
  });

  Course = mongooseService.getMongoose().model('Courses', this.courseSchema);

  constructor() {
    log('New instance of CoursesDao has created!');
  }

  async addCourse(courseFields: CreateCourseDto) {
    const course = new this.Course({
      ...courseFields,
    });
    await course.save();
    return course;
  }

  async getCourseById(courseId: string) {
    return await this.Course.findById(courseId).populate('Course').exec();
  }

  async getCourseByCode(code: string) {
    return this.Course.findOne({ code: code }).exec();
  }

  async getCourses(limit = 25, page = 0) {
    return this.Course.find()
      .limit(limit)
      .skip(limit * page)
      .exec();
  }

  async updateCourseById(
    courseId: string,
    courseFields: PatchCourseDto | PutCourseDto) {
    const existingCourse = await this.Course.findOneAndUpdate(
      { _id: courseId },
      { $set: courseFields },
      { new: true }
    ).exec();

    return existingCourse;
  }

  async removeCourseById(courseId: string) {
    return this.Course.findByIdAndDelete(courseId).exec();
  }
}

export default new CoursesDao();