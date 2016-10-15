﻿using FaaS.Entities.Contexts;
using FaaS.Migrations.FaaSContextMigrations;
using System;
using System.Data.Entity;

namespace FaaS.Migrations
{
    class Program
    {
        private const string CONNECTION_STRING_NAME = "GameStoreConnection";

        static void Main(string[] args)
        {
            Console.WriteLine("Setting migration initializer...");

            // Ensure migration initializer is used and it uses context's connection
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FaaSContext, FaaSContextConfiguration>(useSuppliedContext: true));

            // Initialize databases of both contexts with same connection string
            using (var db = new FaaSContext(CONNECTION_STRING_NAME))
            {
                IntializeDatabase(db);
            }

            Console.WriteLine("Everything is gonna be OK...");
            Console.ReadKey();
        }

        /// <summary>
        /// Forces database initialization on provided <paramref name="context"/>.
        /// </summary>
        /// <typeparam name="TContext">Type of concrete implementation of <see cref="DbContext"/>.</typeparam>
        /// <param name="context">Instance of concrete context</param>
        private static void IntializeDatabase<TContext>(TContext context)
            where TContext : DbContext
        {
            Console.WriteLine($"Initializing {typeof(TContext).FullName}...");
            context.Database.Initialize(force: true);
        }
    }
}