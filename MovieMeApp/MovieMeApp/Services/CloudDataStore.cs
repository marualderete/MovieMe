using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieMeApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;

namespace MovieMeApp.Services
{
	public class CloudDataStore : IDataStore<Item>
	{
		HttpClient client;
		IEnumerable<Item> items;

		public CloudDataStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{AppConfig.DataStoreBaseURL}/");

			items = new List<Item>();
		}

		public async Task<IEnumerable<Item>> GetItemsAsync(string filter)
		{
			//public static readonly string DataStoreBaseURL = "https://api.themoviedb.org/3/";
			//public static readonly string DataStoreSearchURL = "movie/{0}?/api_key={1}{2}{3}";
			//public static readonly string DataStoreMovieImageURL = "https://image.tmdb.org/t/p/{0}/{1}"
			////http://api.themoviedb.org/3/movie/now_playing?api_key=ab41356b33d100ec61e6c098ecc92140&amp;sort_by=popularity.des
			/// public static readonly string SortByPopularity = "sort_by=popularity.des";
			//public static readonly string AppendToResponse = "&append_to_response=";

			var url = string.Format(AppConfig.DataStoreSearchURL, filter, AppConfig.DataStoreApiKey, "&amp;", AppConfig.SortByPopularity);
			var json = await client.GetStringAsync(url);
			var movies = await Task.Run(() => JsonConvert.DeserializeObject<MovieStoreModel>(json));


			return items;
		}

		public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
		{
			if (forceRefresh && CrossConnectivity.Current.IsConnected)
			{
				var json = await client.GetStringAsync($"api/item");
				items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Item>>(json));
			}

			return items;
		}

		public async Task<Item> GetItemAsync(string id)
		{
			if (id != null && CrossConnectivity.Current.IsConnected)
			{
				var json = await client.GetStringAsync($"api/item/{id}");
				items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Item>>(json));
			}

			return null;
		}

		public async Task<bool> AddItemAsync(Item item)
		{
			if (item == null || !CrossConnectivity.Current.IsConnected)
				return false;

			var serializedItem = JsonConvert.SerializeObject(item);

			var response = await client.PostAsync($"api/item", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

			return response.IsSuccessStatusCode ? true : false;
		}

		public async Task<bool> UpdateItemAsync(Item item)
		{
			if (item == null || item.Id == null || !CrossConnectivity.Current.IsConnected)
				return false;

			var serializedItem = JsonConvert.SerializeObject(item);
			var buffer = System.Text.Encoding.UTF8.GetBytes(serializedItem);
			var byteContent = new ByteArrayContent(buffer);

			var response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

			return response.IsSuccessStatusCode ? true : false;
		}

		public async Task<bool> DeleteItemAsync(string id)
		{
			if (string.IsNullOrEmpty(id) && !CrossConnectivity.Current.IsConnected)
				return false;

			var response = await client.DeleteAsync($"api/item/{id}");

			return response.IsSuccessStatusCode ? true : false;
		}
	}
}
