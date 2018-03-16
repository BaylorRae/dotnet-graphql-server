using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Http;

namespace API.middleware
{
    public class SeedDataMiddleware
    {
        private readonly BicycleShopContext _bicycleShopContext;
        private readonly RequestDelegate _next;

        public SeedDataMiddleware(BicycleShopContext bicycleShopContext, RequestDelegate next)
        {
            _bicycleShopContext = bicycleShopContext;
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path != "/seed")
            {
                return _next(context);
            }
            
            const string expectedPassword = "a-password";
            var actualPassword = context.Request.Query["password"];

            if (actualPassword != expectedPassword)
            {
                return context.Response.WriteAsync("Password doesn't match");
            }

            var faker = new Faker();

            context.Response.WriteAsync("Beginning to seed database\n");

            context.Response.WriteAsync("- Adding 75 Parts\n");
            
            var parts = new List<Part>();
            var bicycles = new List<Bicycle>();

            var partTypes = new[]
            {
                "chain",
                "lock",
                "grip",
                "pedal",
                "spoke",
                "tire",
                "tube",
                "air pump",
                "frame",
                "saddle",
                "horn",
                "seat clamp",
                "helmet",
                "action camera",
                "glove"
            };

            for (var i = 0; i < 75; i++)
            {
                var partTitle = Titleize(string.Join(' ',
                    faker.Commerce.Color(),
                    faker.Commerce.ProductMaterial(),
                    faker.PickRandom(partTypes)
                ));

                context.Response.WriteAsync($"\t {i + 1}. {partTitle}\n");
                
                parts.Add(new Part
                {
                    Title = partTitle,
                    Price = faker.Random.Decimal(1, 1000),
                    Quantity = faker.Random.Number(1, 10),
                    Discontinued = faker.Random.Bool()
                });
            }
            
            context.Response.WriteAsync("- Adding 25 Bicycles\n");
            
            for (var i = 0; i < 25; i++)
            {
                var bicycleTitle = Titleize(string.Join(' ',
                    faker.Commerce.Color(),
                    faker.Name.FullName(),
                    "Bicycle"
                ));

                context.Response.WriteAsync($"\t {i + 1}. {bicycleTitle}\n");
                
                bicycles.Add(new Bicycle
                {
                    Title = bicycleTitle,
                    Price = faker.Random.Decimal(1, 1000),
                    Quantity = faker.Random.Number(1, 10),
                    Discontinued = faker.Random.Bool(),
                    BicycleParts = faker.Random
                        .Shuffle(parts)
                        .Take(faker.Random.Number(1, 20))
                        .Select(part => new BicyclePart
                        {
                            Part = part
                        }).ToList()
                });
            }

            using (var transaction = _bicycleShopContext.Database.BeginTransaction())
            {
                _bicycleShopContext.Parts.Insert(parts);
                _bicycleShopContext.Bicycles.Insert(bicycles);

                try
                {
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    context.Response.WriteAsync("Error Saving Records\n");
                    return context.Response.WriteAsync(e.ToString());
                }
            }
            
            return context.Response.WriteAsync("Seeding Database Complete");
        }

        private static string Titleize(string words)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words);
        }
    }
}