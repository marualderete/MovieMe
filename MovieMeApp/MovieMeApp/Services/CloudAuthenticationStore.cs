using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieMeApp.Services
{
	public class CloudAuthenticationStore : IAuthenticationStore
	{
		HttpClient client;
		string requestToken;
		string sessionID;

		public CloudAuthenticationStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{AppConfig.DataStoreBaseURL}/");
			requestToken = string.Empty;
		}

		public string RequestToken 
		{ 
			get 
			{ 
				return requestToken; 
			} 
		}
		public string SessionID
		{
			get
			{
				return sessionID;
			}
		}

		private void Dispose()
		{
			client.Dispose();
		}

		async Task<string> GetRequestToken()
		{
			//TODO: change appconfig string url's
			var url = string.Format(AppConfig.GetRequestTokenURL, AppConfig.DataStoreApiKey);

			var json = await client.GetStringAsync(url);

			var result = await Task.Run(() => JsonConvert.DeserializeObject(json));
			var isOK = (bool) JObject.Parse(result.ToString())["success"];

			if (!isOK)
			{
				return string.Empty;
			}

			return JObject.Parse(result.ToString())["request_token"].ToString();
		}

		public async Task<bool> NewLogin(string username, string password)
		{
			requestToken = await GetRequestToken();

			//TODO: change appconfig string url's
			var validationURL = string.Format(AppConfig.ValidateLoginURL, AppConfig.DataStoreApiKey, username, password, requestToken);
			var jsonFromValidation = await client.GetStringAsync(validationURL);

			var validation = await Task.Run(() => JsonConvert.DeserializeObject(jsonFromValidation));

			var isOK = (bool)JObject.Parse(validation.ToString())["success"];

			if (!isOK)
			{
				return false;
			}
			client.CancelPendingRequests();

			//if validation is ok, then create a session:  
			await CreateNewSession();

			return true;
		}

		public async Task<bool> CreateNewSession()
		{
			if (string.IsNullOrEmpty(requestToken))
			{
				return false;
			}

			var sessionURL = string.Format(AppConfig.GetSessionURL, AppConfig.DataStoreApiKey, requestToken);
			var sessionResult = await client.GetStringAsync(sessionURL);
			var session = await Task.Run(() => JsonConvert.DeserializeObject(sessionResult));

			var isSessionStarted = (bool)JObject.Parse(session.ToString())["success"];

			if (!isSessionStarted)
			{
				return false;
			}

			sessionID = JObject.Parse(session.ToString())["session_id"].ToString();
			return true;
		}

		//llamar al valid_with_login
		public Task<bool> IsValidSession()
		{
			throw new NotImplementedException();
		}
	}
}
