using System.ComponentModel.DataAnnotations;

namespace ExerciseAPI.Models
{
    public class Exercise
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? Force { get; set; }
        public string? Level { get; set; }
        public string? Mechanic { get; set; }
        public string? Equipment { get; set; }
        //public string[]? PrimaryMuscles { get; set; }
        public string? PrimaryMuscles { get; set; }
        //public string[]? SecondaryMuscles { get; set; }
        public string? SecondaryMuscles { get; set; }
        //public string[]? Instructions { get; set; }
        public string? Instructions { get; set; }
        public string? Category { get; set; }
        public string? ImgPath1 { get; set; }
        public string? ImgPath2 { get; set; }
    }
}
