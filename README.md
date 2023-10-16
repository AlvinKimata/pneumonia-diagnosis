# AI-based Pneumonia diagnosis system.

### This web application is implemented using the `ASP.Net Core` framework.

### 1. Database setup (Ubuntu 22.04 + versions).
The database used is Ms SQL server running on a docker container. Use the command below to pull and start a docker container from the image.

```bash
$ docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<password>" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

Start the MS SQL server instance in the docker container with the command below.

```bash
$ docker exec -it <docker_container_name> /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P <password>
```

### 2. Model endpoint for performing inference.
A) The endpoint has been implemented using Flask. Install requirements.

```bash
$ cd "pneumonia diagnosis" && pip install -r requirements.txt
```

B) Run the command below to start the Flask API endpoint.
```bash
$ cd "pneumonia diagnosis/endpoint"
$ python classification_endpoint.py
```

### 3. Run project.
Use the command below to install web app dependencies.

```bash
$ cd "webapp"
$ dotnet restore
$ dotnet build
$ dotnet run
```