using System;

namespace SecureXWebApp.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string StatusLine { get; set; } = "";

        public string Message { get; set; } = "An error occurred while processing your request.";

        public ErrorViewModel()
        { }

        public ErrorViewModel(string message)
        {
            Message = message;
        }

        public ErrorViewModel(string statusLine, string message)
        {
            StatusLine = statusLine;
            Message = message;
        }
    }
}