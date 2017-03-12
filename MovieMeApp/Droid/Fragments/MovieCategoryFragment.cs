using System;
using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MovieMeApp.Droid.Activities;
using MovieMeApp.Droid.Fragments;
using MovieMeApp.Droid.Fragments.Adapters;
using MovieMeApp.ViewModels;

namespace MovieMeApp.Droid.Fragments
{
	/// <summary>
	/// Movie category fragment.
	/// </summary>
	public class MovieCategoryFragment<T> : Android.App.Fragment, IFragmentVisible where T: BaseViewModel
	{
		#region private properties

		BaseRecycleViewAdapter _adapter;
		T _viewModel;
		SwipeRefreshLayout _refresher;
		ProgressBar _progress;
		Action _onRefresh;

		#endregion

		#region public properties

		public void BecameVisible()
		{
		}

		#endregion

		#region constructor

		public MovieCategoryFragment (T viewModel, BaseMovieCategoryAdapter<T> adapter, Action onRefresh = null)
		{
			_viewModel = viewModel;
			_adapter = adapter;
			_onRefresh = onRefresh;
		}

		#endregion

		#region override methods for MovieCategoryFragment
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.movie_category_list, container, false);

			if (_onRefresh != null)
			{
				_refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
				_refresher.SetColorSchemeColors(Resource.Color.accent);
			}

			Refreshing(true);

			//TODO: see if I need this in detail page
			_progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_frame_container);
			_progress.Visibility = ViewStates.Visible;

			var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			recyclerView.HasFixedSize = true;
			recyclerView.SetAdapter(_adapter);

			_progress.Visibility = ViewStates.Gone;

			Refreshing(false);
			return view;
		}

		public override void OnStart()
		{
			base.OnStart();
			if (_onRefresh != null)
			{
				_refresher.Refresh += Refresher_Refresh;
			}
		}

		public override void OnStop()
		{
			base.OnStop();
			if (_onRefresh != null)
			{
				_refresher.Refresh -= Refresher_Refresh;
			}
		}

		#endregion

		#region private methods

		void Refreshing(bool refreshing)
		{
			if (_onRefresh != null)
			{
				_refresher.Refreshing = refreshing;
			}
		}

		void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
		}

		async void Refresher_Refresh(object sender, EventArgs e)
		{
			if (_onRefresh != null)
			{
				_refresher.Refreshing = true;
				_onRefresh();
				_refresher.Refreshing = false;
			}
		}

		#endregion
	}
}
