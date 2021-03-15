# JwTokenExample
Example code for authorization with JSON Web Tokens. Uses in memory EF Core for user testdata seeding.

#### Programs needed
- Visual studio
- .NET Core 3.1
- Postman or equivalent REST-client

#### How to build and start the project
- Pull the latest master-branch to you local enviroment and open the project
- Start IIS Express

#### How to log in/authenticate
Start the project and open Postman or your prefered REST-client.

Send the following to authenticate with the backend.
````
endpoint: /auth/login
method: POST
body:
{
	"login" : "luke.skywalker@skywalker-ranch.com",
	"password": "skywalker"
}
````
In the response body you'll find two properties named ````token```` and ````refresh````, which both contains long strings. These are your validation- and refresh-tokens respectively. 
````
{
    "userId": 1,
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJsdWtlLnNreXdhbGtlckBza3l3YWxrZXItcmFuY2guY29tIiwibmJmIjoxNTgwOTc4ODM4LCJleHAiOjE1ODA5ODI0MzgsImlhdCI6MTU4MDk3ODgzOH0.4YfNow-AINCTPiu1wkjEAIjY634_FSzUGEfIUyEiirk",
    "refresh": "G5T6yu3dVdpE5/U+hy9hEs7yg7OY3jHRFKB+l7ll87M="
}
````

You are now authenticated and logged in on the server. You can now use the validation-token to make requests to the api.

#### How to make requests with the validation-token
Copy the validation-token you got in the previous step and paste it into your Authorization-Header. Make sure your Authorization-Header ````Type```` is set to ````Bearer Token```` (or similar if you're using a different REST-client. The Authorization-Header should look something like this:
````
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJsdWtlLnNreXdhbGtlckBza3l3YWxrZXItcmFuY2guY29tIiwibmJmIjoxNTgwOTc4ODM4LCJleHAiOjE1ODA5ODI0MzgsImlhdCI6MTU4MDk3ODgzOH0.4YfNow-AINCTPiu1wkjEAIjY634_FSzUGEfIUyEiirk
````

Change the method to GET and go to the protected endpoint ````/user````. The request should go through and a response with all the users in the database should be delivered to you.

#### How to refresh the jwt token using the refresh-token
When the jwt token expires it is no longer valid and can not be used to send requests to the server. You can request a new token by calling the refresh endpoint with the jwt-token and the refresh-token which you acquired when you authenticated.
````
endpoint: /auth/refresh
method: POST
body:
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJsdWtlLnNreXdhbGtlckBza3l3YWxrZXItcmFuY2guY29tIiwibmJmIjoxNTgwOTc4ODM4LCJleHAiOjE1ODA5ODI0MzgsImlhdCI6MTU4MDk3ODgzOH0.4YfNow-AINCTPiu1wkjEAIjY634_FSzUGEfIUyEiirk",
    "refresh": "G5T6yu3dVdpE5/U+hy9hEs7yg7OY3jHRFKB+l7ll87M="
}
````
In the response you'll find a new jwt- and refresh-token which you can use to make more calls to the api.

#### How to change the Jwt- and refreshtoken lifetime
You find the lifetime settings in the appsettings.json file.

````
{
  "Auth": {
    "JwTSecret": "myVerySecretSecret",
    "JwTLifetimeInMinutes": "1",
    "RefreshTokenLifetimeInDays":  "1"
  }
}
````
It's good practice to keel the lifetime of the jwt relatively short (1-10 minutes) and the lifetime of the refresh-token a bit longer (1-2 days). You can think of the refresh-token lifetime as the maximum time the user is allowed to be logged in without using the system.

#### User roles
User roles are stored as strings on the User entities. The roles are added to the claims list when a user performs a successfull login and can be checked by using the 
````
[Autorize(Roles = "Role")]
````
... decorator in the usual manner.
