using ESP.Areas.Identity.Data;
using ESP.Data;
using ESP.Models;
using ESP.Models.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using static System.Net.Mime.MediaTypeNames;

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
            if (questionsFiltr.Count>=3)
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
        public async  Task<IActionResult> Update(Guid id)
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
                return await Task.Run(() => View("Update",viewModel));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateQuestionViewModel model)
        {
            var quest = await mvcDbContext.Question.FindAsync(model.Id);
            if(quest != null)
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
        public ActionResult CheckAnswer(List<Question> questions)
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
            if (wynik>=12)//60%
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
        public ActionResult Congratulations(int score)
        {
            ViewBag.Score = score;
            return View();
        }

        public ActionResult FailedTest(int score)
        {
            ViewBag.Score = score;
            return View();
        }
        [HttpPost]
        public ActionResult GeneratePdf(int score)
        {
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
                        document.Add(new Paragraph($"Certyfikat ukończenia testu z wynikiem: {score}"));
                        // Dodaj więcej treści do dokumentu według potrzeb
                        document.Close();
                    }
                }

                Response.Headers["Content-Disposition"] = $"inline; filename=Certificate_{score}.pdf";
                return File(stream.ToArray(), "application/pdf");
            }
        }

    }
}
