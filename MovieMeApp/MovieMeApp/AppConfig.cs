﻿using System;

namespace MovieMeApp
{
	public static class AppConfig
	{
		
		public static readonly string ServiceURL = "https://localhost:5000";

		public static readonly string DataStoreApiKey = "ab41356b33d100ec61e6c098ecc92140";

		//SEARCH FILTERS
		public static readonly string DataStoreBaseURL = "https://api.themoviedb.org/3/";
		public static readonly string DataStoreSearchURL = "https://api.themoviedb.org/3/movie/{0}?api_key={1}&{2}";
		public static readonly string DataStoreMovieImageURL = "https://image.tmdb.org/t/p/{0}{1}";



		//AUTHENTICATION URLS
		public static readonly string GetRequestTokenURL = "https://api.themoviedb.org/3/authentication/token/new?api_key={0}";
		public static readonly string ValidateLoginURL = "https://api.themoviedb.org/3/authentication/token/validate_with_login?api_key={0}&username={1}&password={2}&request_token={3}";
		public static readonly string GetSessionURL = "https://api.themoviedb.org/3/authentication/session/new?api_key={0}&request_token={1}";
		public static readonly string PostFavoriteURL = "https://api.themoviedb.org/3/account/{0}/favorite?api_key={1}&session_id={2}";
		public static readonly string GetAccountURL = "https://api.themoviedb.org/3/account?api_key={0}&session_id={1}";
		public static readonly string GetSimilarMoviesURL = "https://api.themoviedb.org/3/movie/{0}/similar?api_key={1}&language=en-US";
		//https://api.themoviedb.org/3/movie/1111/similar?api_key=asdf&language=en-US&page=1
		//https://api.themoviedb.org/3/account?api_key=dfdfdfd&session_id=ddfdf
		//https://api.themoviedb.org/3/account/9999/favorite?api_key=ab41356b33d100ec61e6c098ecc92140&session_id=9999


		//Filters
		//TODO: this must be in other place
		public static readonly string Movie = "movie";
		public static readonly string Search = "search";
		public static readonly string NowPlaying = "now_playing";
		public static readonly string TopRated = "top_rated";
		public static readonly string Popular = "popular";
		public static readonly string Similar = "similar";
		public static readonly string AppendToResponse = "&append_to_response=";



			//searches
			//https://api.themoviedb.org/3/search/movie?api_key={api_key}&query=Jack+Reacher
			//nopw playing
			//http://api.themoviedb.org/3/movie/now_playing?api_key=ab41356b33d100ec61e6c098ecc92140&amp;sort_by=popularity.des
			//top rated
			//http://api.themoviedb.org/3/movie/top_rated?api_key=ab41356b33d100ec61e6c098ecc92140&amp;sort_by=popularity.des
			//popular
			//http://api.themoviedb.org/3/movie/popular?api_key=ab41356b33d100ec61e6c098ecc92140&amp;sort_by=popularity.des

			//similar
			//http://api.themoviedb.org/3/movie/293660/similar?api_key=ab41356b33d100ec61e6c098ecc92140

			//videos
			//http://api.themoviedb.org/3/movie/293660/videos?api_key=ab41356b33d100ec61e6c098ecc92140

			//append to response (multiple requests)
			//http://api.themoviedb.org/3/movie/157336?api_key={api_key}&append_to_response=videos,images

			//images url https://image.tmdb.org/t/p/w500/kqjL17yufvn9OVLyXYpvtyrFfak.jpg

	}
}
