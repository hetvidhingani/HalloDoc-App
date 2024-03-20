namespace HalloDoc.Entities.ViewModels
{
    public class 
        SendEmailModel
    {
        public string Email { get; set; } = string.Empty;       
        public string From { get; set; } = string.Empty;

        public string To { get; set; } = string.Empty;

        public string SmtpServer { get; set; } = string.Empty;

        public long Port { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;    
    }
}
