using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Data.Dtos
{
    public class AuthenticateRequest
    {
      [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }   
    }
}