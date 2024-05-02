import { Db } from 'mongodb'
import { MigrationInterface } from 'mongo-migrate-ts';
import debug from 'debug';
const log: debug.IDebugger = debug('app:migrations-courses');
export class courses1624168366497 implements MigrationInterface {
  private collection = 'courses';

  public async up(db: Db): Promise<any> {
    log('Migrating: courses');
    const existCourse = await db.collection(this.collection).find().count();
    if (existCourse > 0) return;

    await db.collection(this.collection)
      .insertMany([
        {
          code: 'TEDU-37',
          title: 'Triển khai CI/CD với Azure DevOps',
          description: 'Continuous Integration (CI) Continuous Deployment (CD) Pipelines Azure DevOps',
          content: '<p>DevOps (Development and Operations), là 2 giai đoạn phát triển phần mềm mà cho dù bạn là 1 Junior hay Senior ít nhiều cũng sẽ tham gia. Khái niệm DevOps dù chỉ mới ra đời, nhưng đã gây được sự chú ý và quan tâm của rất nhiều các công ty dù lớn hay nhỏ, vì rõ ràng với DevOps, các công ty có thể realease các tính năng rất nhanh và nhanh chóng nhận được sự phản hồi để từ đó có những kế hoạch chỉnh sửa, nâng cấp phù hợp. Cùng với phương pháp Agile trong quy trình phát triển phần mềm, DevOps là một thành phần cực kỳ quan trọng giúp chu trình phát triển phần mềm tiến hoá từ mô hình Waterfall (thác nước) trước đây vốn có nhiều bất cập sang mô hình phát triển (CI - Continuous Integration) và phát hành (CD-Continuous Deployment) liên tục.</p>',
          price: 1500000,
          releasedDate: new Date('05/22/2021')
        },
        {
          code: 'TEDU-42',
          title: 'Làm chủ Docker để chinh phục DevOps',
          description: 'Docker là một nền tảng để xây dựng (build), chạy (running) và vận chuyển (shipping) các ứng dụng một cách dễ dàng và linh hoạt. Đó là lý do tại sao hầu hết các công ty sử dụng nó và đang tìm kiếm các kỹ sư phần mềm hoặc DevOps có kỹ năng Docker.',
          content: '<p>Docker là một nền tảng để xây dựng (build), chạy (running) và vận chuyển (shipping) các ứng dụng một cách dễ dàng và linh hoạt. Đó là lý do tại sao hầu hết các công ty sử dụng nó và đang tìm kiếm các kỹ sư phần mềm hoặc DevOps có kỹ năng Docker.</p> </p>Hiểu được điều đó, Tedu và giảng viên Kiệt Phạm đã mang đến cho các bạn một khóa học toàn diện và mang tính thực tiễn cao về Docker. Bạn sẽ được giới thiệu và tìm hiểu mọi thứ về Docker từ những khái niệm cơ bản cho đến nâng cao, những kiến thức cần thiết và bổ ích nhất (đã được sàng lọc qua kinh nghiệm làm dự án thực tế). Bạn sẽ cùng giảng viên xây dựng 1 dự án web full-stack, kết nối trực tiếp với cơ sở dữ liệu và các tests tự động chạy trên cloud.</p>',
          price: 2400000,
          releasedDate: new Date('12/12/2020')
        },
      ])
      .catch(err => console.error(err));
  }

  public async down(db: Db): Promise<any> {
    await db.collection(this.collection).deleteMany({
      code: {
        $in: ["TEDU-42", "TEDU-37"]
      }
    });
  }
}
