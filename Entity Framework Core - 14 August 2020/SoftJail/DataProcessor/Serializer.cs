namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Linq;
    using XmlFacade;

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
            var names = prisonersNames.Split("," , StringSplitOptions.RemoveEmptyEntries);

            var result = context.Prisoners
                .Where(x => names.Contains(x.FullName))
                .Select(x => new PrisonersInboxXmlExportModel
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd"),
                    EncryptedMessages = x.Mails.Select(m => new EncriptedMassagesViewModel
                    {
                        Description = string.Join("", m.Description.Reverse())
                    })
                    .ToArray()

                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            var resultForReturn = XmlConverter.Serialize(result, "Prisoners");

            return resultForReturn;
               
        }
    }
}

//< Prisoners >
//  < Prisoner >
//    < Id > 3 </ Id >
//    < Name > Binni Cornhill </ Name >
   
//       < IncarcerationDate > 1967 - 04 - 29 </ IncarcerationDate >
   
//       < EncryptedMessages >
   
//         < Message >
   
//           < Description > !? sdnasuoht evif - ytnewt rof deksa uoy ro orez artxe na ereht sI</Description>
//      </Message>
//    </EncryptedMessages>
