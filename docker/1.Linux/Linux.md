### Chạy Linux với Docker
- Các câu lệnh của Linux phân biệt chữ hoa, chữ thường

```
+ docker images: danh sách các images
+ docker rmi <image name (Repository)>

+ Docker pull {image name}: dowload image
+ Docker run {image name}: run image, nếu không có tự động dowload

+ docker ps: xem các container đang chạy
+ docker ps -a: xem tất cả các container
+ docker rm <id_container>: xoá container có id (chỉ cần 3 số đầu)

+ docker run -it ubuntu: vào ubuntu tương tác, nếu không có tự động dowload (exit: thoát)
```

```
+ docker run -it ubuntu: run image ubuntu, nếu ubuntu chưa có thì tự động dowload
=> root@4d0a1b37f575:/# 
=> root: mình đang đăng nhập vào ubuntu với cái username là root
=> 4d0a1b37f575: ID của container hiện đang chạy
=> /: mình đang đứng ở thư mục gốc
=> #: quyền user cao nhất, $ user bình thường

+ echo {message}: dùng để hiện thị nội dung ra màn hình
+ echo $0: hiện thị nội dung của shell (môi trường dòng lệnh) hiện đang được sử dụng, vị trí đang chạy các câu lệnh

+ whoami: hiển thị tên người dùng đang đăng nhập vào hệ thống hiện tại 
+ pwd: hiển thị thư mục hiện tại bạn đang đứng

+ ls: lấy ra tất cả các file, folder của thư mục hiện tại
+ ls -a: lấy tất cả file, folder của thư mục hiện tại và đang Hidden

+ history: danh sách các câu lệnh đã chạy (!2: chạy lại câu lệnh thứ 2)

+ clear: clear màn hình (Ctrl + L trên window, Ctrl + L trên Linux)

```

### 2.3 Linux Package Management

Advanced Package Tool là một công cụ quản lý gói (package manager tool) trong các hệ thống dựa trên Debian Linux như Ubuntu. Nó được sử dụng để quản lý việc cài đặt, cập nhập và gỡ bỏ các gói phần mềm trên hệ thống

```
* Để sử dụng các lệnh apt, bạn cần có quyền quản trị hệ thống và thường phải sử dụng sudo trước mỗi lệnh để đảm bảo có đủ quyền hạn.
+ apt: help
+ apt list: Danh sách package đã có trên máy 
+ apt install [package]: Cài đặt một gói phần mềm mới
+ apt remove [package]: Gỡ bỏ một gói phần mềm đã cài đặt
+ apt update: Cập nhập danh sách các gói phần mềm từ các nguồn (repositories) đã cấu hình trên hệ thống
+ apt upgrade: Cập nhập tất cả các gói phần mềm đã cài đặt trên phiên bản mới nhất.
+ apt search [keyword]: Tìm kiếm các gói phần mềm đã cài đặt lên phiên bản mới nhất.
+ apt show [package]: Hiển thị thông tin chi tiết về một gói phần mềm cụ thể.
+ apt autoremove: gỡ bỏ các gói phần mềm không còn phụ thuộc vào bất kì gói phần mềm nào khác trên hệ thống.
```

### 2.4 Linux file system
![2.4 Linux file system](../0.Images/Linux/linux-filesystem.png)

+ Trong Linux, tất cả đều là file
+ root (/) là cao nhất trong cây thư mục/hệ thống Linux
+ bin: binary or execute programs (nơi chứa các chương trình dưới dạng thực thi)
+ boot: booting process (một trong những thư mục quan trọng trong hệ thống tệp tin. Nó chứa các tập tin và dữ liệu liên quan đến quá trình khởi động (booting) của hệ thống)
+ dev: devices (chứa các tập tin đại diện cho các thiết bị (devices) vật lý và ảo trong hệ thống)
+ etc: các file cấu hình cho hệ thống
+ home: Nơi người dùng lưu trữ dữ liệu cá nhân

### 2.5: Navigating File System
Navigating File System là cách tổ chức và quản lý các tập tin và thư mục trong hệ điều hành của bạn. Điều này cho phép bạn lưu trữ và tổ chức dữ liệu của mình theo cách phù hợp.

