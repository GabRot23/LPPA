using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebAPIUAI.DTOs;

namespace WebAPIUAI.Services
{
    public static class HashService
    {
        public static ResultadoHash Hash(string textoPlano)
        {

            var salt = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }
            return Hash(textoPlano, salt);
        }

        public static ResultadoHash Hash(string textoPlano, byte[] salt)
        {
            var llaveDerivada = KeyDerivation.Pbkdf2(
                password: textoPlano,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32
            );

            var hash = Convert.ToBase64String(llaveDerivada);

            return new ResultadoHash()
            {
                Hash = hash,
                Salt = salt
            };
        }
    }
}