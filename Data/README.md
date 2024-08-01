# Thiết lập user cho OS(Ubuntu) 22.04
```sh
sudo adduser data
sudo usermod -aG sudo data
su - data
```
# Tải các gói cần thiết
```sh
> sudo apt -y install gcc make unzip ant openjdk-8-jdk-headless
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
# Giải nén TPC-H 3.0.1
```sh
> unzip TPC-H-3-0-1.zip
```
# Cấu hình TPC-H
```sh
-  TPC-H\ V3.0.1/dbgen/
> cp makefile.suite makefile

> nano makefile
CC      = gcc
DATABASE= ORACLE
MACHINE = LINUX
WORKLOAD = TPCH

> make
```
# Tạo dữ liệu
```sh
-  TPC-H\ V3.0.1/dbgen/
>./dbgen -s 10
Trong đó 10 là số GB tạo ra
```
