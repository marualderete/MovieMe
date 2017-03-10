using System;
using System.Collections.Generic;
using System.Linq;
using MovieMeApp.Helpers;
using MovieMeApp.Interfaces;
using Newtonsoft.Json;

namespace MovieMeApp.Models
{
	public class AccountModel : ObservableObject, IBaseDataObject
	{
		/// <summary>
		/// Id for Movie
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// The poster path.
		/// </summary>
		string iso_639_1 = string.Empty;

		[JsonProperty(PropertyName = "iso_639_1")]
		public string Iso_639_1
		{
			get { return iso_639_1; }
			set { SetProperty(ref iso_639_1, value); }
		}

		/// <summary>
		/// The poster path.
		/// </summary>
		string iso_3166_1 = string.Empty;

		[JsonProperty(PropertyName = "iso_3166_1")]
		public string Iso_3166_1
		{
			get { return iso_3166_1; }
			set { SetProperty(ref iso_3166_1, value); }
		}

		/// <summary>
		/// The poster path.
		/// </summary>
		string name = string.Empty;

		[JsonProperty(PropertyName = "name")]
		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}

		/// <summary>
		/// The poster path.
		/// </summary>
		string username = string.Empty;

		[JsonProperty(PropertyName = "username")]
		public string Username
		{
			get { return username; }
			set { SetProperty(ref username, value); }
		}

		/// <summary>
		/// The poster path.
		/// </summary>
		bool include_adult = false;

		[JsonProperty(PropertyName = "include_adult")]
		public bool IncludeAdult
		{
			get { return include_adult; }
			set { SetProperty(ref include_adult, value); }
		}

		/// <summary>
		/// The genre identifiers.
		/// </summary>
		List<string> avatar = new List<string>();

		[JsonProperty(PropertyName = "avatar")]
		public string[] Avatar
		{
			get { return avatar.ToArray(); }
			set { SetProperty(ref avatar, value.ToList()); }
		}
	}
}
