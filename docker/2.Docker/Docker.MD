## Introduce docker

+ Docker files
+ Versioning
+ Sharing
+ Savinf and Loading
+ Reducing
+ Speeding


## 3.1 Học về docker image và containers

### Docker image
```
- Một phiên bản OS đã được tối giản (cut-down)
- Cài đặt các third-party libraries (node, git, angular-cli, ...)
- Chứa các tập tin ứng dụng (Application files)
- Các biến môi trường (Environment variavles)
- Tính bất biến (Immutability)
```

Container
```
+ Có thể hiểu image là class, còn container là những instance được tạo ra, class có thể tạo ra mà không cần container, còn container để tạo ra cần phải có image
+ Cung cấp một môi trường tách biệt hoàn toàn để thực thi ứng dụng
+ Do nó hoạt đông như một máy ảo nên có thể ngừng (stopped) và khởi động lại (restarted)
+ Có thể hiểu container tương tự như một process trong hệ điều hành
```

## 3.4 Tìm hiểu Dockerfile
### Dockerfile là gì?

Dockerfile là một file dạng text không có phần đuôi mở dộng, chứ các đặc tả về một môi trường thực thi phần mềm, cấu trúc cho Docker Image. Từ những câu lệnh đó, Docker sẽ build ra Docker Image (thường có dung lượng nhỏ từ vài MD đến lớn vài GB)


### Cú pháp của một file Dockerfile
Cú pháp chung của một Dockerfile có dạng
```
Cú pháp: INSTRUCTION arguments
+ INSTRUCTION là các tên chỉ thị có trong Dockerfile, mỗi chỉ thị thực hiện một nhiệm vụ nhất định, được Docker quy định. Khi khai báo các chỉ thị này phải được viết bằng chữ IN HOA.
+ Một Dockerfile bắt buộc phải bắt đầu bằng chỉ thị FROM để khai báo đâu là image sẽ được sử dụng làm nền để xây dựng lên image của bạn.
+ aguments là phần nội dung của các chỉ thị, quyết định chỉ thị sẽ làm gì.
```

Ví dụ: 
```
FROM alpine:3.4

RUN apk update && \
    apk add curl && \
    apk add git && \
    apk add vim
```

### Các chỉ thị chính trong Dockerfile

#### FROM

+ Chỉ định rằng image nào sẽ được dùng làm image cơ sở để quá trình build image thực thiện các câu lệnh tiếp theo. Các image base này sẽ được tải về từ Public Repository hoặc Private Repository riêng của mỗi người tùy theo setup.

+ Cú pháp:
```
FROM <image> [AS <name>]
FROM <image>[:<tag>] [AS <name>]
FROM <image>[@<digest>] [AS <name>]
```

+ Chỉ thị FROM là bắt buộc và phải được để lên phía trên cùng của Dockerfile.


#### LABEL
+ Chỉ thị LABEL được dùng để thêm các thông tin meta vào Docker Image khi chúng được build. Chúng tồn tại dưới dạng các cặp key - value, được lưu trữ dưới dạng chuỗi. Có thể chỉ định nhiều label cho một Docker Image, và tất nhiên mỗi cặp key - value phải là duy nhất. Nếu cùng một key mà được khai báo nhiều giá trị (value) thì giá trị được khai báo gần đây nhất sẽ ghi đè lên giá trị trước đó.

+ Cú pháp:
```
LABEL <key>=<value> <key>=<value> <key>=<value> ... <key>=<value> 
```

+ Bạn có thể khai báo metadata cho Image theo từng dòng chỉ thị hoặc có thể tách ra khai báo thành từng dòng riêng biệt.

+ Ví dụ: 
```
LABEL com.example.some-label="lorem"
LABEL version="2.0" description="consectetur adipiscing elit."
```

+ Để xem thông tin meta của một Docker Image, ta sử dụng dòng lệnh:
```
docker inspect <image id>
```

#### MAINTAINER
#### RUN
+ Chỉ thị RUN dùng để chạy một lệnh nào đó trong quá trình build image và thường là các câu lệnh Linux. Tùy vào image gốc được khai báo trong phần FROM thì sẽ có các câu lệnh tương ứng. Ví dụ, để chạy câu lệnh update đối với Ubuntu sẽ là RUN apt-get update -y còn đối với CentOS thì sẽ là Run yum update -y. Kết quả của câu lệnh sẽ được commit lại, kết quả commit đó sẽ được sử dụng trong bước tiếp theo của Dockerfile.

