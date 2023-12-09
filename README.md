# KutCode.Security.Ldap

## Info
Web API microservice with only one purpose:  
Make LDAP auth integration simplier.

## License
Please read the simple [MIT license](./LICENSE).

## Stack
- Dotnet 8
- FastEndpoints
- Swagger
- Docker

## Installation
Use `docker compose` to setup container ease.  
Shure, you can produce manual installation with dotnet-runtime.

### Docker
#### Docker run
```bash
cd ./src/presentation/KutCode.Security.Ldap.WebApi
docker build -t ldap .
docker run -d -p 8080:80 -v ./appsettings:/app/appsettings -v ./logs:/apt/logs -e ASPNETCORE_URLS=http://+:80 ldap
```
#### Docker compose
From the solution root directory:
```bash
ls -la
# be sure that docker-compose.yml is presented
docker compose up -d
# if you docker has not 'compose' command, try write: docker-compose

# uncomment 'image' section if you use network registry 
  #image: registry.neftm.local/ldap

# leave 'context' section if you use local solution files 
  #build:
    #context: src/presentation/KutCode.Security.Ldap.WebApi
```
To check installation open in browser:  
`http://localhost:[your port]/swagger`