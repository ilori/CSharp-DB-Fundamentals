namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Data;
    using Dto.Export;
    using Microsoft.EntityFrameworkCore;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var animals = context.Animals
                .Where(x => x.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(x => new
                {
                    x.Passport.OwnerName,
                    AnimalName = x.Name,
                    x.Age,
                    SerialNumber = x.PassportSerialNumber,
                    RegisteredOn = x.Passport.RegistrationDate.ToString("dd-MM-yyyy")
                }).OrderBy(x => x.Age)
                .ThenBy(x => x.SerialNumber)
                .ToList();

            var result = JsonConvert.SerializeObject(animals, Formatting.Indented);

            return result;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var procedures = context.Procedures
                .Include(x => x.Animal)
                .ThenInclude(x => x.Passport)
                .Include(x => x.ProcedureAnimalAids)
                .ThenInclude(x => x.AnimalAid)
                .OrderBy(x => x.DateTime)
                .ThenBy(x => x.Animal.Passport.SerialNumber)
                .Select(x => new ProcDto()
                {
                    Passport = x.Animal.PassportSerialNumber,
                    OwnerNumber = x.Animal.Passport.OwnerPhoneNumber,
                    DateTime = x.DateTime.ToString("dd-MM-yyyy"),
                    AnimalAids = x.ProcedureAnimalAids.Select(pa => new AniAidDto()
                    {
                        Name = pa.AnimalAid.Name,
                        Price = pa.AnimalAid.Price
                    }).ToArray(),
                    TotalPrice = x.ProcedureAnimalAids.Select(pa => pa.AnimalAid.Price).Sum()
                })
                .ToList();

            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(List<ProcDto>), new XmlRootAttribute("Procedures"));
            serializer.Serialize(new StringWriter(sb), procedures,
                new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty}));

            return sb.ToString();
        }
    }
}