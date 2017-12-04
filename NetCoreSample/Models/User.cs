using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreSample.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required, BsonRequired]
        public string Email { get; set; }
        [Required, BsonRequired]
        public string Name { get; set; }
        [Required, BsonRequired]
        public string Password { get; set; }
        public DateTime? LoginedAt { get; set; }
    }
}
