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

		public async Task<bool> ValidateLogin(string username, string password)
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
			//if validation is ok, then create a session:  
			var sessionURL = string.Format(AppConfig.GetSessionURL, AppConfig.DataStoreApiKey, requestToken);
			client.CancelPendingRequests();
			var sessionResult = await client.GetStringAsync(sessionURL);
			var session = await Task.Run(() => JsonConvert.DeserializeObject(sessionResult));

			var isSessionStarted = (bool) JObject.Parse(session.ToString())["success"];

			if (!isSessionStarted)
			{
				return false;
			}

			sessionID = JObject.Parse(session.ToString())["session_id"].ToString();

			client.Dispose();
			return true;
		}
	}
}
