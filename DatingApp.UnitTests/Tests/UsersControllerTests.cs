using DatingApp.API.DTOs;
using DatingApp.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Xunit;

namespace DatingApp.UnitTests.Test
{
    public class UsersControllerTests
    {
        private string apiRoute = "api/users";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUrl;
        private string loginObjetct;
        private string memberObjetct;
        private HttpContent httpContent;

        public UsersControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd")]
        public async Task GetUsers_ShouldOK(string statusCode, string username, string password)
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
        [InlineData("OK", "admin", "Pa$$w0rd")]
        public async Task GetUserByUsername_ShouldOK(string statusCode, string username, string password)
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

            requestUrl = $"{apiRoute}/" + username;

            // Act
            httpResponse = await _client.GetAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NoContent", "lisa", "Pa$$w0rd", "IntroductionU", "LookingForU", "InterestsU", "CityU", "CountryU")]
        public async Task UpdateUser_ShouldNoContent(string statusCode, string username, string password, string introduction, string lookingFor, string interests, string city, string country)
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
            var memberDto = new MemberDto
            {
                Introduction = introduction,
                Interests = interests,
                LookingFor = lookingFor,
                City = city,
                Country = country
            };

            memberObjetct = GetMemberObject(memberDto);
            httpContent = GetHttpContent(memberObjetct);

            // Act
            httpResponse = await _client.PutAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("Created", "karen", "Pa$$w0rd", "D:/Repositorio/DatingApp.API/DatingApp.UnitTests/1.jpg")]
        public async Task AddPhoto_ShouldCreated(string statusCode, string username, string password, string namePhoto)
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

            MultipartFormDataContent formDataContent = new MultipartFormDataContent();
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync(namePhoto);
            var stream = await storageFile.OpenStreamForReadAsync();

            httpContent = new StreamContent(stream);

            formDataContent.Add(httpContent, "File", namePhoto);

            requestUrl = $"{apiRoute}/add-photo";

            // Act
            httpResponse = await _client.PostAsync(requestUrl, formDataContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NoContent", "lisa", "Pa$$w0rd", "D:/Repositorio/DatingApp.API/DatingApp.UnitTests/1.jpg")]
        public async Task SetMainPhoto_ShouldOK(string statusCode, string username, string password, string namePhoto)
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

            MultipartFormDataContent formDataContent = new MultipartFormDataContent();
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync(namePhoto);
            var stream = await storageFile.OpenStreamForReadAsync();

            httpContent = new StreamContent(stream);

            formDataContent.Add(httpContent, "File", namePhoto);

            requestUrl = $"{apiRoute}/add-photo";

            httpResponse = await _client.PostAsync(requestUrl, formDataContent);
            reponse = await httpResponse.Content.ReadAsStringAsync();
            var photoDto = JsonSerializer.Deserialize<PhotoDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            requestUrl = $"{apiRoute}/photos/" + photoDto.Id;

            // Act
            httpResponse = await _client.PutAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", "D:/Repositorio/DatingApp.API/DatingApp.UnitTests/1.jpg")]
        public async Task DeletePhoto_ShouldOK(string statusCode, string username, string password, string namePhoto)
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

            MultipartFormDataContent formDataContent = new MultipartFormDataContent();
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync(namePhoto);
            var stream = await storageFile.OpenStreamForReadAsync();

            httpContent = new StreamContent(stream);

            formDataContent.Add(httpContent, "File", namePhoto);

            requestUrl = $"{apiRoute}/add-photo";

            httpResponse = await _client.PostAsync(requestUrl, formDataContent);
            reponse = await httpResponse.Content.ReadAsStringAsync();
            var photoDto = JsonSerializer.Deserialize<PhotoDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            requestUrl = $"{apiRoute}/photos/" + photoDto.Id;

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

        private static string GetMemberObject(MemberDto memberDto)
        {
            var entityObject = new JObject()
            {
                { nameof(memberDto.Introduction), memberDto.Introduction },
                { nameof(memberDto.LookingFor), memberDto.LookingFor },
                { nameof(memberDto.Interests), memberDto.Interests },
                { nameof(memberDto.City), memberDto.City },
                { nameof(memberDto.Country), memberDto.Country }
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
