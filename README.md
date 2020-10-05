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
In the response body you'll find a property named ````token````, which contains a long string. This is your validation-token. 
````
{
    "userId": 1,
    "login": "luke.skywalker@skywalker-ranch.com",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJsdWtlLnNreXdhbGtlckBza3l3YWxrZXItcmFuY2guY29tIiwibmJmIjoxNTgwOTc4ODM4LCJleHAiOjE1ODA5ODI0MzgsImlhdCI6MTU4MDk3ODgzOH0.4YfNow-AINCTPiu1wkjEAIjY634_FSzUGEfIUyEiirk"
}
````

You are now authenticated and logged in on the server. You can now use the validation-token to make requests to the api.

#### How to make requests with the validation-token
Copy the validation-token you got in the previous step and paste it into your Authorization-Header. Make sure your Authorization-Header ````Type```` is set to ````Bearer Token```` (or similar if you're using a different REST-client. The Authorization-Header should look something like this:
````
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJsdWtlLnNreXdhbGtlckBza3l3YWxrZXItcmFuY2guY29tIiwibmJmIjoxNTgwOTc4ODM4LCJleHAiOjE1ODA5ODI0MzgsImlhdCI6MTU4MDk3ODgzOH0.4YfNow-AINCTPiu1wkjEAIjY634_FSzUGEfIUyEiirk
````

Change the method to GET and go to the protected endpoint ````/user````. The request should go through and a response with all the users in the database should be delivered to you.
