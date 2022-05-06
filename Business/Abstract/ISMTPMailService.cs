using Core.Results;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ISMTPMailService
    {
        public bool SendInvitation(SendInvitationDTO sendInvitationDTO);
        public bool SendVerificationMail(string email);
        bool SendReminder(int eventId);
    }
}
