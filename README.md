# AE Backend Code Challenge

The AE Backend Code Challenge aims to build a solution comprised of REST APIs that find the closest port to a given ship and calculate the estimated arrival time based on velocity and geolocation (longitude and latitude) of the ship. The project uses C# and xUnit for testing.

## Table of Contents

1. [Project Overview](#project-overview)
2. [Installation and Setup](#installation-and-setup)
3. [API Endpoints](#api-endpoints)
4. [Database Schema](#database-schema)
5. [Testing](#testing)
6. [Usage Examples](#usage-examples)
7. [Error Handling](#error-handling)

## Project Overview

The AE Backend Code Challenge involves the following components:

- Users: Each user has a name, role, and can be assigned one or more ships.
- Ports: Ports have names and geolocations (longitude and latitude).
- Ships: Each ship has a name, a unique ship ID, geolocation, and velocity. Ships can be assigned to users.

## Installation and Setup

1. **Prerequisites**:
   - Install .NET Core (if not already installed).
   - Set up your database (e.g., SQL Server) and configure the connection string in `appsettings.json`.

2. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/ae-backend-challenge.git
   cd ae-backend-challenge
   ```

3. **Build and Run**:
   ```bash
   dotnet build
   dotnet run
   ```

## API Endpoints

- **Port**:
	- **Get All Ports**:
	  - Endpoint: `GET /api/ports/GetAllPorts`
	  
	- **Get Port by Id**:
	  - Endpoint: `GET /api/ports/GetPortbyId/{id}`
	  
- **Role**:
	- **Get All Roles**:
	  - Endpoint: `GET /api/roles/GetAllRoles`
	  
	- **Get Role by Id**:
	  - Endpoint: `GET /api/roles/GetRolebyId/{id}`
	  
	- **Add Role**:
	  - Endpoint: `POST /api/roles/CreateRole`
	  - Request Body: `{ "roleName": "superuser", "createdBy": "admin" }`

	- **Update Role**:
	  - Endpoint: `PUT /api/roles/UpdateRole`
	  - Request Body: `{ "roleId": 1, "roleName": "superuser", "rowStatus": 1, "modifiedBy": "admin" }`

	- **Delete Role**:
	  - Endpoint: `GET /api/roles/DeleteRole?roleId=&modifiedBy=`

- **Ship**:
	- **Get All Ships**:
	  - Endpoint: `GET /api/ships/GetAllShips`
	  
	- **Get Ship by Id**:
	  - Endpoint: `GET /api/ships/GetShipbyId/{id}`
	  
	- **Add Ship**:
	  - Endpoint: `POST /api/ships/CreateShip`
	  - Request Body: `{ "shipName": "Ocean Voyager", "longitude": -73.9876, "latitude": 40.7128, "velocity": 15.5, "createdBy": "admin" }`

	- **Update Ship**:
	  - Endpoint: `PUT /api/ships/UpdateShip`
	  - Request Body: `{ "shipId": 1, "shipName": "Ocean Voyager", "longitude": -73.9876, "latitude": 40.7128, "velocity": 15.5, "rowStatus": 1, "modifiedBy": "admin" }`

	- **Delete Ship**:
	  - Endpoint: `GET /api/ships/DeleteShip?shipId=&modifiedBy=`

	- **Get Unassigned Ships**:
	  - Endpoint: `GET /api/ships/GetUnassignedShips/`
	  
	- **Get Closest Port**:
	  - Endpoint: `GET /api/ships/GetClosestPort/{shipId}`
	  
	- **Get Port Distances**:
	  - Endpoint: `GET /api/ships/port-distances/{shipId}/{limit}`
	  
- **User**:
	- **Get All Users**:
	  - Endpoint: `GET /api/users/GetAllUsers`
	  
	- **Get Users by Id**:
	  - Endpoint: `GET /api/users/GetUserbyId/{userId}`
	  
	- **Add User**:
	  - Endpoint: `POST /api/users/CreateUser`
	  - Request Body: `{ "username": "john_doe", "fullname": "John Doe", "birthdate": "1990-01-01", "createdBy": "admin" }`

	- **Update User**:
	  - Endpoint: `PUT /api/users/UpdateUser`
	  - Request Body: `{ "userId": 0, "username": "john_doe", "fullname": "John Doe", "birthdate": "1990-01-01", "rowStatus": 1, "modifiedBy": "admin" }`

	- **Delete User**:
	  - Endpoint: `GET /api/users/DeleteUser?userId=&modifiedBy=`

	- **Get Ships for User**:
	  - Endpoint: `GET /api/users/GetShipsForUser/{userId}`
	  
- **UserRoles**:
	- **Get All UserRoles**:
	  - Endpoint: `GET /api/UserRoles/GetAllUserRoles`
	  
	- **Get UserRoles by Id**:
	  - Endpoint: `GET /api/UserRoles/GetUserRolebyId/{id}`
	  
	- **Add UserRoles**:
	  - Endpoint: `POST /api/UserRoles/CreateUserRole`
	  - Request Body: `{ "userId": 1, "roleId": 2, "createdBy": "admin" }`

	- **Update UserRoles**:
	  - Endpoint: `PUT /api/UserRoles/UpdateUserRole`
	  - Request Body: `{ "userRoleId": 1, "userId": 2, "roleId": 3, "rowStatus": 1, "modifiedBy": "admin" }`

	- **Delete UserRoles**:
	  - Endpoint: `GET /api/UserRoles/DeleteUserRole?userRoleId=&modifiedBy=`
	  
- **UserShips**:
	- **Get All UserShips**:
	  - Endpoint: `GET /api/roles/GetAllUserShips`
	  
	- **Get UserShip by Id**:
	  - Endpoint: `GET /api/roles/GetUserShipbyId/{id}`
	  
	- **Add UserShip**:
	  - Endpoint: `POST /api/roles/CreateUserShip`
	  - Request Body: `{ "userId": 1, "shipId": 2, "createdBy": "admin" }`

	- **Update UserShip**:
	  - Endpoint: `PUT /api/roles/UpdateUserShip`
	  - Request Body: `{ "userShipId": 1, "userId": 2, "shipId": 3, "rowStatus": 1, "modifiedBy": "admin" }`

	- **Delete UserShip**:
	  - Endpoint: `GET /api/roles/DeleteUserShip?userShipId=&modifiedBy=`

## Database Schema

- Ports
  - PortId (PK)
  - PortName
  - Longitude
  - Latitude
  - RowStatus
  - CreatedDate
  - CreatedBy
  - ModifiedDate
  - ModifiedBy

- Roles
  - RoleId (PK)
  - RoleName
  - RowStatus
  - CreatedDate
  - CreatedBy
  - ModifiedDate
  - ModifiedBy
 
- Ships
  - ShipId (PK)
  - ShipName
  - Longitude
  - Latitude
  - Velocity
  - RowStatus
  - CreatedDate
  - CreatedBy
  - ModifiedDate
  - ModifiedBy
  
- Users
  - UserId (PK)
  - Username
  - Fullname
  - Birthdate
  - RowStatus
  - CreatedDate
  - CreatedBy
  - ModifiedDate
  - ModifiedBy
  
 - UserRoles
  - UserRoleId (PK)
  - UserId
  - RoleId
  - RowStatus
  - CreatedDate
  - CreatedBy
  - ModifiedDate
  - ModifiedBy
  
- UserShips
  - UserShipid (PK)
  - UserId
  - shipId
  - RowStatus
  - CreatedDate
  - CreatedBy
  - ModifiedDate
  - ModifiedBy

## Testing

Run unit tests using xUnit:
```bash
dotnet test
```

## Usage Examples

- To retrieve all ports, send a GET request to `/api/ports/GetAllPorts`.
- To retrieve a certain port by their id, send a GET request to `/api/ports/GetPortbyId`.

- To add a role, make a POST request to `/api/Roles/CreateRole`.
- To retrieve all roles, send a GET request to `/api/Roles/GetAllRoles`.
- To retrieve a certain role by their id, send a GET request to `/api/Roles/GetRolebyId`.
- To update a role, make a POST request to `/api/Roles/UpdateRole`.
- To delete a role, make a POST request to `/api/Roles/DeleteRole`.

- To add a ship, make a POST request to `/api/ships/CreateShip`.
- To retrieve all ships, send a GET request to `/api/ships/GetAllShips`.
- To retrieve a certain ship by their id, send a GET request to `/api/ships/GetShipbyId`.
- To update a ship, make a POST request to `/api/ships/UpdateShip`.
- To delete a ship, make a POST request to `/api/ships/DeleteShip`.
- To retrieve all ships that haven't assigned to a user, send a GET request to `/api/ships/GetUnassignedShips`.
- To retrieve a port closest to a certain ship, send a GET request to `/api/ships/GetClosestPort`.
- To retrieve all ports information, distance, and estimated time from certain ship, send a GET request to `/api/ships/port-distances`.

- To add a user, make a POST request to `/api/users/CreateUser`.
- To retrieve all users, send a GET request to `/api/users/GetAllUsers`.
- To retrieve a certain user by their id, send a GET request to `/api/users/GetUserbyId`.
- To update a user, make a POST request to `/api/users/UpdateUser`.
- To delete a user, make a POST request to `/api/users/DeleteUser`.
- To retrieve all ships that have been assigned to a certain user, send a GET request to `/api/users/GetShipsForUser`.

- To add a UserRole, make a POST request to `/api/UserRoles/CreateUserRole`.
- To retrieve all UserRoles, send a GET request to `/api/UserRoles/GetAllUserRoles`.
- To retrieve a certain UserRole by their id, send a GET request to `/api/UserRoles/GetUserRolebyI`.
- To update a UserRole, make a POST request to `/api/UserRoles/UpdateUserRole`.
- To delete a UserRole, make a POST request to `/api/UserRoles/DeleteUserRole`.

- To add a UserShip, make a POST request to `/api/UserShips/CreateUserShip`.
- To retrieve all UserShips, send a GET request to `/api/UserShips/GetAllUserShips`.
- To retrieve a certain UserShip by their id, send a GET request to `/api/UserShips/GetUserShipbyId`.
- To update a UserShip, make a POST request to `/api/UserShips/UpdateUserShip`.
- To delete a UserShip, make a POST request to `/api/UserShips/DeleteUserShip`.

## Error Handling

- Properly handle exceptions and provide meaningful error messages.
- Consider using custom exception classes for specific scenarios.
