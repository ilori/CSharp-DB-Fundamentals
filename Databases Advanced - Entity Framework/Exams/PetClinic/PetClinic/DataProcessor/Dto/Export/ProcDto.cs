namespace PetClinic.DataProcessor.Dto.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlType("Procedure")]
    public class ProcDto
    {
        public string Passport { get; set; }

        public string OwnerNumber { get; set; }

        public string DateTime { get; set; }

        public AniAidDto[] AnimalAids { get; set; } = new AniAidDto[0];

        public decimal TotalPrice { get; set; }


    }
}