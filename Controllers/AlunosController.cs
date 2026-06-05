using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc.Models;

namespace mvc.Controllers
{
    public class AlunosController : Controller
    {
        private readonly ILogger<AlunosController> _logger;

        public AlunosController(ILogger<AlunosController> logger)
        {
            _logger = logger;
        }

        [Route("/alunos")]
        public IActionResult Index()
        {
            return View(Aluno.Todos());
        }

        [Route("/alunos/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("/alunos/create")]
        [HttpPost]
        public IActionResult Create(string nome, string matricula, string notas)
        {
            var aluno = new Aluno()
            {
                Nome = nome,
                Matricula = matricula
            };

            // Converter notas string para List<double>
            if (!string.IsNullOrEmpty(notas))
            {
                var notasArray = notas.Split(',');
                foreach (var nota in notasArray)
                {
                    if (double.TryParse(nota.Trim().Replace(".", ","), out double notaValue))
                    {
                        aluno.Notas.Add(notaValue);
                    }
                }
            }

            aluno.Salvar();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
