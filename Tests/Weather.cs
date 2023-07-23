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

        public record WeatherItem(DateOnly Date, int TemperatureC);
    }
}