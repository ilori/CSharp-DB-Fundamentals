namespace PetClinic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Procedure
    {
        public int Id { get; set; }

        [Required]
        public Animal Animal { get; set; }

        public int AnimalId { get; set; }

        [Required]
        public Vet Vet { get; set; }

        public int VetId { get; set; }

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; } = new List<ProcedureAnimalAid>();

        public decimal Cost { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}