# Step Challenge Web App

Built for a company step challenge competition. In beta.

## Contents
   1. [Tech stack](#tech-stack)
   2. [Contributing](#contributing)
   3. [License](#license)
   4. [Requirements](#requirements)
   5. [To Run Locally](#to-run-locally)
   6. [To Build the App](#to-build-the-app)
   7. [To Build and Run in Docker](#to-build-and-run-in-docker)

## Tech stack

- Dotnet Core
- GraphQL
- ASP Identity
- Entity Framework Core
- SQLite
- React

## Contributing

Check out the [contributing guide](/contributing.md) for instructions on how to contribute to this project.

## License

This app is licensed as the GNU Affero General Public License v3.0.

Check you're happy with this before contributing or using the source code.

[tldrlegal.com/license/gnu-affero-general-public-license-v3-(agpl-3.0)](https://tldrlegal.com/license/gnu-affero-general-public-license-v3-(agpl-3.0))

## Requirements

// TODO

## To Run Locally

The front end is all within the `ClientApp` directory. It needs building and running locally to make changes to the React files. There's a great indepth README in this directory.

In the ClientApp directory, first install all the node modules:

```
$ npm i
```
Then run the node server

```
$ npm start
```

The main dotnet app is configured to proxy to the front end port, you can change this in the `StartUp.cs` file if you don't need to build the React front end.

Make sure your environment variable is set to development.
```
ASPNETCORE_Environment=Development
```

The sqlite db will be automatically created on startup into the `db` directory and the teams are auto generated.


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
$ dotnet publish
```

**Note, this currently doesn't work very well!

## To Build and Run in Docker

Build the docker image:

```
$ git clone https://github.com/AnnaDodson/step-challenge.git
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

Start the container:

```
# docker start stepchallenge
```

### Running locally in docker

To run docker locally, you'll need to set up local certs for the ASP identity to work. See [documentation](https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/aspnetcore-docker-https.md) here.

To create the container:

```
docker create --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="crypticpassword" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v ${HOME}/.aspnet/https:/https/ -v stepchallenge-db:/app/db --name=stepchallenge stepchallenge
```

Then navigate to [https://localhost:8001](https://localhost:8001)

Happy Stepping!