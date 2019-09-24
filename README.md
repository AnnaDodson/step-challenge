# Step Challenge Web App

- Dotnet Core
- GraphQL
- ASP Identity
- Entity Framework Core
- SQLite
- React

## Requirements

## To Run Locally

The front end is all within the `ClientApp` directory. It needs building and running locally to make changes to the React files. There's a great indepth README in this directory.

In the ClientApp directory, first install all the node modules:

```
$ npm i
```
Then to run the node server

```
$ npm start
```

The main dotnet app is configured to proxy to the front end port, you can change this in the `StartUp.cs` file if you don't need to build the React front end.

Make sure your environment variable is set to development.
```
ASPNETCORE_Environment=Development
```

And ensure you have a database in the `db/` directory

```
$ dotnet ef database update
```

Then run the dotnet app from the command line:

```
$ dotnet run
```

Or through Rider or Visual Studio.

It uses SSL for the authentication, so ignore or accept local host to run locally on https.

## To Build the App

Build the front end first:

```
$ npm run build
```

Then build the main app:

```
$ dotnet build
```

**Note, this currently doesn't work very well!

## To Build and Run in Docker

Build the docker image:

```
$ git clone https://github.com/powercrazed/step-challenge.git
$ cd step-challenge
$ git checkout docker
# docker build -t stepchallenge .
```

Create a volume for the database:

```
# docker volume create stepchallenge-db
```

Create the Container:

```
# docker create --name=stepchallenge -v stepchallenge-db:/app/db -p 80:80 --restart=unless-stopped stepchallenge
```

Copy in the blank database (first run only):

```
# docker cp <path to StepChallenge.db> stepchallenge:/app/db
```

Start the container:

```
# docker start stepchallenge
```