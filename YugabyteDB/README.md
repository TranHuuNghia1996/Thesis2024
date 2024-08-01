# Thiết lập user cho OS(Ubuntu) 22.04

# Tải và cài đặt
```
wget https://downloads.yugabyte.com/releases/2.20.1.3/yugabyte-2.20.1.3-b3-linux-x86_64.tar.gz
tar xvfz yugabyte-2.20.1.3-b3-linux-x86_64.tar.gz && cd yugabyte-2.20.1.3/
./bin/post_install.sh
```

# Khởi động master server
```
./bin/yb-master \
--master_addresses=ip-master:7100 \
--rpc_bind_addresses=ip-master:7100 \
--fs_data_dirs=/home/yugabytedb/yb-master-data \
--replication_factor=1 &
```

# Khởi động tablet server
```
./bin/yb-tserver \
--tserver_master_addrs=ip-master:7100 \
--rpc_bind_addresses=ip-tabletserver:9100 \
--fs_data_dirs=/home/yugabytedb/yb-tserver-data \
--start_pgsql_proxy
```

# Khởi động YugabyteDB
```
./bin/ysqlsh -h ip-master -p 5433
```
