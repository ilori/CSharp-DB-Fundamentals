namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Dto.Import;
    using Models;
    using Newtonsoft.Json;

    public class Deserializer
    {
        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var animalAids = JsonConvert.DeserializeObject<AnimalAid[]>(jsonString);

            var sb = new StringBuilder();

            var validAids = new List<AnimalAid>();

            foreach (var dto in animalAids)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine($"Error: Invalid data.");
                    continue;
                }
                if (validAids.Any(x => x.Name == dto.Name))
                {
                    sb.AppendLine($"Error: Invalid data.");
                    continue;
                }
                var animalAid = new AnimalAid()
                {
                    Name = dto.Name,
                    Price = dto.Price
                };
                validAids.Add(animalAid);
                sb.AppendLine($"Record {dto.Name} successfully imported.");
            }
            context.AnimalAids.AddRange(validAids);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var animals = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString);

            var sb = new StringBuilder();

            var validAnimals = new List<Animal>();

            foreach (var dto in animals)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                if (validAnimals.Any(x => x.Passport.SerialNumber == dto.Passport.SerialNumber))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }


                if (!DateTime.TryParseExact(dto.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var registrationDate))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                var passport = new Passport()
                {
                    OwnerName = dto.Passport.OwnerName,
                    OwnerPhoneNumber = dto.Passport.OwnerPhoneNumber,
                    RegistrationDate = registrationDate,
                    SerialNumber = dto.Passport.SerialNumber,
                    Animal = new Animal()
                    {
                        Name = dto.Name,
                        Age = dto.Age,
                        Type = dto.Type
                    }
                };
                if (!IsValid(passport))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                var animal = new Animal()
                {
                    Passport = passport,
                    Name = dto.Name,
                    Age = dto.Age,
                    Type = dto.Type
                };
                validAnimals.Add(animal);
                sb.AppendLine($"Record {dto.Name} Passport №: {dto.Passport.SerialNumber} successfully imported.");
            }
            context.Animals.AddRange(validAnimals);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(VetDto[]), new XmlRootAttribute("Vets"));

            var vets = (VetDto[]) serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var validVets = new List<Vet>();

            foreach (var dto in vets)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine($"Error: Invalid data.");
                    continue;
                }
                if (validVets.Any(x => x.PhoneNumber == dto.PhoneNumber))
                {
                    sb.AppendLine($"Error: Invalid data.");
                    continue;
                }
                var vet = new Vet()
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    PhoneNumber = dto.PhoneNumber,
                    Profession = dto.Profession
                };
                validVets.Add(vet);
                sb.AppendLine($"Record {dto.Name} successfully imported.");
            }
            context.Vets.AddRange(validVets);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));

            var procedures = (ProcedureDto[]) serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var validProcedures = new List<Procedure>();
            foreach (var dto in procedures)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                if (!context.Animals.Any(x => x.PassportSerialNumber == dto.Animal) || !DateTime.TryParseExact(
                        dto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var dateTime) || !context.Vets.Any(x => x.Name == dto.Vet) ||
                    !dto.AnimalAids.All(x => context.AnimalAids.Any(ai => ai.Name == x.Name)))
                {
                    sb.AppendLine("Error: Invalid data.");
                    continue;
                }
                var procedure = new Procedure()
                {
                    Animal = context.Animals.SingleOrDefault(x => x.PassportSerialNumber == dto.Animal),
                    Vet = context.Vets.SingleOrDefault(x => x.Name == dto.Vet),
                    DateTime = dateTime
                };

                var aidsToEnter = new List<ProcedureAnimalAid>();
                var toBreak = false;
                foreach (var aniDto in dto.AnimalAids)
                {
                    if (!IsValid(aniDto))
                    {
                        sb.AppendLine("Error: Invalid data.");
                        continue;
                    }
                    if (aidsToEnter.Any(x => x.AnimalAid.Name == aniDto.Name))
                    {
                        sb.AppendLine("Error: Invalid data.");
                        toBreak = true;
                        break;
                    }
                    var aid = new ProcedureAnimalAid()
                    {
                        Procedure = procedure,
                        AnimalAid = context.AnimalAids.SingleOrDefault(x => x.Name == aniDto.Name)
                    };
                    aidsToEnter.Add(aid);
                }
                if (toBreak)
                {
                    toBreak = false;
                    continue;
                }
                procedure.ProcedureAnimalAids = aidsToEnter;
                validProcedures.Add(procedure);
                sb.AppendLine("Record successfully imported.");
            }
            context.Procedures.AddRange(validProcedures);
            context.SaveChanges();
            return sb.ToString();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, results, true);
        }
    }
}