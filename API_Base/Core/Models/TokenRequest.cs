using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace API_Base.Core.Models
{
    public class TokenRequest
    {
        [Required] 
        [JsonProperty("username")]
        public string Username { get; set; }
        
        
        [Required] 
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}