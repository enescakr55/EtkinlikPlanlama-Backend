using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Concrete
{
    public class InvitationStatus:IEntity
    {
        [Key]
        public int StatusId { get; set; }
        public int InvitationId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(InvitationId))]
        public virtual Invitation Invitation { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
