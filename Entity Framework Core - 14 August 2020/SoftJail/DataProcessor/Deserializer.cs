namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using XmlFacade;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var departments = new List<Department>();

            var departmentCells = JsonConvert.DeserializeObject<IEnumerable<DepartmentCellInputModel>>(jsonString);

            foreach (var departmentCell in departmentCells)
            {
                if(!IsValid(departmentCell) || !departmentCell.Cells.All(IsValid) || !departmentCell.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                };

                var department = new Department
                {
                    Name = departmentCell.Name,
                    Cells = departmentCell.Cells.Select(x => new Cell
                    {
                        CellNumber = x.CellNumber,
                        HasWindow = x.HasWindow,

                    })
                    .ToList()
                };

                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                
            }
            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var listOfPrisoners = new List<Prisoner>();

            var prisonerMails = JsonConvert
                .DeserializeObject<IEnumerable<ImportPrisonersMailsInputModel>>(jsonString);

            foreach (var prisoner in prisonerMails)
            {
                if (!IsValid(prisoner) || !prisoner.Mails.Any(IsValid) || !prisoner.Mails.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var IsValidReleaseDate = DateTime.TryParseExact(prisoner.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);
                var incarcerationDate = DateTime.ParseExact(prisoner.IncarcerationDate,"dd/MM/yyyy",CultureInfo.InvariantCulture);

                var prisoners = new Prisoner
                {
                    FullName = prisoner.FullName,
                    Nickname = prisoner.Nickname,
                    Age = prisoner.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = IsValidReleaseDate ? (DateTime?)releaseDate : null,
                    Bail = prisoner.Bail,
                    CellId = prisoner.CellId,
                    Mails = prisoner.Mails.Select(x => new Mail
                    {
                        Description = x.Description,
                        Sender = x.Sender,
                        Address = x.Address
                    })
                    .ToList()
                };

                listOfPrisoners.Add(prisoners);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(listOfPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var listOfficerPrisoners = new List<Officer>();

            var officerPrisoners = XmlConverter.Deserializer<ImportOfficersPrisonersInputModel>(xmlString, "Officers");

            foreach (var officerPrisoner in officerPrisoners)
            {
                if (!IsValid(officerPrisoner))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var offiser = new Officer
                {
                    FullName = officerPrisoner.Name,
                    Salary = officerPrisoner.Money,
                    Position = Enum.Parse<Position>(officerPrisoner.Position),
                    Weapon = Enum.Parse<Weapon>(officerPrisoner.Weapon),
                    DepartmentId = officerPrisoner.DepartmentId,
                    OfficerPrisoners = officerPrisoner.Prisoners.Select(x => new OfficerPrisoner
                    {
                        PrisonerId = x.Id
                    })
                    .ToList()
                };

                listOfficerPrisoners.Add(offiser);
                sb.AppendLine($"Imported {offiser.FullName} ({offiser.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(listOfficerPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}