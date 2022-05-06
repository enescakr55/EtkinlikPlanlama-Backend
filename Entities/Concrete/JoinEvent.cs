using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Concrete
{
    public class JoinEvent:IEntity
    {
        [Key]
        public int JoinId { get; set; }
        public int EventId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
