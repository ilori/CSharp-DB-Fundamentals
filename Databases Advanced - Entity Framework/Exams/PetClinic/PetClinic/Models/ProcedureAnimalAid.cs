namespace PetClinic.Models
{
    public class ProcedureAnimalAid
    {
        public Procedure Procedure { get; set; }
        public int ProcedureId { get; set; }

        public AnimalAid AnimalAid { get; set; }
        public int AnimalAidId { get; set; }
    }
}