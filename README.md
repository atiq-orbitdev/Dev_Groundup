# Dev_Groundup
Develop app ground up using Github

## WebAPI Project
This repository contains a WebAPI project built using ASP.NET Core. To run the project:

1. Navigate to the project directory.
2. Use the command `dotnet run` to start the application.
3. Access the API at `https://localhost:5049` or `http://localhost:5049`.

## Testing API Endpoints
This project includes Swagger for testing API endpoints. To use Swagger:

1. Start the application using `dotnet run`.
2. Open your browser and navigate to `https://localhost:5049/swagger` or `http://localhost:5049/swagger`.
3. Use the Swagger UI to explore and test the available API endpoints.

## Enabling CORS
CORS (Cross-Origin Resource Sharing) is enabled in this project to allow requests from any origin. The configuration is added in the `Program.cs` file:

```
