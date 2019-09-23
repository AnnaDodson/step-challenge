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
