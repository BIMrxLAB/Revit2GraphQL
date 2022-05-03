using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RevitGraphQLSchema
{
    public class MarconiUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public MarconiUser()
        {

        }

        //public MarconiUser(ClaimsIdentity aIdentity)
        //{
        //    UserId = aIdentity.FindFirst("oid").Value;
        //    UserName = aIdentity.FindFirst("name").Value;
        //    UserEmail = aIdentity.FindFirst("email").Value;
        //}

        public MarconiUser(ClaimsPrincipal aPrincipal)
        {
            UserId = aPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserName = aPrincipal.FindFirst("name").Value;
            UserEmail = aPrincipal.FindFirst("emails").Value;
        }
        public MarconiUser(JwtSecurityToken aToken)
        {
            UserEmail = aToken.Claims.FirstOrDefault(x => x.Type == "emails")?.Value;
            UserName = aToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            UserId = aToken.Claims.FirstOrDefault(x => x.Type == "oid")?.Value;

        }
    }
}
