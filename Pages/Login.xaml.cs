namespace Ponto.Pages
{
    //Login:084354
    //Senha 084354@@
    public partial class Login : ContentPage
    {
        private string authToken;

        private const string apiUrl = "https://ida.ceub.br/api/v1.0/pp/Credencial";
        private const string username = "PP.CEUB.BR";
        private const string password = "JoBonizm9uId/uM1ED3Q7WKtr9umltxBUl+UepBHF2A=";

        public List<AulaModel> Aula;

        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false); // Onde 'this' � a p�gina atual
            //FlyoutPage.IsPresentedProperty.Equals(false);

        }
        private async Task<string> Authenticate(string matricula, string senha)
        {
            var httpClient = new HttpClient();

            // Configurar o cabe�alho de autentica��o b�sica
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));

            // Configurar o corpo da requisi��o
            var requestData = new
            {
                drt = matricula,
                senha
            };

            var dataJson = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            // Enviar a solicita��o de autentica��o
            var response = await httpClient.PostAsync(apiUrl, dataJson);

            if (response.IsSuccessStatusCode)
            {
                // Ler o conte�do da resposta
                var content = await response.Content.ReadAsStringAsync();

                // Desserializar o JSON para um objeto AuthenticationResponse
                var authenticationResponse = JsonSerializer.Deserialize<AuthenticationResponse>(content);

                // Retornar o token JWT
                return authenticationResponse.response;
            }
            else
            {
                // Ler o conte�do da resposta
                var content = await response.Content.ReadAsStringAsync();

                // Desserializar o JSON para um objeto AuthenticationResponse
                var authenticationResponse = JsonSerializer.Deserialize<AuthenticationResponse>(content);

                // Lan�ar uma exce��o com a mensagem de erro da API
                throw new Exception(authenticationResponse.response);
            }
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Ler os valores dos campos de texto
                var matricula = txtMatricula.Text;
                var senha = txtSenha.Text;

                // Autenticar o usu�rio e obter o token JWT
                authToken = await Authenticate(matricula, senha);

                // Navegar para a p�gina Home    
                //await Navigation.PushAsync(new Home(authToken));
                await Navigation.PushAsync(new Home(authToken, Aula));
                //await Navigation.PushAsync(new AppFlyout(authToken));
            }
            catch (Exception ex)
            {
                // Exibir uma mensagem de erro


                _ = DisplayAlert("Erro", ex.Message, "OK");

                // Limpar o campo de senha
                ClearPasswordField();
            }
        }

        private void ClearPasswordField()
        {
            txtSenha.Text = string.Empty;
        }
    }
}
