namespace PetClinic.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("AnimalAid")]
    public class AnimalAidDto
    {
        [Required]
        public string Name { get; set; }
    }
}