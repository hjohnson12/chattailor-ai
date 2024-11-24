namespace ChatTailorAI.Shared.Events.EventArgs
{
    public class ApiKeyChangedEventArgs : System.EventArgs
    {
        public ApiKeyType KeyType { get; set;  }

        public string ApiKey { get; set; }
    }
}