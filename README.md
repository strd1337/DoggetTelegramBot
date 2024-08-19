# DoggetTelegramBot

This telegram bot enhances user engagement in the game community by managing virtual currency transactions and facilitating game-related activities. Developed in C# with .NET and adhering to Clean Architecture principles, the bot enables users to earn and manage in-game currency through various chat commands, activity tracking, and interactions. The project’s structure emphasizes a clear separation of responsibilities across layers, ensuring the system is scalable, maintainable, and easy to test.

Technologies:
1) C# with .NET - the core programming language and framework used to build the bot, providing robust performance and scalability.
2) CQRS (Command Query Responsibility Segregation) - ensures separation of the read (queries) and write (commands) operations for optimized performance and clearer code organization.
3) Mediator Pattern - utilizes MediatR for handling communication between components, allowing for decoupled architecture and easier testing.
4) Mapster - provides object mapping functionality between different layers (e.g., from DTOs to domain models) to maintain clean separation.
5) Entity Framework Core with PostgreSQL - manages the database layer, with Npgsql used for PostgreSQL-specific database operations. EF Core's design tools enable efficient database migrations and schema management.
6) Telegram Bot API - the bot interacts with Telegram's API to send and receive messages, using the Telegram.Bot package.
7) ErrorOr - standardizes error handling and result types across commands and queries.
8) NLog - provides flexible and powerful logging capabilities for monitoring bot behavior and troubleshooting.
9) AspNetCoreRateLimit - implements rate limiting to ensure that the bot adheres to API limits and prevents abuse of its functionality.
