namespace DatabaseClient
{
    public class AnError
    {
        public string Text { get; set; }
        public ErrorSource Source { get; set; }
    }
    public enum ErrorSource
    {
        Conversion,
        Validation
    }
}
