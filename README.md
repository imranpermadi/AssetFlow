# Project Overview

This is a **sample project** created to demonstrate my skills in various technologies. Due to confidentiality agreements, I cannot share the actual projects I have worked on in my professional capacity. However, this project showcases my proficiency in the technologies I use, including **VueJS**, **.NET Core**, **RabbitMQ**, and more. 

The focus of this example is to illustrate the types of solutions I am capable of delivering and my approach to software architecture and development.

## Technologies Used
- **Frontend**: VueJS v3
- **Backend**: .NET Core, .NET Framework
- **Message Queuing**: RabbitMQ
- **Others**: Node.js, Erlang (for RabbitMQ)

## Features
- A restructured Order Management System (OMS)
- Integration of **RabbitMQ** for message queuing and asynchronous communication
- Modular architecture with clearly defined services
- API endpoints designed following RESTful conventions

## Getting Started

### Prerequisites
To run this project locally, ensure you have the following installed:
- **Node.js**
- **RabbitMQ** (along with Erlang)
- **.NET Framework v4.8**
- **.NET Core v3.1**

### Installation Steps

1. Clone the repository to your local machine.
2. Install the required Node.js packages:
   ```bash
   npm install
3. Set up RabbitMQ on your local system.
4. Run the .NET services using Visual Studio or the .NET CLI.
5. Start the frontend application using Vite:
   npm run dev

The development server should now be running, and you can access the application in your browser at http://localhost:3000.

**API and Security Overview
JWT Authentication**
For this project, the backend uses JWT (JSON Web Tokens) to authenticate and authorize users. Below are the key concepts and configurations:

JWT Generation:

The backend generates a JWT token after successful user authentication.
The token contains claims that store user-specific information (e.g., user role, permissions).
The JWT is signed using a secret key stored in environment variables.
Token Validation:

Every request to secure API endpoints should include a valid JWT token in the Authorization header, using the Bearer scheme.
The backend verifies the token, checks the claims, and returns the requested data if the token is valid.
Example Token Header
To authenticate API requests, include the JWT in the header like so:

Authorization: Bearer <your_jwt_token>
Claims: Claims are key-value pairs included within the JWT, and they contain user-related information. Some common claims used in this project include:

sub: The subject of the token (usually the user ID).
role: The user's role (e.g., "admin", "user").
exp: The expiration time of the token.
Security Considerations:

Token Expiry: JWT tokens have an expiration time. Once expired, the user must re-authenticate to receive a new token.
Refresh Tokens: In a production environment, it is advisable to implement refresh tokens, which allow users to obtain new access tokens without having to log in again.
HTTPS: Ensure that all communication happens over HTTPS to protect token integrity during transmission.
Claims Integrity: Claims should be properly validated on every request to ensure the user is authorized to perform the action they are attempting.
Token Handling in .NET
The backend uses token DLLs to manage JWT token operations, such as:

Signing and validating tokens.
Creating claims.
Refreshing expired tokens (if implemented).
Make sure you have the following NuGet packages installed:

System.IdentityModel.Tokens.Jwt
Microsoft.AspNetCore.Authentication.JwtBearer
Security Best Practices
Store JWT Secret Key Securely: The secret key used to sign the tokens should never be hardcoded in the source code. Instead, store it in a secure location such as an environment variable or a key vault.
Use Strong Signing Algorithms: Always use strong signing algorithms like HS256 or RS256.
Limit Token Expiration Time: Keep the expiration time short for added security. Refresh tokens can be used for longer sessions.
Contribution
While this is a personal project for demonstration purposes, contributions to the underlying methodology are welcome! If you'd like to improve this example or suggest enhancements, feel free to open an issue or create a pull request.

Coding Standards
To maintain code clarity and consistency:

Variables that are not directly mapped to database columns should be wrapped inside a #region and written in lowercase_underline format (e.g., shipment_type_name).
All API routes should follow the lowercase_underline convention (e.g., refresh_freight_cost).
API responses are generally wrapped in a ResultData object, with exceptions for list-type APIs.
Disclaimer
This repository is only an example project, and the actual work I’ve done for clients is confidential. If you'd like to discuss real-world projects, feel free to contact me directly through my Upwork profile (https://www.upwork.com/freelancers/~01fa5dc6bf03e3b6f3)


### Key Points:
1. **Vite**: The `npm run dev` command has been added to run the frontend using Vite. This should be mentioned under the installation steps, and I also clarified that the project runs on `http://localhost:3000` by default.
2. **Security (JWT and Claims)**:
   - JWT authentication is explained in detail, covering token generation, validation, and usage.
   - Claims are described, along with an example token header for API authentication.
   - Best practices for security in handling JWT tokens are outlined, especially for production scenarios.
   - I also included a mention of the necessary **NuGet packages** for .NET-based token handling.
3. **Clarification of Sample Project**: Make it clear that this is a sample project and that the actual projects are confidential.
4. **Technologies**: Highlight the key technologies used in the project (VueJS, .NET Core, RabbitMQ, etc.).
5. **Installation Steps**: Provide the necessary instructions for setting up the project locally.
6. **Contribution Guidelines**: Since you cannot directly share the real project, invite others to contribute to this demo project to enhance its quality.
7. **Disclaimer**: Clearly state that the real projects cannot be shared due to confidentiality agreements, but offer to discuss real projects privately.
