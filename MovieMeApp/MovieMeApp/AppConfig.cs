using System;

namespace MovieMeApp
{
	public static class AppConfig
	{
		
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


		//Filters
		//TODO: this must be in other place
		public static readonly string Movie = "movie";
		public static readonly string Search = "search";
		public static readonly string NowPlaying = "now_playing";
		public static readonly string TopRated = "top_rated";
		public static readonly string Popular = "popular";
		public static readonly string Similar = "similar";
		public static readonly string AppendToResponse = "&append_to_response=";

	}
}
