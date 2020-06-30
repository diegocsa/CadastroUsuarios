using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CadastroUsuarios.Infra.CrossCutting
{
    public static class StringExtensions 
    {
        public static bool EmailValido(this string email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string TratarComoSenha(this string senhaPlana)
        {
            byte[] salt = Encoding.ASCII.GetBytes(senhaPlana);

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: senhaPlana,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            
        }
    }
}
