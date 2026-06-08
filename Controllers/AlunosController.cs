using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc.Models;
using mvc.Services;

namespace mvc.Controllers
{
    public class AlunosController : Controller
    {
        private readonly ILogger<AlunosController> _logger;
        private readonly AlunoService _alunoService;

        public AlunosController(ILogger<AlunosController> logger, AlunoService alunoService)
        {
            _logger = logger;
            _alunoService = alunoService;
        }

        [Route("/alunos")]
        public async Task<IActionResult> Index()
        {
            var alunos = await _alunoService.ObterTodosAsync();
            return View(alunos);
        }

        [Route("/alunos/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("/alunos/create")]
        [HttpPost]
        public async Task<IActionResult> Create(string nome, string matricula, string notas)
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

            await _alunoService.InserirAsync(aluno);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
