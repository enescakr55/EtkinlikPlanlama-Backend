using Business.Abstract;
using Entities.Concrete.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Business.SMTPConfig;
using Core.Results;
using Entities.Concrete;

namespace Business.Concrete
{
    public class SMTPMailManager : ISMTPMailService
    {
        IUserService _userService;
        IAccountValidationCodeService _accountValidationCodeService;
        IEventService _eventService;
        IInvitationService _invitationService;
        IJoinEventService _joinEventService;
        string siteUrl = "http://localhost:4200/";
        //string siteUrl = "http://es-events.gq/";
        //string siteUrl = "http://localhost/";
        public SMTPMailManager(IUserService userService, IAccountValidationCodeService accountValidationCodeService, IEventService eventService, IInvitationService invitationService, IJoinEventService joinEventService)
        {
            _invitationService = invitationService;
            _userService = userService;
            _accountValidationCodeService = accountValidationCodeService;
            _eventService = eventService;
            _joinEventService = joinEventService;
        }
        public SmtpClient Client() {
            SmtpClient client = new SmtpClient();
            client.Port = BusinessConfig.SMTPPort; // Genelde 587 ve 25 portları kullanılmaktadır.
            client.Host = BusinessConfig.SMTPAddress; // Hostunuzun smtp için mail domaini.
            client.EnableSsl = true; // Güvenlik ayarları, host'a ve gönderilen server'a göre değişebilir.
            client.Timeout = 20000; // Milisaniye cinsten timeout
            client.DeliveryMethod = SmtpDeliveryMethod.Network; // Mailin yollanma methodu
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(BusinessConfig.SMTPUsername, BusinessConfig.SMTPPassword); // Burada hangi hesabı kullanarak mail yollayacaksanız onun ayarlarını yapmanız gerekiyor
            return client;

        }
        public bool SendInvitation(SendInvitationDTO sendInvitationDTO)
        {
            
            var inviter = _userService.GetByUserId(sendInvitationDTO.Inviter);
            var invitedEvent = _eventService.GetById(sendInvitationDTO.InvitationInfo.EventId);
            var displayname = "GMT+3";
            string standartName = "+3";
            TimeSpan offset = new TimeSpan(03, 00, 00);
            TimeZoneInfo tz = TimeZoneInfo.CreateCustomTimeZone(standartName, offset, displayname, standartName);
            invitedEvent.Data.Date = TimeZoneInfo.ConvertTimeFromUtc(invitedEvent.Data.Date,tz);
            var invitedUserFullname = sendInvitationDTO.InvitationInfo.Firstname + " " + sendInvitationDTO.InvitationInfo.Lastname;
            var eventType = invitedEvent.Data.IsOnline == true ? "Uzaktan" : "Etkinlik Salonunda";
            var isPrivate = invitedEvent.Data.IsPrivate == true ? "Özel Etkinlik" : "Herkese Açık Etkinlik";
            var header = "<div style='position:relative;'><font style='font-size:17px; font-weight:bold;'>" + invitedEvent.Data.EventName + "</font></div>";
            var replycode = sendInvitationDTO.InvitationInfo.Code;
            var acceptUrl = siteUrl+"invitations/accept/";
            var rejectUrl = siteUrl+"invitations/reject/";
            var replyButtons = "<div><a href='"+acceptUrl+replycode+ "'><button style='margin-right:10px; padding:7px; border-radius:10px; color:#3d9bff; background:transparent; cursor:pointer;'>Katılıyorum</button></a><font> -- </font><a href='" + rejectUrl+replycode+ "'><button style='padding:7px; border-radius:10px; color:#3d9bff; background:transparent; cursor:pointer;'>Katılmıyorum</button></a></div>";
            var html = "<html><body style='font-size:14px;'>"+header+"<div style='border:1px solid black; width:100%; padding:4px;'><div style='background-color:green; color:white; padding:3px;'>Bir etkinliğe davet edildiniz</div><b>Merhaba " + invitedUserFullname + "</b><br/>" + inviter.Data.Email + " tarafından bir etkinliğe davet edildiniz.<br/><br/><b>"+invitedEvent.Data.EventName+"</b><br/>Zaman : "+invitedEvent.Data.Date+"<br/>Etkinlik Yeri : "+eventType+"<br/>Etkinlik Türü : "+isPrivate+"<br/><b>Katılıyor musunuz ?</b><br/>"+replyButtons+"</div></body></html>";
            
            var code = sendInvitationDTO.InvitationInfo.Code;
            try
            {
                var client = Client();
                MailMessage mm = new MailMessage(); // Hangi mail adresinden nereye, konu ve içerik mail ayarlarını yapabilirsiniz
                mm.From = new MailAddress("esnetce@yandex.com", "ES-Events");
                mm.To.Add(sendInvitationDTO.InvitationInfo.Email);
                mm.IsBodyHtml = true; // True: Html olarak Gönderme, False: Text olarak Gönderme
                mm.Subject = "ES-Events Etkinlik Daveti";
                mm.Body = html;
                mm.BodyEncoding = UTF8Encoding.UTF8; // UTF8 encoding ayarı
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess; // Hata olduğunda uyarı ver 
                client.Send(mm);
            }catch(SmtpException e)
            {
                throw e;
            }
            return true;
        }

