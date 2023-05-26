using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DataTransferObjects
{
    public class RefreshToken
    {
        public string Token { get; }
        public DateTime Created { get; }
        public DateTime Expires { get; }

        public RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            Created = DateTime.Now;
            Expires = Created.AddDays(1);
        }
    }
}
