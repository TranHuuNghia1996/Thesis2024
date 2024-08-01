# Hệ điều hành sử dụng: CentOS 8
```sh
adduser gpadmin
sudo usermod -aG sudo gpadmin
su - gpadmin
```
# Tải các gói cần thiết
```sh
> sudo yum install -y apr wget nano apr-util bzip2 krb5-devel libcgroup-tools net-tools perl zip
Các phiên bản sử dụng
apr
  Installed:  1.6.3-12.el8
apr-util
  Installed: 1.6.1-9.el8         
bzip2
 Installed: 1.0.6-26.el8                                                    
krb5-devel
  Installed: 1.18.2-25.el8                                                 
libcgroup-tools.x86_64
  Installed: 0.41-19.el8                                                       
net-tools.x86_64
  Installed: 2.0-0.52.20160912git.el8                                        
perl.x86_64
  Installed: 4:5.26.3-422.el8                                                  
zip.x86_64
  Installed: 3.0-23.el8
nano
  Installed: 2.9.8-1.el8.x86_64
wget
Installed: 1.19.5-11.el8
                                                     
```
# Tải gói và cài đặt Greenplum
```sh
> wget https://github.com/greenplum-db/gpdb/releases/download/6.25.1/open-source-greenplum-db-6.25.1-rhel8-x86_64.rpm
> sudo rpm -ivh open-source-greenplum-db-6.25.1-rhel8-x86_64.rpm
```
# Thiết lập passwordless ssh (CentOS)
```sh
ssh-keygen
```
# Cấu hình greenplum
```sh

Tạo file hostfile_gpinitsystem và hostfile_gpinitsystem
hostfile_gpinitsystem: chứa các ip woker và coordiantor
gpinitsystem_config: cấu hình greenplum
> source /usr/local/greenplum-db-6.25.1/greenplum_path.sh
> cp $GPHOME/docs/cli_help/gpconfigs/gpinitsystem_config .
  ARRAY_NAME="Greenplum Data Platform"
  SEG_PREFIX=gpseg
  PORT_BASE=6000
  declare -a DATA_DIRECTORY=(/home/gpadmin/data1/primary /home/gpadmin/data1/primary /home/gpadmin/data1/primary /home/gpadmin/data2/primary /hom$
  MASTER_HOSTNAME=master
  MASTER_DIRECTORY=/home/gpadmin/data/master
  MASTER_PORT=5432
  TRUSTED_SHELL=ssh
  CHECK_POINT_SEGMENTS=8
  ENCODING=UNICODE
  DATABASE_NAME=Data1
  MACHINE_LIST_FILE=/home/gpadmin/hostfile_gpinitsystem
  > gpinitsystem -c gpinitsystem_config
> cp $GPHOME/docs/cli_help/gpconfigs/hostfile_gpinitsystem .
  > gpssh-exkeys -f  hostfile_gpinitsystem

```
Thêm đường dẩn của MASTER_DATA_DIRECTORY vào /etc/enviroment
```
MASTER_DATA_DIRECTORY="/home/gpadmin/data/master/gpseg-1"
```
> export MASTER_DATA_DIRECTORY="/home/gpadmin/data/master/gpseg-1"


# Truy cập vào greenplum 
```sh
 > gpstart -a
 > psql postgres
```