        public bool SendVerificationMail(string email)
        {
            var emailUser = _userService.GetByEmail(email);
            if(emailUser == null)
            {
                return false;
            }
            if (emailUser.Data.IsVerified)
            {
                return false;
            }
            try
            {
                var fullname = emailUser.Data.Firstname + " " + emailUser.Data.Lastname;
                var createValidation = _accountValidationCodeService.CreateVerificationCode(emailUser.Data.UserId).Data;
                var client = Client();
                MailMessage mm = new MailMessage(); // Hangi mail adresinden nereye, konu ve içerik mail ayarlarını yapabilirsiniz
                mm.From = new MailAddress("esnetce@yandex.com", "ES-Events");
                mm.To.Add(email);
                mm.IsBodyHtml = true; // True: Html olarak Gönderme, False: Text olarak Gönderme
                mm.Subject = "ES-Events Hesap Doğrulama";
                mm.Body = "<html><body><p><b>Merhaba "+fullname+"</b></p>Etkinlik oluşturmak, e-posta adresinize gelen etkinlik davetlerini site üzerinden görmek için lütfen hesabınızı doğrulayın <p>Doğrulama Linki => "+siteUrl+ "verifyaccount/"+createValidation.ValidationCode + "</p><p>Doğrulama Kodunuz => "+createValidation.ValidationCode+"</p><p><center><b>ES-Events Etkinlik Yönetim Projesi</b></center></p></body></html>";
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                
                client.Send(mm);
            }
            catch(SmtpException e)
            {
                throw e;
            }
            // Mail yolla
            return true;
        }
        public bool SendReminder(int eventId)
        {
            List<string> emails = new List<string>();
            List<InvitationInfoDto> invitationInfoList = _invitationService.GetInvitationInfosByEventId(eventId).Data;
            foreach (var i in invitationInfoList)
            {
                if(i.EventInvitationStatus.Status == 1)
                {
                    emails.Add(i.EventInvitation.Email);
                }
            }
            List<JoinEvent> joinEvents = _joinEventService.GetEventJoinsByEventId(eventId).Data;
            foreach (var joinEvent in joinEvents)
            {
                emails.Add(joinEvent.Email);
            }
            var invitedEvent = _eventService.GetById(eventId);
            var displayname = "GMT+3";
            string standartName = "+3";
            TimeSpan offset = new TimeSpan(03, 00, 00);
            TimeZoneInfo tz = TimeZoneInfo.CreateCustomTimeZone(standartName, offset, displayname, standartName);
            invitedEvent.Data.Date = TimeZoneInfo.ConvertTimeFromUtc(invitedEvent.Data.Date, tz);
            var eventType = invitedEvent.Data.IsOnline == true ? "Uzaktan" : "Etkinlik Salonunda";
            var isPrivate = invitedEvent.Data.IsPrivate == true ? "Özel Etkinlik" : "Herkese Açık Etkinlik";
            var header = "<div style='position:relative;'><font style='font-size:17px; font-weight:bold;'>ES-Events Etkinlik Hatırlatma<br/></font></div>";
            var html = "<html><body style='font-size:14px;'>" + header + "<div style='border:1px solid black; width:100%; padding:4px;'><div style='background-color:green; color:white; padding:3px;'>"+invitedEvent.Data.EventName +" hatırlatma maili"+"</div><b>Merhaba, </b><br/>Katılacak olduğunuz <b>" + invitedEvent.Data.EventName + "</b> etkinliği yarın gerçekleşecektir.<br/>Zaman : " + invitedEvent.Data.Date + "<br/>Etkinlik Yeri : " + invitedEvent.Data.EventAddress + "<br/>Etkinlik Türü : " + isPrivate + "<br/></div></body></html>";

            try
            {
                var client = Client();
                MailMessage mm = new MailMessage(); // Hangi mail adresinden nereye, konu ve içerik mail ayarlarını yapabilirsiniz
                mm.From = new MailAddress("esnetce@yandex.com", "ES-Events");
                foreach (var mail in emails)
                {
                    mm.Bcc.Add(mail);
                }
                mm.IsBodyHtml = true; // True: Html olarak Gönderme, False: Text olarak Gönderme
                mm.Subject = "ES-Events Etkinlik Hatırlatma";
                mm.Body = html;
                mm.BodyEncoding = UTF8Encoding.UTF8; // UTF8 encoding ayarı
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess; // Hata olduğunda uyarı ver 
                client.Send(mm);
            }
            catch (SmtpException e)
            {
                throw e;
            }
            return true;
        }
        
    }
}
