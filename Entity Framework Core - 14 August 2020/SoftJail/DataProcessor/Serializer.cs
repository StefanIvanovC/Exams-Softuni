namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name
                    })
                    .OrderBy(o => o.OfficerName)
                    .ToList(),
                    TotalOfficerSalary = decimal.Parse(p.PrisonerOfficers
                        .Sum(p => p.Officer.Salary)
                        .ToString("F2"))
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToList();

            var result = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return result;
        }
    //"Id": 3,
    //"Name": "Binni Cornhill",
    //"CellNumber": 503,
    //"Officers": [
    //  {
    //    "OfficerName": "Hailee Kennon",
    //    "Department": "ArtificialIntelligence"
    //  },
    //  {
    //    "OfficerName": "Theo Carde",
    //    "Department": "Blockchain"
    //  }


public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            throw new NotImplementedException();
        }
    }
}