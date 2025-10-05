# OKR - Authentication - Authorization

## Purpose

######The main purpose of this project is to provide `Authentication & Authorization` for upcoming projects.
######Second purpose is for learning purposes especially learning the microservices structure and communication between them.

### Features

- Authentication and authorization using json web tokens
- Bundled Swagger for multiple microservices
- Gateway using Ocelot
- Microservices communication using `RabbitMQ` as message broker
- Mapping, form validations, exception handling
- Mediatr pattern

##RabbitMQ

#####What Is RabbitMQ?
RabbitMQ is one of the most popular Message-Broker Service. It supports various messaging protocols. It basically gives your applications a common platform for sending and receiving messages. This ensures that your messages (data) is never lost and is successfully received by each intended consumer. RabbitMQ makes the entire process seemless.

In simple words, you will have a publisher that publishes messages to the message broker (RabbitMQ Server). Now the server stores the message in a queue. To this particular queue , multiple consumers can subscribe. Whenever there is a new message, each of the subscibers would receive it. An application can act as both producer / consumer based on how you configure it and what the requirement demands.

A message could consist of any kind of information like a simple string to a complex nested class. RabbitMQ stores these data within the server till a consumer connects and take the message off the queue for processing.

###Installing RabbitMQ

####Installing ErLang
Erlang is a programming language with which the RabbitMQ server is built on. Since we are installing the RabbitMQ Server locally to our Machine (Windows 10), make sure that you install Erlang first.

Download the Installer from here – `https://www.erlang.org/downloads`

Install it in your machine with Admin Rights.

####Installing RabbitMQ as a Service in Windows
We will be installing the RabbitMQ Server and a service within our Windows machine.

Download from here – `https://www.rabbitmq.com/install-windows.html` I used the official installer. Make sure that you are an Admin.

####Enabling RabbitMQ Management Plugin – Dashboard
Now that we have our RabbitMQ Service installed at the sysyem level, we need to activate the Management Dashboard which by default is disabled. To do this, open up Command Prompt with Admin Rights and enter the following command:

```
cd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.8.7\sbin
rabbitmq-plugins enable rabbitmq_management
net stop RabbitMQ
net start RabbitMQ
```

## Web App

### Project Structure: Microservice Structure

##### src: Contains all the microservices and other solution project files
##### Identity.Microservice: Is an monolithic web api containing all user related methods
##### Api.Gateway: Is the gateway for unifying all microservices urls, used for exception handling, authentication and many more

### Identity.Microservice - Structure

- Identity.Microservice.Api: Contains api endpoints that target specified handlers
- Identity.Microservice.Core: Contains all the services, dto, interfaces and handlers that do the logic of the project
- Identity.Microservice.Domain: Contains domain entities, appsettings.json configurations as classes, enums that are used by services or handlers
- Identity.Microservice.Infrastructure: Contains database connection configurations, entities configurations
- Identity.Microservice.WebHost: Contains initial app configurations for services and middlewares and app running configurations

## Identity.Microservice.Core

- Commands: This folder contains mediatr handlers for crud operations
- Events: Contains events that are published with mediatr
- Queries: Contains mediatr handlers used for getting information
- Mappings: Contains automapper configurations for different entities

## Identity.Microservice.Domain

- Common: Contains common methods or classes that are used in overall project
- Configuration: Contains appsettings.json configurations stored in classes
- Entities: Contains entities used for data manipulation with connected tables in database

## Identity.Microservice.Infrastructure

- DAL: Is the data access layer folder and contains configurations for db context
- Persistence: Contains configurations for entities post project build

## Identity.Microservice.WebHost

- Configurations: Contains service configurations used in startup as bundle
- Consumers: RabbitMQ subscribers for events published

## Api.Gateway

- ocelot.json: Contains all the routes of the microservices that will be unified and authenticated
