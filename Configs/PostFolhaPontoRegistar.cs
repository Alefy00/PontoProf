namespace Ponto.Configs
{

    public class PostFolhaPontoRegistar
    {
        private readonly HttpClient httpClient;
        private string authToken;
        public PostFolhaPontoRegistar(string authToken)
        {
            this.authToken = authToken;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var hrReferencia = DateTime.Now.ToString("HH:mm:ss");



        }




    }
}
