using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Common
{
    public static class StringHelper
    {
        public static string GenerateRandomString(int length)
        {
            var random = new Random();
            var sb = new StringBuilder(length);
            for (var i = length; i > 0; i--)
            {
                var num = random.Next(122);
                var isValid = (num >= 'a' && num <= 'z') 
                    || (num >= 'A' && num <= 'Z') 
                    || (num >= '0' && num <= '9');
                if (isValid)
                {
                    sb.Append((char)num);
                }
                else
                {
                    i++;
                }
            }

            return sb.ToString();
        }
    }
}