+ Cú pháp:
```
RUN <command>
RUN ["executable", "param1", "param2"]
```

+ Ví dụ: 
```
RUN /bin/bash -c 'source $HOME/.bashrc; echo $HOME'
-------- hoặc --------
RUN ["/bin/bash", "-c", "echo hello"]
```

+ Ở cách thức shell form bạn có thể thực hiện nhiều câu lệnh cùng một lúc với dấu `\`:

```
FROM ubuntu
RUN apt-get update
RUN apt-get install curl -y

hoặc

FROM ubuntu
RUN apt-get update; \
    apt-get install curl -y
```

#### ADD
+ Chỉ thị ADD sẽ thực hiện sao chép các tập, thư mục từ máy đang build hoặc remote file URLs từ src và thêm chúng vào filesystem của image dest.

+ Cú pháp:
```
ADD [--chown=<user>:<group>] <src>... <dest>
ADD [--chown=<user>:<group>] ["<src>",... "<dest>"]
```
+ Trong đó:
    - src có thể khai báo nhiều file, thư mục, ...
    - dest phải là đường dẫn tuyệt đối hoặc có quan hệ chỉ thị đối với WORKDIR

+ Ví dụ: 
```
ADD hom* /mydir/
ADD hom?.txt /mydir/
ADD test.txt relativeDir/
```

+ Bạn cũng có thể phân quyền vào các file/thư mục mới được copy:
```
ADD --chown=55:mygroup files* /somedir/
ADD --chown=bin files* /somedir/
ADD --chown=1 files* /somedir/
ADD --chown=10:11 files* /somedir/

```


#### COPY
+ Chỉ thị COPY cũng giống chỉ thị ADD là copy file, thư mục từ src và thêm chúng vào dest của container. Khác với ADD, nó không hỗ trợ thêm các file remote file URLs từ các nguồn trên mạng.

+ Cú pháp:
```
COPY [--chown=<user>:<group>] <src>... <dest>
COPY [--chown=<user>:<group>] ["<src>",... "<dest>"]
```

#### ENV
+ Chỉ thị ENV dùng để khai báo các biến môi trường. Các biến này được khai báo dưới dạng key - value bằng các chuỗi. Giá trị của các biến này sẽ có hiện hữu cho các chỉ thị tiếp theo của Dockerfile.

+ Cú pháp:
```
ENV <key>=<value> ...
```
+ Ví dụ: 
```
ENV DOMAIN="open.school"
ENV PORT=80
ENV USERNAME="dochihung" PASSWORD="hung89hy1"
```

+ ENV chỉ được sử dụng trong các command sau:
    + ADD
    + COPY
    + ENV
    + EXPOSE
    + FROM
    + LABEL
    + STOPSIGNAL
    + USER
    + VOLUME
    + WORKDIR

#### CMD
+ Chỉ thị CMD định nghĩa các câu lệnh sẽ được chạy sau khi container được khởi động từ image đã build. Có thể khai báo được nhiều nhưng chỉ có duy nhất CMD cuối cùng được chạy.

+ Cú pháp:
```
CMD ["executable","param1","param2"]
CMD ["param1","param2"] 
CMD command param1 param2
```
+ Ví dụ: 
```
FROM ubuntu
CMD echo Viblo
```

#### USER
+ Có tác dụng set username hoặc UID để sử dụng khi chạy image và khi chạy các lệnh có trong RUN, CMD, ENTRYPOINT sau nó.

+ Cú pháp:
```
USER <user>[:<group>]
hoặc
USER <UID>[:<GID>]
```
+ Ví dụ: 
```
FROM alpine:3.4
RUN useradd -ms /bin/bash namdh
USER namdh
```
#### EXPOSE
#### STOPSIGNAL
#### VOLUME
#### WORKDIR
#### ENTRYPOINT 


## Bài 3.13: Removing images

+ “docker image prune”: Clear all dangling image - Xoá tất cả các image treo không dõ nguồn gốc, đang stopped
+ “docker container prune”: Clear all dangling container - Xoá tất cả các container treo không rõ nguồn gốc
+ “docker image rm {image name:tag/image id} {image name:tag/image id}”
+ “docker image remove {image name:tag}”: remove image với tag cụ thể

## Bài 3.14: Tagging images