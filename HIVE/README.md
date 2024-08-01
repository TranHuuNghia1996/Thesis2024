# Thiết lập user cho OS(Ubuntu) 22.04
```sh
> sudo adduser hive
> sudo usermod -aG sudo hive
> su - hive
```

# Tải các gói cần thiết hadoop và hive
```sh
Tải hadoop tại  https://archive.apache.org/dist/hadoop/common/hadoop-3.3.6/hadoop-3.3.6-src.tar.gz
Giải nén gói hadoop 

>tar xzf hadoop-3.3.6.tar.gz

Tải HIVE tại  https://downloads.apache.org/hive/hive-3.1.3/apache-hive-3.1.3-bin.tar.gz
Giải nén gói hive tar 

>xzvf apache-hive-3.1.3-bin.tar.gz
```

# Thiết lập passwordless ssh (ubuntu)

```sh
>ssh-keygen
```

# Cập nhật biến môi trường HIVE_HOME và HADOOP trong ~/.bashrc
```sh
> nano ~/.bashrc

#HADOOP
export HADOOP_HOME=/home/hive/hadoop-3.3.6
export PATH=$PATH:$HADOOP_HOME/bin
export JAVA_HOME=/usr
export PATH=$PATH:$JAVA_HOME/bin
export PATH=$PATH:$HADOOP_HOME/bin
export PATH=$PATH:$HADOOP_HOME/sbin
export HADOOP_MAPRED_HOME=$HADOOP_HOME
export HADOOP_COMMON_HOME=$HADOOP_HOME
export HADOOP_HDFS_HOME=$HADOOP_HOME
export YARN_HOME=$HADOOP_HOME
export HADOOP_COMMON_LIB_NATIVE_DIR=$HADOOP_HOME/lib/native
export HADOOP_OPTS="-Djava.library.path=$HADOOP_HOME/lib"
export JAVA_HOME=/usr/lib/jvm/java-8-openjdk-amd64
export PATH=$PATH:$JAVA_HOME/bin

#hive
#export HIVE_HOME=/home/hive/apache-hive-3.1.3-src
export HIVE_HOME=/home/hive/apache-hive-3.1.3-bin
export PATH=$PATH:$HIVE_HOME/bin
```

# Cấu hình hadoop

```sh
core-site.xml:

<configuration>
	
	 <property>
        <name>fs.defaultFS</name>
        <value>hdfs://master:9000</value>
    </property>

    <property>
        <name>fs.defaultFS</name>
        <value>hdfs://master:9000</value>
    </property>
</configuration>

hdfs-site.xml:
<configuration>
    <property>
        <name>dfs.replication</name>
        <value>3</value>
    </property>
</configuration>

mapred-site.xml:
<configuration>
    <property>
        <name>mapreduce.framework.name</name>
        <value>yarn</value>
    </property>
</configuration>

yarn-site.xml:
<configuration>
    <property>
        <name>yarn.nodemanager.aux-services</name>
        <value>mapreduce_shuffle</value>
    </property>
    <property>
        <name>yarn.nodemanager.aux-services.mapreduce.shuffle.class</name>
        <value>org.apache.hadoop.mapred.ShuffleHandler</value>
    </property>
</configuration>

workers: Địa chỉ các máy workers

```

# Khởi động hadoop

```sh
>start-all.sh
```


# Khởi tạo cơ sở dữ liệu Derby (cơ sở dữ liệu mặc định của Hive)

```sh
> schematool -initSchema -dbType derby
```

# khởi động hive

```sh
> HIVE
```

