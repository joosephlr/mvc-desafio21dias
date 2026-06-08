using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using mvc.Models;
using mvc.Services;

namespace mvc.Services
{
    public class AlunoService
    {
        private readonly SupabaseService _supabaseService;

        public AlunoService(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        public async Task<List<Aluno>> ObterTodosAsync()
        {
            try
            {
                // GetAsJsonAsync retorna a string JSON bruta
                var jsonString = await _supabaseService.GetAsJsonAsync("Alunos");
                var alunos = new List<Aluno>();

                if (!string.IsNullOrEmpty(jsonString))
                {
                    using (JsonDocument doc = JsonDocument.Parse(jsonString))
                    {
                        var root = doc.RootElement;
                        
                        if (root.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var item in root.EnumerateArray())
                            {
                                var notas = new List<double>();
                                
                                if (item.TryGetProperty("notas", out var notasElement) && notasElement.ValueKind == JsonValueKind.String)
                                {
                                    var notasStr = notasElement.GetString();
                                    if (!string.IsNullOrEmpty(notasStr))
                                    {
                                        foreach (var nota in notasStr.Split(','))
                                        {
                                            if (double.TryParse(nota.Trim().Replace(".", ","), out double notaValue))
                                            {
                                                notas.Add(notaValue);
                                            }
                                        }
                                    }
                                }

                                var aluno = new Aluno()
                                {
                                    Id = item.GetProperty("id").GetInt32(),
                                    Nome = item.GetProperty("nome").GetString() ?? "",
                                    Matricula = item.GetProperty("matricula").GetString() ?? "",
                                    Notas = notas
                                };

                                alunos.Add(aluno);
                            }
                        }
                    }
                }

                return alunos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter alunos: {ex.Message}");
                return new List<Aluno>();
            }
        }

        public async Task InserirAsync(Aluno aluno)
        {
            try
            {
                var data = new
                {
                    nome = aluno.Nome,
                    matricula = aluno.Matricula,
                    notas = string.Join(",", aluno.Notas.ToArray())
                };

                await _supabaseService.PostAsync("Alunos", data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir aluno: {ex.Message}");
                throw;
            }
        }

        public async Task AtualizarAsync(Aluno aluno)
        {
            try
            {
                var data = new
                {
                    nome = aluno.Nome,
                    matricula = aluno.Matricula,
                    notas = string.Join(",", aluno.Notas.ToArray())
                };

                await _supabaseService.PatchAsync($"Alunos?id=eq.{aluno.Id}", data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar aluno: {ex.Message}");
                throw;
            }
        }

        public async Task DeletarAsync(int id)
        {
            try
            {
                await _supabaseService.DeleteAsync($"Alunos?id=eq.{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar aluno: {ex.Message}");
                throw;
            }
        }
    }
}
