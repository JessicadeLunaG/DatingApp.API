using DatingApp.API.DTOs;
using DatingApp.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace DatingApp.UnitTests.Test
{
    public class LikesControllerTests
    {
        private string apiRoute = "api/likes";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUrl;
        private string loginObjetct;
        private HttpContent httpContent;

        public LikesControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("NotFound", "lisa", "Pa$$w0rd", "bob")]
        public async Task AddLike_ShouldNotFound(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            requestUrl = "api/account/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            loginObjetct = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            var reponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            requestUrl = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("BadRequest", "lisa", "Pa$$w0rd", "lisa")]
        public async Task AddLike_ShouldBadRequest(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            requestUrl = "api/account/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            loginObjetct = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            var reponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            requestUrl = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", "todd")]
        public async Task AddLike_ShouldOK(string statusCode, string username, string password, string userLiked)
        {
            // Arrange
            requestUrl = "api/account/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            loginObjetct = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            var reponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            requestUrl = $"{apiRoute}/" + userLiked;

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "todd", "Pa$$w0rd")]
        public async Task GetUserLikes_ShouldOK(string statusCode, string username, string password)
        {
            // Arrange
            requestUrl = "api/account/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            loginObjetct = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            var reponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            requestUrl = $"{apiRoute}?predicated=likedBy";

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods

        private static string GetLoginObject(LoginDto loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };

            return entityObject.ToString();
        }

        private static StringContent GetHttpContent(string objectToCode)
        {
            return new StringContent(objectToCode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}