```
+ pwd: Print Working Directory - Hiển thị thư mục hiện tại đang làm việc 

+ ls: List - Liệt kê danh sách các file
+ ls -1: Liệt kê danh sách các file và in ra từng dòng
+ ls -l: hiển thị chi tiết ...
+ ls -a: lấy ra tất cả các file, cả file ẩn

+ cd: Change directory - Thay đổi thư mục làm việc
+ cd -: Quay lại đến thư mục trước đó
+ cd ~: Di chuyển đến thư mục hôm người dùng hiện tại
+ cd ~[username]: Di chuyển đến thư mục home của người dùng cụ thể

+ mkdir [directory_name]: Tạo thư mục mới

+ mv [source] [destination]: Di chuyển hoặc đổi tên tập tin hoặc thư mục
=> mv file1.txt /path/to/new_directory

+ cp [source] [destination]: Sao chép tập tin hoặc thư mục
=> cp file1.txt /path/to/new_directory
=> cp -r /root/hung1.txt /root/hung: khi bạn sử dụng tùy chọn -r, lệnh cp sẽ sao chép tất cả các tệp và thư mục con của hung1.txt vào đích (hung).

+ rm [file_name]: Xoá tập tin hoặc thư mục
+ rm -r [directory_name]: xóa thư mục và nội dung của nó 
```

### 2.6: Manipulating Files and Directories (Thao tác tập tin và thư mục)
Di chuyển File System

```
+ mkdir: Making Directory - Tạo thư mục

+ mv [source] [destination]: Di chuyển hoặc đổi tên một tập tin hoặc thư mục
=> mv old_file.txt new_file.txt: đổi tên thư mục
=> mv file.txt /path/to/directory: di chuyển một tập tin vào một thư mục

+ cp [source] [destination]: Sao chép một tập tin hoặc thư mục
=> cp file.txt file_copy.txt: Sao chép tập tin
=> cp file.txt /path/to/directory: Sao chép một tập tin vào một thư mục

+ rm [file_name]: xoá tập tin hoặc thư mục
+ rm -r directory_name: xóa một thư mục và nội dung của nó

+ touch {file path/file name.x}: Tạo 1 file mới, hoặc nhiều file
=> touch colen.txt colen1.txt
```

### 2.7: Edit and View file (Chỉnh sửa và hiển thị file)

```
+ nano {filename.x}: View và Edit nội dung của file
+ cat {filename.x}: Xem nội dung file (toàn bộ content)
+ more {filename.x}: Xem content theo từng page (press space để load thêm, q to quit)

+ less {filename.x} (install using apt): xem content theo từng page (press up/down để load thêm, press q to quit)
+ head -n {number line} {path}: View từ số dòng đầu tiên của file
+ tail -n {number line} {path}: View từ số dòng đầu tiên của file

```

### 2.8: Redirection (Chuyển hướng)

```
+ command > file.txt : ( > ) Ghi đè (overwrite) nội dung ra tập tin.
+ cat hello-docker.txt > file-docker.txt : chuyển nội dung của file hello-docker.txt sang file-docker.txt
+ echo hello docker file-docker.txt

+ command >> Ghi thêm nội dung vào cuối file mà không làm mất nội dung của file

+ command 2> error.txt
=> command là lệnh bạn muốn thực thi
=> 2> : đây là phần lệnh redirection, chuyển hướng đầu ra lỗi chuẩn (stderr).
=> error.txt: Đây là tên của tệp tin mà đầu ra lỗi chuẩn sẽ được ghi vào
=> Lệnh này sẽ chạy "command" và chuyển hướng bất kì lỗi nào xuất hiện trong quá trình thực thi của nó vào tập tin "error.txt".

+ command &> output.txt
=> command: đây là lệnh bạn muốn thực thi
=> &>: Đây là phần lệnh redirection chuyển hướng cả đầu ra chuẩn (stdout) và lỗi chuẩn (stderr)
=> output.txt: Đây là tập tin mà đầu ra chuẩn và lỗi chuẩn sẽ được ghi vào
=> Lệnh này sẽ chạy command và chuyển hướng cả đầu ra chuẩn và đầu ra lỗi chuẩn vào tập tin output.txt. Điều này có nghĩa là cả kết quả đúng và lỗi sẽ được ghi vào cùng một tập tin

+ command < input.txt: Lệnh này sử dụng nội dung của tập tin input.txt làm đầu vào cho command

+ $(cat docker.txt): đọc nội dung file docker.txt và chạy nội dung đó "echo hello docker"

+ command1 | command2: Pipes (|) cho phép bạn kết hợp đầu ra của một lệnh với đầu vào của một lệnh khác. Điều này hữu ích khi bạn muốn chuyển dữ liệu trực tiếp từ một lệnh sang lệnh khác
=> ls | grep "hello-docker"
=> ls là lệnh để liệt kê các tệp tin trong thư mục hiện tại.
=> grep "example" là lệnh để tìm kiếm chuỗi "example" trong đầu vào được cung cấp cho nó.
=> Kết quả của lệnh "ls" sẽ chuyển đến lệnh 'grep', và 'grep' sẽ hiện thị ca dòng có chứa từ khoá "example". Điều này giúp bạn tìm kiếm các tệp tin có tên chứa từ khoá "hello-docker" trong thư mục hiện tại

```

