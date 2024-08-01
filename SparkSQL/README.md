# Thiết lập user cho OS(Ubuntu) 22.04
```sh
sudo adduser sparksql
sudo usermod -aG sudo sparksql
su - sparksql
```
# Tải gói cài đặt
```sh
>sudo apt-get install maven 
>wget https://archive.apache.org/dist/spark/spark-3.4.1/spark-3.4.1.tgz
Phiên bản sử dụng
maven:
  Installed: 3.6.3-1
```
# Cài đặt biến môi trường
```sh
export SPARK_HOME=/home/sparksql/spark-3.4.1
export PATH=$PATH:$SPARK_HOME/bin:$SPARK_HOME/sbin
```
# Thiết lập passwordless ssh (ubuntu)
```sh
ssh-keygen
```
# Cập nhật biến môi trường SPARK_HOME trong ~/.bashrc
```sh
> nano ~/.bashrc
export SPARK_HOME=/home/sparksql/spark-3.4.1
export PATH=$PATH:$SPARK_HOME/bin:$SPARK_HOME/sbin
> source ~/.bashrc
```
# Cấu hình SPARK
```sh
nano $SPARK_HOME/conf/spark-env.sh
export SPARK_MASTER_HOST=<ip master>
export SPARK_WORKER_MEMORY=2g  
export SPARK_EXECUTOR_MEMORY=2g
export SPARK_DRIVER_MEMORY=2g
export SPARK_WORKER_CORES=4
> nano workers
Địa chỉ ip các máy worker
```
# Xây dựng Spark với Hive và Hive Thrift Server
```sh
>build/mvn -Phive -Phive-thriftserver -DskipTests clean package
```
# Truy cập SparkSQL
```sh
> spark-sql
```
