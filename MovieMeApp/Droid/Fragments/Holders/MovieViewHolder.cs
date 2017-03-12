using System;
using System.Collections.ObjectModel;

using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.App;
using Android.Content;

using MovieMeApp.ViewModels;
using MovieMeApp.Services;
using MovieMeApp.Utils;
using MovieMeApp.Droid.Activities;


using Square.Picasso;

namespace MovieMeApp.Droid.Fragments.Holders
{
	#region HOLDERS

	/// <summary>
	/// Movie view holder.
	/// </summary>
	public class MovieViewHolder : RecyclerView.ViewHolder
	{
		#region public properties
		public ImageView MovieCover { get; set; }
		#endregion

		#region constructor
		public MovieViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
							Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
		{
			MovieCover = itemView.FindViewById<ImageView>(Resource.Id.movie_cover);
			MovieCover.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
		}
		#endregion
	}

	#endregion

}