### Bài 2.9: Searching (Tìm kiếm)

```
+ grep: là một công cụ tìm kiếm mạnh mẽ để tìm kiếm các dòng trong tệp tin hoặc đầu vào chuẩn (stdin) mà khớp với một biểu thức chính quy (regular expression)

=> grep hello hello-docker.txt
=> grep -i hello hello-docker.txt: (-i) không phân biệt chữ hoa thường
=> grep -i hello hello-docker*
=> grep -i root /etc/passwd
=> grep -i -r hello .: (.) thư mục hiện tại
```

### Bài 2.10: Finding

```
+ find: được sử dụng để tìm kiếm tệp tin và thư mục dựa trên nhiều tiêu chí khác nhau như tên, kích thước, quyền truy cập, v.v.

=> find /etc/ -type d: tìm thư mục trong thư mục /etc/
=> find /etc/ -type f: tìm file trong thư mục /etc/
=> find / -type f -name "f*": Tìm tất cả các file có tên bắt đầu bằng chữ "f" ở trong thư mục / (root)
=> find / -type f -i -name "f*": Không phân biệt chữ hoa, chữ thường
=> .....

```

### Bài 2.11: Chaining commands (Chuỗi lệnh)

```
1. Dấu chấm phẩy (';'): để liên kết các lệnh và thực thi chúng theo thứ tự. Các lệnh sẽ được chạy tuần tự từ trái sang phải.
=> command1 ; command2 ; command3

2. Toán tử logic && và ||
+ &&: lệnh sau chỉ được thực thi nếu lệnh trước thành công (trả về exit code 0)
+ ||: lệnh sau chỉ được thực thi nếu lệnh trước thất bại (trả về exit code khác 0)
=> command1 && command2 || command3

3. Toán tử | (pipe): cho phép bạn chuyển đầu ra của một lệnh thành đầu vào của một lệnh khác. Điều này hữu ích khi bạn muốn kết hợp kết quả của hai lệnh lại với nhau
=> command1 | command2

4. Toán tử > và >>
Toán tử > được sử dụng để chuyển hướng đầu ra của một lệnh sang một tập tin, trong khi >> được sử dụng để nối thêm đầu ra của một lệnh vào cuối tập tin

=> example:
 mkdir vip;\
> cd vip;\
> echo 'do chi hung'

```

### Bài 2.12: Environment variables

```
+ env hoặc printenv: hiển thị ra các biến môi trường (environment variables) trong hệ thống Linux/Unix

=> echo $PATH: hiển thị nội dung biến PATH
=> printenv PATH: hiển thị nội dung biến PATH

+ export VARIABLE_NAME=value: được sử dụng để xuất (export) biến môi trường, trong phiên làm việc hiện tại

+ Biến môi trường vĩnh viễn: Bạn có thể đặt biến môi trường để tồn tại vĩnh viễn bằng cách thêm nó vào file cấu hình như ~/.bashrc, ~/.bash_profile, hoặc /etc/profile.

=> echo DB_USER=dochihung >> ~/.bashrc : lưu biến môi trường vào file ~/.bashrc, lưu ý không lưu biến nhạy cảm vào file này

source  ~/.bashrc:  được sử dụng để thực thi nội dung của file ~/.bashrc trong shell hiện tại

```

### Bài 2.13: Quản lý processes
Quản lý quy trình (processes) là một phần quan trọng trong việc quản lý hệ thống Linux/Unix.Dưới đây là một số lệnh thường được sử dụng để quản lý các quy trình

