using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace Entities.Concrete
{
    public class AccountValidationCode:IEntity
    {
        [Key]
        public int ValidationCodeId { get; set; }
        
        public int UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public string ValidationCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsUsed { get; set; }

    }
}
