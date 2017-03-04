using System;
using System.Collections.Generic;

namespace MovieMeApp
{
	public partial class App
	{
		public static bool AzureNeedsSetup => AzureMobileAppUrl == "https://CONFIGURE-THIS-URL.azurewebsites.net";
		public static string AzureMobileAppUrl = "http://localhost:5000";

		public App()
		{
		}

		public static void Initialize()
		{
			if (AzureNeedsSetup)
				ServiceLocator.Instance.Register<IDataStore<Item>, MockDataStore>();
			else
				ServiceLocator.Instance.Register<IDataStore<Item>, CloudDataStore>();

#if __IOS__
			ServiceLocator.Instance.Register<IMessageDialog, iOS.MessageDialog>();
#else
			ServiceLocator.Instance.Register<IMessageDialog, Droid.MessageDialog>();
#endif
		}

		public static IDictionary<string, string> LoginParameters => null;
	}
}