```
1. ps - Hiển thị danh sách các quy trình đang chạy
+ ps aux: Hiển thị tất cả các quy trình trên hệ thống
+ ps aux | grep <process_name>: lọc danh sách quy trình bằng tên quy trình

2. top - Hiển thị danh sách quy trình đang chạy và sử dụng tài nguyên
+ top: Hiển thị danh sách các quy trình đang chạy và sử dụng tài nguyên như CPU, bộ nhớ, ... Cập nhập thông tin một cách liên tục

3. kill - Kết thúc một quy trình
+ kill <process_id>: Gửi tín hiệu kết thúc đến quy trình có ID là <process_id>
+ killall <process_name>: Kết thúc tất cả các quy trình có tên là <process_name>

4. pgrep và pkill - Tìm kiếm và kết thúc quy trình dựa trên tên:
+ pgrep <process_name>: Tìm kiếm và hiển thị ID của các quy trình có tên là <process_name>
+ pkill <process_name>: Kết thúc tất cả các quy trình dựa trên tên là <process_name>

5. killall - Kết thúc tất cả các quy trình dựa trên tên:
+ killall <process_name>: Kết thúc tất cả các quy trình có tên là <process_name>.

6. htop - Một công cụ quản lý quy trình tương tác:
+ htop: Hiển thị danh sách các quy trình đang chạy dưới dạng biểu đồ tương tác, giúp dễ dàng quản lý và giám sát các quy trình.

```

Các lệnh trên là một số trong những công cụ cơ bản và phổ biến được sử dụng để quản lý các quy trình trong hệ thống Linux/Unix

### Bài 2.14: Quản lý User
Quản lý người dùng (user management) là một phần quan trọng trong việc quản lý hệ thống Linux/Unix. Dưới đây là một số lệnh và công cụ thường được sử dụng để quản lý người dùng

```
1. adduser hoặc useradd - Thêm người dùng mới
+ adduser <username>: Tạo một người dùng mới với tên là <username> và hướng dẫn người dùng nhập thông tin cần thiết.
+ useradd <username>: Tạo một người dùng mới với tên là <username> và sử dụng các cài đặt mặc định.
=> useradd -m dochihung: tạo username dochihung và tạo thư mục home cho họ

2. passwd - Đặt mật khẩu cho người dùng
+ passwd <username>: Đặt mật khẩu cho người dùng có tên là <username>

3. usermod - Sửa đổi thông tin người dùng
+ usermod -aG <group_name> <username>: Thêm người dùng vào một nhóm.
+ usermod -l <new_username> <old_username>: Đổi tên người dùng từ <old_username> thành <new_username>.
+ usermod -s <shell> <username>: Đặt shell mặc định cho người dùng.

4. deluser hoặc userdel - Xóa người dùng
+ eluser <username>: Xóa người dùng và tất cả các dữ liệu liên quan đến họ.
+ userdel <username>: Xóa người dùng, nhưng không xóa các dữ liệu liên quan đến họ

5. id - Hiển thị thông tin về người dùng
+ id <username>: Hiển thị thông tin về người dùng, bao gồm cả UID (User ID) và GID (Group ID).


=> useradd -m {username}: Tạo mới 1 user và home directory

=> cat /etc/passwd: xem chi tiết tất cả users and groups
=> hung89hy:x:1000:1000::/home/john:/bin/sh
=> :x => password is stored somewhere else
=> :1000:1000 => user id:group id
=> :/bin/sh => login shell for the user account

=> usermod -s /bin/bash {username}: Đổi từ shell to bash for a user
=> cat /etc/shadow: xem tất cả passwords của các users (only root account)

=> docker exec -it -u {username} {container id} bash: execute command using user
=> userdel {username}: xóa user
=> adduser {username}: Tạo mới 1 user (nhưng phải nhập thông tin từng bước)

```

Quản lý người dùng là một phần quan trọng trong bảo quản và quản lý hệ thống Linux/Unix, đặc biệt khi bạn cần tạo, sửa đổi hoặc xoá người dùng, cũng như quản lý quyền tuy câp, an ninh


### Bài 2.15: Quản lý Group
Trong Linux có 2 nhóm chính là Primary Group (Nhóm chính) và Supplementary Groups (Nhóm Bổ Sung)

Primary Group (Nhóm Chính)
```
+ Nhóm chính là nhóm mặc định được gán cho mỗi người dùng khi tài khoản được tạo ra
+ Mỗi tệp và thư mục mà người dùng mà người dùng tạo ra sẽ được gán quyền của nhóm chính họ
+ Khi một người dùng tạo một tệp mới , nhóm của tệp đó sẽ là nhóm chính của người dùng đó
+ Thường thì, nhóm chính có cùng tên với tên người dùng tương ứng, nhưng điều này không bắt buộc
```

