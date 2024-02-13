using ESP.Models.Domains;
namespace ESP.Models
    
{
    public class AddQuestionViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public param Category { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string CorrectAnswer { get; set; }
        public string? SelectedOption { get; set; }
        public string? Creator { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
