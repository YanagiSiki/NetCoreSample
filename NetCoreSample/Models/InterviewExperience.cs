using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreSample.Models
{
    public class InterviewExperience
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExperienceId { get; set; } = 0;
        [Required]
        public string Experience { get; set; }
        public DateTime InterviewDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
        
    }
}