Supplementary Groups (Nhóm bổ sung)
```
+ Ngoài nhóm chính, người dùng cũng có thể thuộc vào nhiều nhóm bổ sung khác
+ Nhóm bổ sung cho phép người dùng truy cập vào các tệp và thư mục mà thuộc về các nhóm đó
+ Mỗi người dùng có thể thuộc vào một hoặc nhiều nhóm bổ sung 
+ Các nhóm bổ sung thường được sử dụng để chia sẻ quyền truy cập giữa các người dùng hoặc để quản lý quyền truy cập vào tệp và thư mục
```

Mối quan hệ giữa Primary Group và Supplementary Groups (Nhóm bổ sung) là
```
+ Nhóm chính thường là nhóm mặc định được sử dụng khi người dùng tạo các tệp và thư mục mới.
+ Nhóm bổ sung cung cấp quyền truy cập đến các tệp và thư mục mà thuộc về nhóm đó, bổ sung vào quyền của nhóm chính
```

```
+ groupadd <group_name>: Thêm nhóm mới

+ groupdel <group_name>: Xoá nhóm

+ groupmod -n <new_group_name> <old_group_name>: Đổi tên nhóm từ <old_group_name> sang <new_group_name>

+ gpasswd: Quản lý mật khẩu nhóm
=> gpasswd -a <username> <group_name>: Thêm một người dùng vào nhóm.
=> gpasswd -d <username> <group_name>: Xóa một người dùng khỏi nhóm.
=> gpasswd -r <group_name>: Xóa mật khẩu của nhóm.

+ id <group_name>: Hiển thị thông tin về nhom, bao gồm cả GID (GroupID) và danh sách user thuộc nhóm đó

+ cat /etc/group: xem danh sách các group
+ getent group {group_name} : xem các user thuộc về group developers
```

### Bài 2.16: Phân quyền cho file và directories

-rw-r--r-- 1 root root   29 Apr 15 18:58 deloy.sh
```
+ -rw-r--r--: Phần này mô tả quyền truy cập của tệp
=> rw-: người sở hữu ở đây là root có quyền có quyền đọc (r) và ghi (w) nhưng không có quyền thực thi (-)
=> r--: Nhóm người dùng ở đây là root chỉ có quyền đọc  (r), không có quyền ghi (w) hoặc thực thi (-)
=> r--: tất cả các người dùng khác cũng chỉ có quyền đọc (r), nhưng không có quyền ghi và thực thi
=> 1 : số lượng liên kết tới tệp hoặc thư mục
=> root root: Người sở hữu và nhóm người sở hữu của tệp.
=> 9 Apr 15 18:58: Thời gian và ngày tạo hoặc sửa đổi cuối cùng của tệp.
=> deloy.sh: Tên của tệp.

```


drwxr-xr-x 2 root root 4096 Apr 15 18:58 hello
```
=> drwxr-xr-x: Phần này mô tả quyền truy cập của thư mục. Tương tự như trên, nhưng với dấu d ở đầu để chỉ ra rằng nó là một thư mục.
=> 2: Số lượng liên kết tới thư mục.
=> root root: Người sở hữu và nhóm người sở hữu của thư mục.
=> 4096: Kích thước của thư mục.
=> Apr 15 18:58: Thời gian và ngày tạo hoặc sửa đổi cuối cùng của thư mục.
=> hello: Tên của thư mục.
```

Cách gán quyền truy cập cho tệp hoặc thư mục trong hệ thống Linux, bạn có thể sử dụng câu lệnh 'chmod'. Dưới đây là các sử dụng lệnh chmod để gán quyền truy cập
1. Sử dụng mã Octal (3 số)
2. Sử dụng kí hiệu Symbolic (u, g, o, +, -, =)
3. Sử dụng một kết hợp của cả hai

2. Sử dụng kí hiệu Symbolic (u, g, o, +, -, =)
```
+ u đại diện cho người sở hữu, g đại diện cho nhóm, và o đại diện cho các người dùng khác.
+ `+` để thêm quyền, - để loại bỏ quyền, và = để đặt quyền một cách tuyệt đối.
+ r là quyền đọc, w là quyền ghi, và x là quyền thực thi.
Ví dụ:
=> chmod u+x file.txt: Thêm quyền thực thi cho người sở hữu của file.txt.
=> chmod g-w file.txt: Loại bỏ quyền ghi cho nhóm.
```

