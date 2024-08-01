# Hệ điều hành sử dụng: Ubuntu 23.10

# Cài đặt gói cần thiết
```
sudo apt install curl wget tar mysql-client-core-8.0  -y 
```

# Cài Đặt và Cấu Hình TiUP
```
curl --proto '=https' --tlsv1.2 -sSf https://tiup-mirrors.pingcap.com/install.sh | sh
export PATH=$PATH:~/.tiup/bin
```
# Triển Khai Cụm TiDB
Tạo một file cấu hình YAML topology.yaml
```
global:
  user: "tidb"
  ssh_port: 22
  deploy_dir: "/tidb-deploy"
  data_dir: "/tidb-data"

pd_servers:
  - host: ip-pd_servers
    client_port: 2379
    peer_port: 2380
  - host: ip-pd_servers
    client_port: 2379
    peer_port: 2380
  - host: ip-pd_servers
    client_port: 2379
    peer_port: 2380

tidb_servers:
  - host: ip-tidb_servers
    port: 4000
    status_port: 10080
  - host: ip-tidb_servers
    port: 4000
    status_port: 10080
  - host: ip-tidb_servers
    port: 4000
    status_port: 10080

tikv_servers:
  - host: ip-tikv_servers
    port: 20160
    status_port: 20180
  - host: ip-tikv_servers
    port: 20160
    status_port: 20180

monitoring_servers:
  - host: ip-monitoring_servers
    port: 9090

grafana_servers:
  - host: ip-monitoring_servers
    port: 3000

alertmanager_servers:
  - host: ip-alertmanager_servers
    web_port: 9093
    cluster_port: 9094
```
# Triển Khai Cụm
```
tiup cluster deploy tidb-cluster v7.6.0 ./topology.yaml --user tidb
```
# khởi động cụm
```
tiup cluster start tidb-cluster --init
```
# Kết Nối với Cơ Sở Dữ liệu TiDB
```
mysql -h ip-tidb_servers -P 4000 -u root -p
```
