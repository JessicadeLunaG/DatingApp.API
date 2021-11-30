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
    public class AdminControllerTests
    {
        private string apiRoute = "api/admin";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUrl;
        private string loginObjetct;
        private HttpContent httpContent;

        public AdminControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("OK", "admin", "Pa$$w0rd")]
        public async Task GetUsersWithRoles_ShouldOK(string statusCode, string username, string password)
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

            requestUrl = $"{apiRoute}/users-with-roles";

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NotFound", "admin", "Pa$$w0rd", "bob", "Moderator,Member")]
        public async Task EditRoles_ShouldNotFound(string statusCode, string username, string password, string username2, string roles)
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

            requestUrl = $"{apiRoute}/edit-roles/" + username2 + "?roles=" + roles;

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "admin", "Pa$$w0rd", "lisa", "Moderator,Member")]
        public async Task EditRoles_ShouldOK(string statusCode, string username, string password, string username2, string roles)
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

            requestUrl = $"{apiRoute}/edit-roles/" + username2 + "?roles=" + roles;

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "admin", "Pa$$w0rd")]
        public async Task GetPhotosForModeration_ShouldOK(string statusCode, string username, string password)
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

            requestUrl = $"{apiRoute}/photos-to-moderate";

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
