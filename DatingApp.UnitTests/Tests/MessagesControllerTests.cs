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
    public class MessagesControllerTests
    {
        private string apiRoute = "api/messages";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUrl;
        private string loginObjetct;
        private string messageObjetct;
        private HttpContent httpContent;

        public MessagesControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("NoContent", "lisa", "Pa$$w0rd", "bob", "lisa's message for Bob")]
        public async Task CreateMessage_ShoulNotFound(string statusCode, string username, string password, string recipientUsername, string content)
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

            requestUrl = $"{apiRoute}";
            var messageDto = new CreateMessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };

            messageObjetct = GetMessageObject(messageDto);
            httpContent = GetHttpContent(messageObjetct);

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("BadRequest", "lisa", "Pa$$w0rd", "lisa", "lisa's message for lisa")]
        public async Task CreateMessage_ShouldBadRequest(string statusCode, string username, string password, string recipientUsername, string content)
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

            requestUrl = $"{apiRoute}";
            var messageDto = new CreateMessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };

            messageObjetct = GetMessageObject(messageDto);
            httpContent = GetHttpContent(messageObjetct);

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "todd", "Pa$$w0rd", "karen", "karen's message for lisa")]
        public async Task CreateMessage_ShuldOK(string statusCode, string username, string password, string recipientUsername, string content)
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

            requestUrl = $"{apiRoute}";
            var messageDto = new CreateMessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };

            messageObjetct = GetMessageObject(messageDto);
            httpContent = GetHttpContent(messageObjetct);

            // Act
            httpResponse = await _client.PostAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd")]
        public async Task GetMessagesForUser_ShuldOK(string statusCode, string username, string password)
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

            requestUrl = $"{apiRoute}";

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", "Outbox")]
        public async Task GetMessagesForUserFromQuery_ShuldOK(string statusCode, string username, string password, string container)
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

            requestUrl = $"{apiRoute}?container=" + container;

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "karen", "Pa$$w0rd", "todd")]
        public async Task GetMessagesThread_ShuldOK(string statusCode, string username, string password, string usernameThread)
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

            requestUrl = $"{apiRoute}/thread/" + usernameThread;

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("Unauthorized", "lisa", "Pa$$w0rd", "todd", "lisa's message for todd", "karen")]
        public async Task DeleteMessage_ShuldUnauthorized(string statusCode, string username, string password, string recipientUsername, string content, string deleteUsername)
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

            requestUrl = $"{apiRoute}";
            var messageDto = new CreateMessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };

            messageObjetct = GetMessageObject(messageDto);
            httpContent = GetHttpContent(messageObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            reponse = await httpResponse.Content.ReadAsStringAsync();
            var messageDto2 = JsonSerializer.Deserialize<MessageDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            requestUrl = "api/account/login";
            loginDto = new LoginDto
            {
                Username = deleteUsername,
                Password = password
            };

            loginObjetct = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            reponse = await httpResponse.Content.ReadAsStringAsync();
            userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            requestUrl = $"{apiRoute}/" + messageDto2.Id;

            // Act
            httpResponse = await _client.DeleteAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", "todd", "lisa's message for todd")]
        public async Task DeleteMessage_ShuldOK(string statusCode, string username, string password, string recipientUsername, string content)
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

            requestUrl = $"{apiRoute}";
            var messageDto = new CreateMessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };

            messageObjetct = GetMessageObject(messageDto);
            httpContent = GetHttpContent(messageObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            reponse = await httpResponse.Content.ReadAsStringAsync();
            var messageDto2 = JsonSerializer.Deserialize<MessageDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            requestUrl = $"{apiRoute}/" + messageDto2.Id;

            // Act
            httpResponse = await _client.DeleteAsync(requestUrl);

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

        private static string GetMessageObject(CreateMessageDto createMessageDto)
        {
            var entityObject = new JObject()
            {
                { nameof(createMessageDto.RecipientUsername), createMessageDto.RecipientUsername },
                { nameof(createMessageDto.Content), createMessageDto.Content }
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
