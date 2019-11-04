namespace StepChallenge.DataModels
{
    public class LoginModel
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string ErrorLogMessage { get; set; }
        public bool IsAdmin { get; set; }
    }
}