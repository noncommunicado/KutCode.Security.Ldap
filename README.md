# KutCode.Security.Ldap

## ℹ️ Info
Web API microservice with only one purpose:  
Make LDAP auth integration simplier.

## 🧑‍⚖️ License
Please read the simple [MIT license](./LICENSE).

## 📜 Stack
- Dotnet 8
- FastEndpoints
- Swagger
- Docker

## 📦 Installation
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
# if your docker hasn't 'compose' command, try: 'docker-compose'

# uncomment 'image' section if you use network registry  
  #image: registry.neftm.local/ldap  

# leave 'context' section if you use local solution files  
  #build:  
    #context: src/presentation/KutCode.Security.Ldap.WebApi  
```
`docker-compose.yml` file example:
```bash 
services: 
    webapi:
        #image: registry.domain.local/ldap
        build:
            # set path to source code project directory
            context: src/presentation/KutCode.Security.Ldap.WebApi
        ports: 
            - 9080:80
        environment:
            ASPNETCORE_ENVIRONMENT: Production
            ASPNETCORE_URLS: http://+:80
        volumes:
          - ./appsettings:/app/appsettings
          - ./logs:/app/logs
```

#### Verify installation
To check installation open in browser:  
`http://localhost:[your port]/swagger`



## ⚙️ Configuration
### Application settings configuration
In application root `/appsettings` directory create `appsettings.json` file with following content:
```json
{
  "Culture": "en",
  "Ldap": {
    "Server": "dc01.examplpe.local",
    "ServerPort": 389,
    "DomainName": "examplpe.local",
    "BaseLdapFilter": "DC=examplpe,DC=local",
    "AdditionalLdapFilter": "&(objectClass=user)(objectClass=person)",
    "LoginAttribute": "sAMAccountName",
    "DisplayNameAttribute": "displayName",
    "UseSsl": false
  },
  "Cors": {
    "Origins": [
      "localhost", "some-one-else.com"
    ]
  }
}
```
Here some information about this settings:
- `Culture` - language of validation messages
- `Ldap`
  - `Server` - LDAP server name or ip-address
  - `ServerPort` - LDAP server port, 389 is default non-ssl LDAP port 
  - `DomainName` - Domain name of LDAP instance
  - `BaseLdapFilter` - LDAP base filter for user search
  - `AdditionalLdapFilter` - LDAP additional filter for user search
  - `LoginAttribute` - LDAP login attribute
  - `DisplayNameAttribute` - LDAP display name attribute
  - `UseSsl` - Should LDAP connection use ssl
- `Cors`
  - `Origins` - list of allowed origins, use `localhost` by default,
  and add some custom origins if application has access to browser url  

## 🏃‍♂️ Usage

After launching the application, you can access the web api from the browser using `Swagger UI`:  
`http://localhost:[your port]/swagger`

Here is some methods description:
- GET: `/api/v1/ping` - check if service is up
- POST: `/api/v1/auth` - authenticate user with LDAP by login/password

### POST `/api/v1/auth` schemes
JSON request body:
```json
{
  "login": "example_user", // user Domain login
  "password": "example_password" // user Domain password
}
```
JSON response:
```json
{
  "status": "OK",
  "code": 200,
  "value": {
    "authorized": true, // is user authorized success
    "userData": {
      "userId": "1.3.3.2.2.1.4554.1.22.3", // LDAP unique identity
      "userDistinguishedName": "CN=Example User,OU=Some group,OU=Users,DC=somedomain,DC=local",
      "userDisplayName": "Example User",
      "memberOfGroups": [ // the name of the groups that the user is a member of
        "some_groups"
      ]
    }
  }
}
```