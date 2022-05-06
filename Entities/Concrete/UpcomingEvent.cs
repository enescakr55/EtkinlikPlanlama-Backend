using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Concrete
{
    public class UpcomingEvent : IEntity
    {
       [Key]
        public int UpcomingEventId { get; set; }
        public int EventId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; }
        public DateTime EventDate { get; set; }
    }
}
