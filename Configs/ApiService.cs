namespace Ponto.Configs
{
    public class ApiService
    {
        private readonly HttpClient httpClient;
        private string authToken;

        public ApiService(string authToken)
        {
            this.authToken = authToken;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }


        public async Task<List<AulaModel>> GetAulasAsync(string apiUrl)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dados = JsonSerializer.Deserialize<AulaResponse>(content);
                    return dados.response;
                }
                else
                {
                    // Lidar com erros
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



    }
}
