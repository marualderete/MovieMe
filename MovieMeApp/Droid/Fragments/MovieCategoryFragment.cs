using System;
using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MovieMeApp.Droid.Activities;
using MovieMeApp.ViewModels;

namespace MovieMeApp.Droid.Fragments
{
	/// <summary>
	/// Movie category fragment.
	/// </summary>
	public class MovieCategoryFragment : Android.App.Fragment, IFragmentVisible
	{
		#region private properties

		MovieCategoryAdapter adapter;
		SwipeRefreshLayout refresher;
		ProgressBar progress;

		#endregion

		#region public properties
		public static MovieCategoryFragment NewInstance() =>
			new MovieCategoryFragment { Arguments = new Bundle() };

		public MovieExplorerViewModel ViewModel
		{
			get;
			set;
		}

		public void BecameVisible()
		{
		}

		#endregion

		#region override methods for MovieCategoryFragment
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.movie_category_list, container, false);

			refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
			refresher.SetColorSchemeColors(Resource.Color.accent);
			refresher.Refreshing = true;

			progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_frame_container);
			progress.Visibility = ViewStates.Visible;

			var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			recyclerView.HasFixedSize = true;
			recyclerView.SetAdapter(adapter = new MovieCategoryAdapter(Activity, ViewModel));

			progress.Visibility = ViewStates.Gone;
			refresher.Refreshing = false;
			return view;
		}

		public override void OnStart()
		{
			base.OnStart();

			refresher.Refresh += Refresher_Refresh;
			//adapter.ItemClick += Adapter_ItemClick;
		}

		public override void OnStop()
		{
			base.OnStop();
			refresher.Refresh -= Refresher_Refresh;
			//adapter.ItemClick -= Adapter_ItemClick;
		}

		#endregion

		#region private methods
		//void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
		//{
		//	var item = ViewModel.Categories[e.Position];
		//	var intent = new Intent(Activity, typeof(BrowseItemDetailActivity));

		//	intent.PutExtra("data", Newtonsoft.Json.JsonConvert.SerializeObject(item));
		//	Activity.StartActivity(intent);
		//}

		void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
		}

		async void Refresher_Refresh(object sender, EventArgs e)
		{
			refresher.Refreshing = true;

			ViewModel.Categories.Clear();
			await ViewModel.LoadModels(AppConfig.TopRated);
			await ViewModel.LoadModels(AppConfig.Popular);
			await ViewModel.LoadModels(AppConfig.NowPlaying);

			refresher.Refreshing = false;
		}

		#endregion
	}
}
