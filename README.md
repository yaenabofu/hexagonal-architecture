# Hexagonal architecture (Ports and Adapters)

# Business Logic
Each user is defined by the following attributes: login, password, group and status. The group can be "User" or "Admin". The status attribute can be "Active" or "Blocked".

At any given time, there can be no more than one active administrator in the system (user with the "Admin" group). In addition, there should not be two or more active users with the same login (users with the "Active" group).

When a user is deleted, his status in the system should change to "Blocked".

## Technologies and Tools
- .NET Core 6
- SQL Server, EntityFrameworkCore
- FluentValidation

## Tests
- xUnit, Moq, InMemoryDB

## Deploy
- Docker

# How to run the project
1.   Run `docker-compose up -d` from directory `hexagonal-architecture/hexagonal-architecture-implementation`
2.   Go to `http://localhost:8080/swagger` 
