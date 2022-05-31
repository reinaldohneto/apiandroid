﻿namespace AppAndroid;

public static class DatabaseConnection
{
    public static string? ConfigureDatabaseConnection(string environmentName)
    {
        string? defaultConnectionString;

        if (environmentName == "Development") {
            defaultConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
        }
        else
        {
            // Use connection string provided at runtime by Heroku.
            var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            connectionUrl = connectionUrl?.Replace("postgres://", string.Empty);
            var userPassSide = connectionUrl?.Split("@")[0];
            var hostSide = connectionUrl?.Split("@")[1];

            var user = userPassSide?.Split(":")[0];
            var password = userPassSide?.Split(":")[1];
            var host = hostSide?.Split("/")[0];
            var database = hostSide?.Split("/")[1].Split("?")[0];

            defaultConnectionString =
                $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
        }

        return defaultConnectionString;
    }
}