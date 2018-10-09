using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreSample.Models
{
    public class User
    {
        //https://stackoverflow.com/questions/36155429/auto-increment-on-partial-primary-key-with-entity-framework-core
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; } 
        [Required]
        public string Email { get; set; }
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime? LoginAt { get; set; }
        public string VerifyCode { get; set; }
        public bool Active { get; set; }


        public IEnumerable<Post> Posts { get; set; }
    }
}
