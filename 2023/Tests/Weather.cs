namespace Tests
{
    [TestClass]
    public class Weather
    {
        [TestMethod]
        public async Task GetAndSum()
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("apikey", "myak1");
            using var response = await http.GetAsync("http://localhost:5109/weather");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<WeatherItem[]>();
            Assert.AreEqual(5, result!.Length);

            var sum = result.Aggregate(0, (a, w) => a + w.TemperatureC);
            Assert.AreNotEqual(0, sum);
        }

        [TestMethod]
        public async Task MirrorData() {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("apikey", "myak1");
            using var response = await http.PostAsync(
                requestUri: "http://localhost:5109/weather", 
                content: new [] 
                {
                    new WeatherItem(DateOnly.FromDateTime(DateTime.Today), 11)
                }.AsJsonBody()
            );
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<WeatherItem[]>();
            Assert.AreEqual(1, result!.Length);
            Assert.AreEqual(11, result.Single().TemperatureC);
            Assert.AreEqual(DateOnly.FromDateTime(DateTime.Now), result.Single().Date);
        }

        public record WeatherItem(DateOnly Date, int TemperatureC);
    }
}