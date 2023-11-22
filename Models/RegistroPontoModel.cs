namespace Ponto.Models
{
    public class RegistroPontoModel
    {
        public List<RegistroPontoModel> registros { get; set; }
        public int idTipoRegistroPonto { get; set; }
        public int idTurmaHorario { get; set; }
        public string hrReferenciaPonto { get; set; }
        public string deJustificativa { get; set; }
        public int idTipoJustificativaPonto { get; set; }

    }
}
