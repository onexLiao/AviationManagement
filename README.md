# Aviation Management API 



## How to run this API

* preparations

1. install docker
2. install dotnet core
3. install docker-compose ```pip install docker-compose```




* clone the repo

```bash
git clone https://github.com/jason-xuan/AviationManagement.git
```



* build the project

```shell
cd AviationManagement
dotnet restore ./AviationManagement.sln
dotnet publish ./AviationManagement.sln -c Release -o ./obj/Docker/publish
```



* docker options

```shell
sudo docker-compose up
```

