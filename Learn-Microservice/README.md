## Project eCommerce Web API Backend using ASP.NET 7.0 Following Microservices

## Prepare environment
* Install dotnet core version in file `global.json`
* Jetbrains Rider Csharp 2023
* Docker Desktop

## How to run the project

Run command for build project
```Powershell
dotnet build
```

Go to folder contain file  `docker-compose`

1. Using docker-compose
```Powershell
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
```

## Application URLs - LOCAL Environment (Docker container):
- Product API: http://localhost:6002/api/products
- Customer API: http://localhost:6003/api/customers
- Customer API: http://localhost:6004/api/baskets

## Docker Application URLs - LOCAL Environment (Docker container):
- Portainer: http://localhost:9000 - username: admin ; pass: admin123456789
- Kibana: http://localhost:5601 - username: elastic ; pass: admin
- Rabbitmq: http://localhost:15672 - username: guest ; pass: guest

2. Using Jetbrains Rider Csharp 2023
- Open eCommerce.sln - `eCommerce.sln`
- Run Compound to start multi projects

---
## Application URLs - DEVELOPMENT Environment:
- Product API: http://localhost:5002/api/products
- Customer API: http://localhost:5003/api/customers
- Customer API: http://localhost:5004/api/baskets

---
## Application URLs - PRODUCTION Environment:

---

## Package References

## Install Environment
- https://dotnet.microsoft.com/dowload/dotnet/7.0
- https://www.jetbrains.com/rider/download/#section=windows

## Reference URLs

## Docker Commands
```Powershell
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
```




## Useful Commands
- ASPNETCORE_ENVIRONMENT=Production dotnet ef database update
- dotnet watch run --environment "Development"
- dotnet restore


- dotnet ef migrations add "init_order_db" --project Ordering.Infrastructure --startup-project Ordering.API --output-dir Persistence/Migrations
- dotnet ef database remove --project Ordering.Infrastructure --startup-project Ordering.API
- dotnet ef database update --project Ordering.Infrastructure --startup-project Ordering.API


