namespace PetClinic.DataProcessor
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class Bonus
    {
        public static string UpdateVetProfession(PetClinicContext context, string phoneNumber, string newProfession)
        {
            var sb = new StringBuilder();

            var vet = context.Vets.SingleOrDefault(x => x.PhoneNumber == phoneNumber);

            if (vet == null)
            {
                sb.AppendLine($"Vet with phone number {phoneNumber} not found!");
                return sb.ToString().TrimEnd();
            }

            var oldProfession = vet.Profession;
            vet.Profession = newProfession;
            context.SaveChanges();
            sb.AppendLine($"{vet.Name}'s profession updated from {oldProfession} to {newProfession}.");
            return sb.ToString().TrimEnd();
        }
    }
}