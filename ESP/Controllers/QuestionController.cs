using ESP.Areas.Identity.Data;
using ESP.Data;
using ESP.Models;
using ESP.Models.Domains;
using iText.Commons.Actions.Contexts;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ESP.Controllers
{
    public class QuestionController : Controller
    {
        private readonly MVCDBC mvcDbContext;//Baza pytań
        private readonly UserManager<ForumUser> _userManager; // Do rozpoznawania użytkownika
        private readonly ILogger<HomeController> _logger;
        public QuestionController(MVCDBC mvcDbContext, UserManager<ForumUser> userManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;// Do rozpoznawania użytkownika
            this.mvcDbContext = mvcDbContext;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Ustaw zmienną w sesji
            HttpContext.Session.SetInt32("wynik", 0);
            // Ustaw zmienną w sesji
            HttpContext.Session.SetInt32("nrPytania", 1);
            HttpContext.Session.SetInt32("skip", 0);
            HttpContext.Session.SetString("prewId", "");

            //var questions = await mvcDbContext.Question.ToListAsync();
            List<Question> questions = new List<Question>();
            return View(questions);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(param category)
        {
            var questions = await mvcDbContext.Question.ToListAsync();
            List<Question> questionsFiltr = new List<Question>();
            List<Question> questionsRand = new List<Question>();
            Random rnd = new Random();

            foreach (var quest in questions)
            {
                if (quest.Category == category)
                {
                    questionsFiltr.Add(quest);
                }
            }
            if (questionsFiltr.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    int randNum = rnd.Next(questionsFiltr.Count);
                    questionsRand.Add(questionsFiltr[randNum]);
                    questionsFiltr.Remove(questionsFiltr[randNum]);
                }
                return View(questionsRand);
            }
            else
            {
                return View(questionsFiltr);
            }

        }
        [HttpGet]
        public async Task<IActionResult> IndexQuest()
        {
            var questions = await mvcDbContext.Question.ToListAsync();
            return View(questions);
        }
        [HttpGet]
        public async Task<IActionResult> TestQuest()//Test
        {
            var questions = await mvcDbContext.Question.ToListAsync();
            HttpContext.Session.SetInt32("wynik", 0);
            HttpContext.Session.SetInt32("nrPytania", 0);
            return View(questions);
        }
        //Dodawanie pytań
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddQuestionViewModel addQuestionRequest)
        {
            if (_userManager.GetUserName(HttpContext.User) != null)
            {
                var question = new Question()
                {
                    Id = Guid.NewGuid(),
                    Text = addQuestionRequest.Text,
                    Category = addQuestionRequest.Category,
                    //Answer = addQuestionRequest.Answer,
                    AnswerA = addQuestionRequest.AnswerA,
                    AnswerB = addQuestionRequest.AnswerB,
                    AnswerC = addQuestionRequest.AnswerC,
                    CorrectAnswer = addQuestionRequest.CorrectAnswer,
                    CreationTime = DateTime.Now,
                    Creator = _userManager.GetUserName(HttpContext.User) // Nadanie nazwy użytkownika
                };

                await mvcDbContext.Question.AddAsync(question);
                await mvcDbContext.SaveChangesAsync();
                return RedirectToAction("IndexQuest");
            }
            else
            {
                return RedirectToAction("Error");
            }




        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var quest = await mvcDbContext.Question.FirstOrDefaultAsync(x => x.Id == id);
            if (quest != null)
            {
                var viewModel = new UpdateQuestionViewModel()
                {
                    Id = quest.Id,
                    Text = quest.Text,
                    Category = quest.Category,
                    AnswerA = quest.AnswerA,
                    AnswerB = quest.AnswerB,
                    AnswerC = quest.AnswerC,
                    CorrectAnswer = quest.CorrectAnswer,
                    CreationTime = DateTime.Now,
                    Creator = quest.Creator
                };
                return await Task.Run(() => View("Update", viewModel));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateQuestionViewModel model)
        {
            var quest = await mvcDbContext.Question.FindAsync(model.Id);
            if (quest != null)
            {
                quest.Text = model.Text;
                quest.Category = model.Category;
                //quest.Answer = model.Answer;
                quest.AnswerA = model.AnswerA;
                quest.AnswerB = model.AnswerB;
                quest.AnswerC = model.AnswerC;
                quest.CorrectAnswer = model.CorrectAnswer;
                quest.CreationTime = DateTime.Now;
                quest.Creator = model.Creator;

                await mvcDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");//Error Page 
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateQuestionViewModel model)
        {
            var quest = await mvcDbContext.Question.FindAsync(model.Id);
            if (quest != null)
            {
                mvcDbContext.Question.Remove(quest);
                await mvcDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult CheckAnswer(List<Question> questions)
        {
            // questions to lista pytań przekazanych z formularza
            int wynik = 0;
            foreach (var quest in questions)
            {
                // question.Id - Id pytania
                // question.CorrectAnswer - Poprawna odpowiedź
                // question.SelectedOption - Wybrana odpowiedź

                if (quest.CorrectAnswer == quest.SelectedOption)
                {
                    // Odpowiedź jest poprawna
                    wynik++;
                }
                else
                {
                    // Odpowiedź jest niepoprawna
                }
            }
            if (wynik >= 12)//60%
            {
                return RedirectToAction("Congratulations", new { score = wynik });
                //Wygenerój certyfikat
            }
            else
            {
                return RedirectToAction("FailedTest", new { score = wynik });
            }
            // ...
            // return View("NazwaTwojegoWidoku", questions);
            return View();
        }

        public ActionResult Congratulations()
        {

            //HttpContext.Session.SetInt32("wynik", 0);
            //HttpContext.Session.SetInt32("nrPytania", 0);
            return View();
        }

        public ActionResult FailedTest()
        {

            //HttpContext.Session.SetInt32("wynik", 0);
            //HttpContext.Session.SetInt32("nrPytania", 0);
            return View();
        }
        [HttpPost]
        public ActionResult GeneratePdf()
        {
            int wynik = HttpContext.Session.GetInt32("wynik") ?? 0;
            // Logika generowania PDF na podstawie wyniku testu
            // Użyj odpowiednich narzędzi do generowania PDF, np. iTextSharp

            // Przykładowy kod generowania PDF
            using (MemoryStream stream = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(stream))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        Document document = new Document(pdf);
                        document.Add(new Paragraph($"Certyfikat ukończenia testu z wynikiem: "+ ((wynik/20) * 100).ToString()+"%"));
                        // Dodaj więcej treści do dokumentu według potrzeb
                        document.Close();
                    }
                }

                Response.Headers["Content-Disposition"] = $"inline; filename=CertificateESP.pdf";
                return File(stream.ToArray(), "application/pdf");
            }
        }
        [HttpGet]
        public async Task<IActionResult> OneTest(Guid id)
        {
            var quest = await mvcDbContext.Question.FirstOrDefaultAsync(x => x.Id == id);
            if (quest != null)
            {
                var viewModel = new UpdateQuestionViewModel()
                {
                    Id = quest.Id,
                    Text = quest.Text,
                    Category = quest.Category,
                    AnswerA = quest.AnswerA,
                    AnswerB = quest.AnswerB,
                    AnswerC = quest.AnswerC,
                    CorrectAnswer = quest.CorrectAnswer,
                    CreationTime = DateTime.Now,
                    Creator = quest.Creator
                };
                return await Task.Run(() => View("OneTest", viewModel));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> OneTest(UpdateQuestionViewModel model)
        {
            // Pobierz zmienną z sesji
            int wynik = HttpContext.Session.GetInt32("wynik") ?? 0;
            int nrPytania = HttpContext.Session.GetInt32("nrPytania") ?? 1;
            //HttpContext.Session.SetInt32("skip", 0);
            //HttpContext.Session.SetInt32("prewId", 0);
            int skipValue = HttpContext.Session.GetInt32("skip") ?? 1;  // Początkowa wartość skip

            HttpContext.Session.SetString("prewId", model.Id.ToString());
            string prewId = HttpContext.Session.GetString("prewId") ?? "";

            var quest = await mvcDbContext.Question.FindAsync(model.Id);
            if (quest != null)
            {
                quest.Text = model.Text;
                quest.Category = model.Category;
                quest.AnswerA = model.AnswerA;
                quest.AnswerB = model.AnswerB;
                quest.AnswerC = model.AnswerC;
                quest.CorrectAnswer = model.CorrectAnswer;
                quest.SelectedOption = model.SelectedOption;
                quest.CreationTime = DateTime.Now;
                quest.Creator = model.Creator;

                //await mvcDbContext.SaveChangesAsync();
                //Sprawdzanie odpowiedzi
                if (quest.SelectedOption == quest.CorrectAnswer)
                {
                    wynik++;
                    HttpContext.Session.SetInt32("wynik", wynik);
                }
                if (nrPytania % 2 == 1)  // Jeśli i jest nieparzyste (bo indeksowanie zaczyna się od 0)
                {
                    skipValue ++;
                    HttpContext.Session.SetInt32("skip", skipValue);
                }
                // Pobierz id drugiej kategorii
                var secondCategory = await mvcDbContext.Question
                    .Select(q => q.Category)
                    .Distinct()
                    .Skip(skipValue)  // Pomijamy pierwszą kategorię
                    .FirstOrDefaultAsync();

                // Pobierz losowe pytanie z drugiej kategorii
                var randomQuestion = await mvcDbContext.Question
                    .Where(q => q.Category == secondCategory && q.Id.ToString() != prewId)
                    .OrderBy(q => Guid.NewGuid())  // Losowe sortowanie
                    .FirstOrDefaultAsync();


                if (nrPytania>=20)//20
                {
                    if (wynik >= 12)//12/60%
                    {
                        return RedirectToAction("Congratulations");
                        //Wygenerój certyfikat
                    }
                    else
                    {
                        return RedirectToAction("FailedTest");
                    }
                }
                else
                {
                    nrPytania++;
                    // Zapisz nową wartość w sesji
                    HttpContext.Session.SetInt32("nrPytania", nrPytania);
                    return RedirectToAction("OneTest", randomQuestion);
                }

            }
            return RedirectToAction("Index");//Error Page 
        }

    }
}
