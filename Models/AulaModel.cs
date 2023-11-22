namespace Ponto.Models
{
    public class AulaModel
    {
        public int idTurmaHorario { get; set; }
        public DateTime dtAula { get; set; }
        public string hrInicioAula { get; set; }
        public string hrFimAula { get; set; }
        public int idPeriodoLetivo { get; set; }
        public int aaPeriodoLetivo { get; set; }
        public int nuPeriodoLetivo { get; set; }
        public string nmCampus { get; set; }
        public string nmCurso { get; set; }
        public string nmTurno { get; set; }
        public int idUnidadeFuncional { get; set; }
        public string sgUnidadeFuncional { get; set; }
        public string nmBloco { get; set; }
        public string deEspacoFisico { get; set; }
        public string nmDisciplina { get; set; }
        public string coTurma { get; set; }
        public int idFuncionario { get; set; }
        public string nmPessoa { get; set; }
        public int nuDRT { get; set; }
    }

    public class AulaResponse
    {
        public string id { get; set; }
        public List<AulaModel> response { get; set; }
        public List<object> notificacoes { get; set; }
    }



}
