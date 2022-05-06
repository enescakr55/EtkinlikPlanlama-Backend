using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete.Helpers
{
    public class Tools
    {
        public static string CreateRandomCode(int n=20)
        {
            string karakterler = "0123456789ABCDEFGHJKLMNOPRSTUVYZabcdefghjklmnoprstuvyz";
            Random rnd = new Random();
            string pano = "";
            for (int i = 0; i < n; i++)
            {
                pano += karakterler[rnd.Next(karakterler.Length)];
            }
            return pano;
        }
    }
}
