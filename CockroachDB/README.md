# Thiết lập user cho OS(Ubuntu) 22.04
#  Chuẩn Bị Môi Trường
```
> sudo apt-get update && sudo apt-get upgrade -y
> sudo apt-get install curl wget -y
```
# Tải và Cài Đặt CockroachDB
```
> wget -qO- https://binaries.cockroachdb.com/cockroach-v21.1.7.linux-amd64.tgz | tar xvz
> sudo cp -i cockroach-v21.1.7.linux-amd64/cockroach /usr/local/bin/
```
# Khởi Tạo và Cấu Hình Cụm CockroachDB
Trên máy Master khởi tạo cụm CockroachDB:
```
> cockroach start-single-node --insecure --advertise-addr=ip-master --listen-addr=ip-master --http-addr=ip-master:8080 --background
```
Trên mỗi Node, tham gia vào cụm bằng cách chạy:
```
cockroach start --insecure --store=cockroach-data --listen-addr=ip-node --http-addr=ip-node:8080 --join=ip-node --background
```
# Chạy CockroachDB
```
> cockroach sql --insecure --host=ip-master
```
