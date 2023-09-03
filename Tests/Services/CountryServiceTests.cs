using System.Net;
using NSubstitute;
using System.Text.Json;
using countries.Models;
using countries.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace countries.Tests.Services
{
    public class CountryServiceTests
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CountryService> _logger;

        public CountryServiceTests()
        {
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _logger = Substitute.For<ILogger<CountryService>>();
        }

        private void SetupHttpClientFactory(HttpResponseMessage httpResponseMessage)
        {
            var httpClient = new HttpClient(new HttpMessageHandlerStub((request, cancellationToken) => httpResponseMessage));
            _httpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);
        }

        [Fact]
        public async Task GetAllCountriesAsync_ShouldReturnListOfCountries_WhenApiIsAvailable()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = new Name { Common = "USA" }, Population = 300000000 },
            new Country { Name = new Name { Common = "Canada" }, Population = 40000000 }
        };

            var json = JsonSerializer.Serialize(countries);
            SetupHttpClientFactory(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) });

            var service = new CountryService(_httpClientFactory, _logger);

            // Act
            var result = await service.GetAllCountriesAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllCountriesAsync_ShouldReturnEmptyList_WhenApiReturnsEmptyList()
        {
            // Arrange
            var countries = new List<Country>();
            var json = JsonSerializer.Serialize(countries);
            SetupHttpClientFactory(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) });

            var service = new CountryService(_httpClientFactory, _logger);

            // Act
            var result = await service.GetAllCountriesAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllCountriesAsync_ShouldThrowException_WhenApiIsUnavailable()
        {
            // Arrange
            SetupHttpClientFactory(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var service = new CountryService(_httpClientFactory, _logger);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetAllCountriesAsync());
        }

        [Fact]
        public async Task GetAllCountriesAsync_ShouldFilterByCountryName_WhenCountryNameIsProvided()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Name = new Name { Common = "USA" }, Population = 300000000 },
            new Country { Name = new Name { Common = "Canada" }, Population = 40000000 }
        };
            var json = JsonSerializer.Serialize(countries);
            SetupHttpClientFactory(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) });

            var service = new CountryService(_httpClientFactory, _logger);

            // Act
            var result = await service.GetAllCountriesAsync(countryName: "Canada");

            // Assert
            Assert.Single(result);
            Assert.Equal("Canada", result[0].Name!.Common);
        }
    }

    public class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> _sendAsync;

        public HttpMessageHandlerStub(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> sendAsync)
        {
            _sendAsync = sendAsync ?? throw new ArgumentNullException(nameof(sendAsync));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_sendAsync(request, cancellationToken));
        }
    }
}
