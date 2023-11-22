// Código da classe Professor

namespace Ponto.Models
{
    public class Professor
    {
        public int idFuncionario { get; set; }
        public string nmPessoa { get; set; }
        public int nuDRT { get; set; }

        public Professor(int idFuncionario, string nmPessoa, int nuDRT)
        {
            this.idFuncionario = idFuncionario;
            this.nmPessoa = nmPessoa;
            this.nuDRT = nuDRT;
        }


    }
}
