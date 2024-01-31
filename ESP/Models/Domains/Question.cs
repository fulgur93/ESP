using System.ComponentModel.DataAnnotations;

namespace ESP.Models.Domains
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public param Category { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public int CorrectAnswer { get; set; }
        public string Creator { get; set; }
        public DateTime CreationTime { get; set; }

    }
    public enum param
    {
        [Display(Name = "Organizacja i funkcjonowanie administracji publicznej")]
        OrganizacjaIFunkcjonowanieAdministracjiPublicznej,
        [Display(Name = "Podstawy prawne funkcjonowania statystyki publicznej")]
        PodstawyPrawneFunkcjonowaniaStatystykiPublicznej,
        [Display(Name = "Ochrona danych osobowych oraz informacji niejawnych")]
        OchronaDanychOsobowychOrazInformacjiNiejawnych,
        [Display(Name = "System służby cywilnej")]
        SystemSłużbyCywilnej,
        [Display(Name = "Etyka i uczciwość w służbie cywilnej")]
        EtykaIUczciwośćWSłużbieCywilnej,
        [Display(Name = "Ochrona praw człowieka")]
        OchronaPrawCzłowieka,
        [Display(Name = "Równe traktowanie w administracji publicznej")]
        RówneTraktowanieWAdministracjiPublicznej,
        [Display(Name = "Bezpieczeństwo informacji")]
        BezpieczeństwoInformacji,
        [Display(Name = "Wartośći statystyki oficjalnej i koncepcji mierzenia wartości statystyk oficjalnych")]
        WartośćiStatystykiOficjalnejIKoncepcjiMierzeniaWartościStatystykOficjalnych
    }
}
