using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete.Dtos
{
    public class SendInvitationDTO
    {
        public Invitation InvitationInfo { get; set; }
        public int Inviter { get; set; }
    }
}
