using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ponto.Configs;
using System.Collections.Generic;

using System.Net.NetworkInformation;

namespace Ponto.Pages
{
    public partial class Home : ContentPage
    {
        private ApiService apiService;
        private string authToken;
        private List<AulaModel> Aula;

        private List<RegistroPontoModel> registros = new List<RegistroPontoModel>();

        public string ApiUrl { get; set; }

        public Home(string authToken, List<AulaModel> Aula)
        {
            ApiUrl = "https://ida.ceub.br/api/v1.0/pp/Aulas?dataInicio=2023-10-24T00:00:00&datafim=2023-10-24T00:00:00";

            this.authToken = authToken;
            this.Aula = Aula;

            apiService = new ApiService(authToken);

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ExecuteRequest();

            DateTime dataAtual = DateTime.Now;
            string semanaDia = dataAtual.ToString("dddd" + " | " + "dd/MM/yyyy", new System.Globalization.CultureInfo("pt-BR"));

            dateWeek.Text = semanaDia;
        }

        private async void ExecuteRequest()
        {
            List<AulaModel> aulas = await apiService.GetAulasAsync(ApiUrl);
            if (aulas != null && aulas.Count > 0)
            {
                var aula = aulas[0];
                aulasListView.ItemsSource = aulas;
                if (aulas.Count > 0)
                {
                    lblDados.Text = aula.nmDisciplina;
                }
                else
                {
                    lblDados.Text = "Não há aulas hoje";
                }
            }
        }

        private async void RegistrarEntrada_Clicked(object sender, EventArgs e)
        {
            if (IsConnectedToCollegeNetwork())
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var hrReferencia = DateTime.Now.ToString("HH:mm:ss");

                var body = new
                {
                    idTipoRegistroPonto = "1",
                    idTurmaHorario = "4861927",
                    hrReferenciaPonto = "08:00:08",
                    deJustificativa = "",
                    idTipoJustificativaPonto = "0"
                };

                var json = JsonConvert.SerializeObject(body);

                var response = await client.PostAsync("https://ida.ceub.br/api/v1.0/pp/FolhaPonto/Registrar",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);

                    await DisplayAlert($"Sucesso", $"Saída registrada com sucesso às {body.hrReferenciaPonto}", "OK");
                }
                else
                {
                    Console.WriteLine($"A requisição falhou com o código de status {response.StatusCode}");
                    Console.WriteLine("Erro ao registrar ponto");
                }
            }
            else
            {
                await DisplayAlert("Erro", "Você só pode registrar o ponto quando estiver conectado à rede da faculdade.", "OK");
            }
        }

        private async void RegistrarSaida_Clicked(object sender, EventArgs e)
        {
            if (IsConnectedToCollegeNetwork())
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var exitTime = DateTime.Now.ToString("HH:mm:ss");

                var body = new
                {
                    idTipoRegistroPonto = "2",
                    idTurmaHorario = "4861927",
                    hrReferenciaPonto = "09:40:10",
                    deJustificativa = "",
                    idTipoJustificativaPonto = "0"
                };

                var json = JsonConvert.SerializeObject(body);

                var response = await client.PostAsync("https://ida.ceub.br/api/v1.0/pp/FolhaPonto/Registrar",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);

                    await DisplayAlert($"Sucesso", $"Saída registrada com sucesso às {body.hrReferenciaPonto}", "OK");
                }
                else
                {
                    Console.WriteLine($"A requisição falhou com o código de status {response.StatusCode}");
                    Console.WriteLine("Erro ao registrar ponto");
                }
            }
            else
            {
                await DisplayAlert("Erro", "Você só pode registrar o ponto quando estiver conectado à rede da faculdade.", "OK");
            }
        }

        private bool IsConnectedToCollegeNetwork()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("https://ida.ceub.br/api/v1.0/pp/FolhaPonto/Registrar");
                return reply != null && reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
