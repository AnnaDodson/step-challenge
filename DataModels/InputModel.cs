using System.ComponentModel.DataAnnotations;

namespace StepChallenge.DataModels
{
        public class InputModel
        {
             public string Username { get; set; }
             public int TeamId { get; set; }
             public string Password { get; set; }
        }
}