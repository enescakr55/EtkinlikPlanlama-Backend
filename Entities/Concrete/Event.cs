using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Concrete
{
    public class Event:IEntity
    {
        [Key]
        public int EventId { get; set; }
        
        public int EventOwner { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(EventOwner))]
        public virtual User User { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime Date { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsOnline { get; set; }
        public string EventAddress { get; set; }
    }
}
