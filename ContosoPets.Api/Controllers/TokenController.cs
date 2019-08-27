using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using ContosoPets.Domain.Models;
using ContosoPets.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ContosoPets.Api.Controllers
{
    public class TokenController : Controller
    {
        
        //JwtToken jwttoken = new JwtToken();
        public readonly JwtSettings _jwtSettings;

        private HttpResponseMessage WriteMsg(string msg) => new HttpResponseMessage{Content=new StringContent(msg,Encoding.UTF8,"application/json")};

        public TokenController(IOptions<JwtSettings> jwtSettingsAccesser)
        {
            this._jwtSettings = jwtSettingsAccesser.Value;
        }

        public ActionResult CreateToken([FromBody]User user) 
        {
            string strResult = "";
            if(user.UserName =="admin" && user.Password=="admin")
            {
                //jwttoken.
            }
            return Content(strResult);
        }

        private string MakeToken(User user)
        {
            string strToken = "";
            var claim = new Claim[]{
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,user.Password)
            };

            //对称密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            //签名证书(密钥，加密算法)
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            
            //生成token [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
           var token = new JwtSecurityToken
                (       
                    issuer: _jwtSettings.Issuer,               
                    audience: _jwtSettings.Audience,         
                    claims: claim,       
                    notBefore: DateTime.Now,         
                    expires: DateTime.Now.AddHours(2),//过期时间        
                    signingCredentials: creds         
                );
                try
                {
                    //生成口令
                    strToken = new JwtSecurityTokenHandler().WriteToken(token);
                }
                catch (System.Exception)
                {            
                    throw;
                }
                return strToken;
        }
    }
}