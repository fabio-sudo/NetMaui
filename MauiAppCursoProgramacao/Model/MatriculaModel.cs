namespace MauiAppCursoProgramacao.Model
{
   public class MatriculaModel
    {
      public int idMatriclua { get; set; }
      public int ordemMatricula { get; set; }
      public string diaSemana { get; set; }
      public ProfessorModel professor { get; set; }
      public CursoModel curso { get; set; }
      public PeriodoModel periodo { get; set; }
      public string? statusCurso { get; set; }
      public List<AlunoModel>listaAlunos { get; set; } = null!;

    }
}
