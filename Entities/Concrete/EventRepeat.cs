using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Concrete
{
    public class EventRepeat:IEntity
    {
        [Key]
        public int EventRepeatId { get; set; }
        public int EventId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
