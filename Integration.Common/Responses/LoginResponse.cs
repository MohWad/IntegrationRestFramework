using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Common.Responses
{
    /// <summary>
    /// The login response object
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// The generated jwt token
        /// </summary>
        public string Token { get; set; }
    }
}
