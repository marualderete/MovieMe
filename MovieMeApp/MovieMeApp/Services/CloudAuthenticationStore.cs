using System;
using System.Net.Http;
using System.Threading.Tasks;
using MovieMeApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieMeApp.Services
{
	/// <summary>
	/// Cloud authentication store.
	/// </summary>
	public class CloudAuthenticationStore : IAuthenticationStore
	{
		#region private properties
		HttpClient client;
		string requestToken;
		string sessionID;
		string accountID;
		#endregion

		#region constructor
		public CloudAuthenticationStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{AppConfig.DataStoreBaseURL}/");
			requestToken = string.Empty;
		}
		#endregion

		#region private methods

		private void Dispose()
		{
			client.Dispose();
		}

		/// <summary>
		/// Gets the request token.
		/// </summary>
		/// <returns>The request token.</returns>
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
		#endregion

		#region public methods
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

		public string AccountID
		{
			get
			{
				return accountID;
			}
		}

		/// <summary>
		/// News the login. This method consume the API to validate the user according to the request 
		/// already given by other method and according to user/passw given.
		/// </summary>
		/// <returns>The login.</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public async Task<bool> NewLogin(string username, string password)
		{
			requestToken = await GetRequestToken();

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

		/// <summary>
		/// Creates the new session. This method consumes the API to create a new session 
		/// </summary>
		/// <returns>The new session.</returns>
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
			client.CancelPendingRequests();
			await GetAccount();

			return true;
		}

		public async Task<bool> GetAccount()
		{
			var getAccountURL = string.Format(AppConfig.GetAccountURL, AppConfig.DataStoreApiKey, SessionID);
			var json = await client.GetStringAsync(getAccountURL);

			await Task.Run(() => JsonConvert.DeserializeObject(json));

			var account = await Task.Run(() => JsonConvert.DeserializeObject<AccountModel>(json));

			if (account == null)
			{
				return false;
			}

			accountID = account.Id;
			return true;
		}

		//llamar al valid_with_login
		public Task<bool> IsValidSession()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
