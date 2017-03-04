using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Graphics;

namespace MovieMeApp.Droid
{
	[Activity(Label = "@string/login",
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class LoginActivity : BaseActivity
	{
		/// <summary>
		/// Specify the layout to inflace
		/// </summary>
		protected override int LayoutResource => Resource.Layout.activity_login;

		Button signInButton, notNowButton;
		LinearLayout signingInPanel;
		ProgressBar progressBar;
		LoginViewModel viewModel;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			//Layout gets inflated here
			base.OnCreate(savedInstanceState);

			viewModel = new LoginViewModel();

			signInButton = FindViewById<Button>(Resource.Id.button_signin);
			notNowButton = FindViewById<Button>(Resource.Id.button_not_now);

			signInButton.Text = "Sign In";

			progressBar = FindViewById<ProgressBar>(Resource.Id.progressbar_signin);
			signingInPanel = FindViewById<LinearLayout>(Resource.Id.container_signin);

			progressBar.Indeterminate = false;
			signingInPanel.Visibility = ViewStates.Invisible;

			//Turn off back arrows
			SupportActionBar.SetDisplayHomeAsUpEnabled(false);
			SupportActionBar.SetHomeButtonEnabled(false);

			// viewModel.PropertyChanged += ViewModel_PropertyChanged;
		}

		protected override void OnStart()
		{
			base.OnStart();
			signInButton.Click += SignInButton_Click;
			notNowButton.Click += NotNowButton_Click;
		}

		protected override void OnStop()
		{
			base.OnStop();
			signInButton.Click -= SignInButton_Click;
			notNowButton.Click -= NotNowButton_Click;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			// viewModel.PropertyChanged -= ViewModel_PropertyChanged;
		}

		private void NotNowButton_Click(object sender, System.EventArgs e)
		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.ClearTop);
			StartActivity(intent);
			Finish();
		}

		private async void SignInButton_Click(object sender, System.EventArgs e)
		{
			// await viewModel.SignIn();
		}

		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			//    RunOnUiThread(() =>
			//    {
			//        switch(e.PropertyName)
			//        {
			//            case nameof(viewModel.IsBusy):
			//                {
			//                    if (viewModel.IsBusy)
			//                    {
			//                        progressBar.Indeterminate = true;
			//                        signingInPanel.Visibility = ViewStates.Visible;
			//                    }
			//                    else
			//                    {
			//                        progressBar.Indeterminate = false;
			//                        signingInPanel.Visibility = ViewStates.Invisible;

			//                        if(Settings.IsLoggedIn)
			//                        {
			//                            var newIntent = new Intent(this, typeof(MainActivity));

			//                            newIntent.AddFlags(ActivityFlags.ClearTop);
			//                            newIntent.AddFlags(ActivityFlags.SingleTop);
			//                            StartActivity(newIntent);
			//                            Finish();
			//                        }
			//                    }
			//                }
			//                break;
			//        }
			//    });
			//}

		}
	}
}

