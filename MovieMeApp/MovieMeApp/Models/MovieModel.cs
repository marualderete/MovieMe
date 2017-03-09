using System;
using System.Collections.Generic;
using System.Linq;
using MovieMeApp.Helpers;
using MovieMeApp.Interfaces;
using Newtonsoft.Json;

namespace MovieMeApp
{
	/// <summary>
	/// Movie model.
	/// </summary>
	public class MovieModel : ObservableObject, IBaseDataObject
	{
		/// <summary>
		/// Id for Movie
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// The poster path.
		/// </summary>
		string poster_path = string.Empty;

		[JsonProperty(PropertyName = "poster_path")]
		public string Poster_Path
		{
			get { return poster_path; }
			set { SetProperty(ref poster_path, value); }
		}

		/// <summary>
		/// The adult.
		/// </summary>
		bool adult = false;

		[JsonProperty(PropertyName = "adult")]
		public bool Adult
		{
			get { return adult; }
			set { SetProperty(ref adult, value); }
		}

		/// <summary>
		/// The overview.
		/// </summary>
		string overview = string.Empty;

		[JsonProperty(PropertyName = "overview")]
		public string Overview
		{
			get { return overview; }
			set { SetProperty(ref overview, value); }
		}

		/// <summary>
		/// The release date.
		/// </summary>
		DateTime release_date = new DateTime();

		[JsonProperty(PropertyName = "release_date")]
		public DateTime ReleaseDate
		{
			get { return release_date; }
			set { SetProperty(ref release_date, value); }
		}

		/// <summary>
		/// The genre identifiers.
		/// </summary>
		List<int> genre_ids = new List<int>();

		[JsonProperty(PropertyName = "genre_ids")]
		public int[] GenreIds
		{
			get { return genre_ids.ToArray(); }
			set { SetProperty(ref genre_ids, value.ToList()); }
		}

		/// <summary>
		/// The original title.
		/// </summary>
		string original_title = string.Empty;

		[JsonProperty(PropertyName = "original_title")]
		public string OriginalTitle
		{
			get { return original_title; }
			set { SetProperty(ref original_title, value); }
		}

		/// <summary>
		/// The original language.
		/// </summary>
		string original_language = string.Empty;

		[JsonProperty(PropertyName = "original_language")]
		public string OriginalLanguage
		{
			get { return original_language; }
			set { SetProperty(ref original_language, value); }
		}

		/// <summary>
		/// The title.
		/// </summary>
		string title = string.Empty;

		[JsonProperty(PropertyName = "title")]
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}

		/// <summary>
		/// The backdrop path.
		/// </summary>
		string backdrop_path = string.Empty;

		[JsonProperty(PropertyName = "backdrop_path")]
		public string BackdropPath
		{
			get { return backdrop_path; }
			set { SetProperty(ref backdrop_path, value); }
		}

		/// <summary>
		/// The popularity.
		/// </summary>
		decimal popularity = 0m;

		[JsonProperty(PropertyName = "popularity")]
		public decimal Popularity
		{
			get { return popularity; }
			set { SetProperty(ref popularity, value); }
		}

		/// <summary>
		/// The vote count.
		/// </summary>
		int vote_count = 0;

		[JsonProperty(PropertyName = "vote_count")]
		public int VoteCount
		{
			get { return vote_count; }
			set { SetProperty(ref vote_count, value); }
		}

		/// <summary>
		/// The video.
		/// </summary>
		bool video = false;

		[JsonProperty(PropertyName = "video")]
		public bool Video
		{
			get { return video; }
			set { SetProperty(ref video, value); }
		}

		/// <summary>
		/// The video.
		/// </summary>
		double vote_storage = 0;

		[JsonProperty(PropertyName = "vote_storage")]
		public double VoteStorage
		{
			get { return vote_storage; }
			set { SetProperty(ref vote_storage, value); }
		}
	}
}
