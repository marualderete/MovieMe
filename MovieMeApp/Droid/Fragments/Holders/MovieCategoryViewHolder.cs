using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace MovieMeApp.Droid.Fragments.Holders
{
	/// <summary>
	/// Movie category view holder.
	/// </summary>
	public class MovieCategoryViewHolder : RecyclerView.ViewHolder
	{
		#region public properties
		public TextView CategoryTitle { get; set; }
		public RecyclerView CategoryRecyclerView { get; set; }
		#endregion

		#region constructor
		public MovieCategoryViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
							Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
		{
			CategoryTitle = itemView.FindViewById<TextView>(Resource.Id.movie_title);
			CategoryRecyclerView = itemView.FindViewById<RecyclerView>(Resource.Id.category_recycler_view);

		}
		#endregion
	}

}
