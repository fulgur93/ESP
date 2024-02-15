using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
        public odp CorrectAnswer { get; set; }
        public string? SelectedOption { get; set; }
        public string? Creator { get; set; }
        public DateTime CreationTime { get; set; }

    }
    [Flags]
    public enum odp
    {
        [EnumMember(Value = "A")]
        [Display(Name = "A")]
        A = 1,

        [EnumMember(Value = "B")]
        [Display(Name = "B")]
        B = 2,

        [EnumMember(Value = "C")]
        [Display(Name = "C")]
        C = 3
    }
    [Flags]
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
