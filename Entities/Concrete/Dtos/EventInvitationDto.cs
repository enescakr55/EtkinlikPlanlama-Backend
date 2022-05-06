using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete.Dtos
{
    public class EventInvitationDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public string EventAddress { get; set; }
        public string InvitedUserEmail { get; set; }
        public int InvitationId { get; set; }
        public string InvitationCode { get; set; }
    }
}
