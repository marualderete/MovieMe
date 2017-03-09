using System;
using System.Collections.Generic;
using MovieMeApp.Helpers;
using MovieMeApp.Interfaces;
using MovieMeApp.Models;
using MovieMeApp.Services;

namespace MovieMeApp
{
	public partial class App
	{
		public App()
		{
		}

		public static void Initialize()
		{
			ServiceLocator.Instance.Register<IDataStore<MovieStoreModel>, CloudDataMovieStore>();
			ServiceLocator.Instance.Register<IAuthenticationStore, CloudAuthenticationStore>();

#if __IOS__
			ServiceLocator.Instance.Register<IMessageDialog, iOS.MessageDialog>();
#else
			ServiceLocator.Instance.Register<IMessageDialog, Droid.MessageDialog>();
#endif
		}

		public static IDictionary<string, string> LoginParameters => null;
	}
}
