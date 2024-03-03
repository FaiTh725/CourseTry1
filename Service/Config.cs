using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CourseTry1.Service
{
    public class Config
    {
        public string Issuer {get;set;}

        public string Audience { get;set;}

        public string Key { get;set;}

        public int LifeTime { get; set; } = 2;

        public Config()
        {
        }

        public Config(IConfiguration configuration)
        {
            var conf = configuration.GetSection("JWTConfiguration");

            Issuer = conf["Issuer"];
            Audience = conf["Audience"];
            Key = conf["Key"];
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}
