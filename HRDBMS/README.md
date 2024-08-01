# Thiết lập user cho OS(Ubuntu) 22.04
```sh
sudo adduser hrdbms
sudo usermod -aG sudo hrdbms
su - hrdbms
```
# Tải và cài đặt các gói cần thiết
```sh
> sudo apt -y install gcc git make unzip ant openjdk-8-jdk-headless
Các phiên bản sử dụng
gcc:
  Installed: 4:9.3.0-1ubuntu2
git:
  Installed: 1:2.25.1-1ubuntu3.11
make:
  Installed: 4.2.1-1.2
unzip:
  Installed: 6.0-25ubuntu1.1
ant:
  Installed: 1.10.7-1
openjdk-8-jdk-headless:
  Installed: 8u382-ga-1~20.04.1
```
# Thiết lập passwordless ssh (ubuntu)
```sh
ssh-keygen 
```

# Tải và cài đăt HRDBMS
```sh
> mkdir HRDBMS
> cp -r  tranhuunghia\HRDBMS HRDBMS
> wget https://repo.maven.apache.org/maven2/org/apache/ivy/ivy/2.1.0/ivy-2.1.0.jar -P /home/hrdbms/HRDBMS/HRDBMS/lib/
> mv /home/hrdbms/HRDBMS/HRDBMS/lib/ivy-2.1.0.jar /home/hrdbms/HRDBMS/HRDBMS/lib/ivy.jar
> ant createJar
```
# Cấu hình HRDBMS
```sh
hparms
data_directories=/home/hrdbms/HRDBMS/HRDBMS/data1,/home/hrdbms/HRDBMS/HRDBMS/data2,/home/hrdbms/HRDBMS/HRDBMS/data3
log_dir=/home/hrdbms/HRDBMS/HRDBMS/log_dir
temp_directories=/home/hrdbms/HRDBMS/HRDBMS/temp1,/home/hrdbms/HRDBMS/HRDBMS/temp2
archive_dir=/home/hrdbms/HRDBMS/HRDBMS/archive
java_path=/usr/lib/jvm/java-8-oracle/bin
max_open_files=65536
Xmx_string=10g
jvm_args=-XX:+UseG1GC -XX:G1HeapRegionSize=2m -XX:+ParallelRefProcEnabled -XX:MaxDirectMemorySize=600000000 -XX:+AggressiveOpts -XX:CompileThreshold=200 -Xbatch -XX:-TieredCompilation
worker_debug_jvm_args=-agentlib:jdwp=transport=dt_socket,server=y,suspend=n,address=5015
coordinator_debug_jvm_args=-agentlib:jdwp=transport=dt_socket,server=y,suspend=n,address=5010
package_classpath=/home/hrdbms/HRDBMS/HRDBMS/build/HRDBMS.jar
ssh_args=-o StrictHostKeyChecking=no
pbpe_version=2
max_pbpe_time=60000

nodes.cfg
Thông tin các máy worker
```
# Khởi động HRDBMS
```sh
Coordinator
> java -Xms4g -Xmx8g  -classpath /home/hrdbms/HRDBMS/HRDBMS/build/HRDBMS.jar: com.exascale.managers.HRDBMSWorker 0 &
Worker
> java -Xms4g -Xmx8g  -classpath /home/hrdbms/HRDBMS/HRDBMS/build/HRDBMS.jar: com.exascale.managers.HRDBMSWorker 2 &
```
# Truy cập vào HRDBMS từ phía client
```sh
ssh tới máy coordinator
> connect to jdbc:hrdbms://<ip coordinator>:3232
```
