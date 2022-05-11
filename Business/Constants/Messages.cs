using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public class Messages
    {
        public static string SuccessMessage = "İşlem başarılı";
        public static string ErrorMessage = "İşlem başarısız";
        public static string UserAdded = "Kullanıcı başarıyla eklendi";
        public static string UserDeleted = "Kullanıcı başarıyla silindi";
        public static string UserUpdate = "Kullanıcı başarıyla güncellendi";
        public static string InvalidCode = "Hatalı kod girildi";
        public static string AccountApproved = "Hesap onaylandı";
        public static string WrongMailOrPassword = "Lütfen E-Posta ve şifrenizi kontrol edin";
        public static string SuccessfulLogin = "Giriş başarılı";
    }
}
