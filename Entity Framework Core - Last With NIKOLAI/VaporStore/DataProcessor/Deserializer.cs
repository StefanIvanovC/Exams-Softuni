namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var games = JsonConvert.DeserializeObject<IEnumerable<GenresTagImportModel>>(jsonString);

            foreach (var game in games)
            {
                if (!IsValid(game) || game.Tags.Count() == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var ganre = context.Genres.FirstOrDefault(x => x.Name == game.Genre) ?? new Genre { Name = game.Genre };
                var developer = context.Developers.FirstOrDefault(x => x.Name == game.Developer) ?? new Developer { Name = game.Developer };

                var gameForImport = new Game
                {
                    Name = game.Name,
                    Genre = ganre,
                    Developer = developer,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate.Value,
                };

                foreach (var jsonTags in game.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(x => x.Name == jsonTags)
                       ?? new Tag { Name = jsonTags };
                    gameForImport.GameTags.Add(new GameTag { Tag = tag });
                }
                sb.AppendLine($"Added {game.Name} ({game.Genre}) with {game.Tags.Count()} tags");
                context.AddRange(gameForImport);
                context.SaveChanges();

            }

            return sb.ToString().TrimEnd();

        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var users = JsonConvert.DeserializeObject<IEnumerable<UsersImportModel>>(jsonString);

            foreach (var jsonUser in users)
            {
                if (!IsValid(jsonUser))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User
                {
                    FullName = jsonUser.FullName,
                    Age = jsonUser.Age,
                    Email = jsonUser.Email,
                    Username = jsonUser.Username,
                    Cards = jsonUser.Cards.Select(x => new Card
                    {
                        Number = x.Number,
                        Cvc = x.CVC,
                        Type = x.Type.Value
                    }).ToList(),
                };

                sb.AppendLine($"Imported {jsonUser.Username} with {jsonUser.Cards.Count()} cards");
                context.Users.Add(user);
                
            }
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}