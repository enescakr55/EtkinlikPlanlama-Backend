using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete.Dtos
{
    public class InvitationInfoDto
    {
        public Invitation EventInvitation { get; set; }
        public InvitationStatus EventInvitationStatus { get; set; }
    }
}
