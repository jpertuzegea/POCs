//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Marzo 2023</date>
//-----------------------------------------------------------------------

namespace POC_JWT_NetCore6.Models
{
    public class JWTAuthentication
    {
        public int ExpirationInMinutes { get; set; }
        public string Secret { get; set; }
        public string[] HostOriginPermited { get; set; }

    }
}
