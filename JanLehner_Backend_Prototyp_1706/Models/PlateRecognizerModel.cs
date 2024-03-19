using Newtonsoft.Json;

namespace JanLehner_Backend_Prototyp_1706.Models
{
    public class PlateRecognizerModel
    {
        public static async Task<string> RecognizePlate(string imagePath)
        {
            string numberPlate = "";
            string apiEndpoint = "https://api.platerecognizer.com/v1/plate-reader/";
            string apiKey = Environment.GetEnvironmentVariable("Apikey");


            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Token {apiKey}");

                byte[] imageData = File.ReadAllBytes(imagePath);
                string base64Image = Convert.ToBase64String(imageData);

                var requestData = new MultipartFormDataContent();
                requestData.Add(new StringContent(base64Image), "upload");

                HttpResponseMessage response = await client.PostAsync(apiEndpoint, requestData);

                if (response.IsSuccessStatusCode)
                {
                    Root deserializedClass = JsonConvert.DeserializeObject<Root>(await response.Content.ReadAsStringAsync());
                    Result result = deserializedClass.results.FirstOrDefault();
                    numberPlate = result.plate;
                    Console.WriteLine(numberPlate);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    numberPlate = $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }

                return numberPlate;
            }
        }
    }
}
