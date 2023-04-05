namespace ESP.Models.Domains
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public param Category { get; set; }
        public string Answer { get; set; }
        public string Creator { get; set; }
        public DateTime CreationTime { get; set; }

    }
    public enum param
    {
        IT, Administration, Programming
    }
}
