namespace PetClinic.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Procedure")]
    public class ProcedureDto
    {
        [Required]
        public string Vet { get; set; }

        [Required]
        public string Animal { get; set; }

        [Required]
        public string DateTime { get; set; }

        public AnimalAidDto[] AnimalAids { get; set; } = new AnimalAidDto[0];
    }
}