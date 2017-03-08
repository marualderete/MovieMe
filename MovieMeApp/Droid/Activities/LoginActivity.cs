using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Graphics;
using MovieMeApp.ViewModels;

namespace MovieMeApp.Droid.Activities
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

		Button signUpButton, loginButton;
		LinearLayout signingInPanel;
		ProgressBar progressBar;
		LoginViewModel viewModel;
		EditText userName, password;


		protected override void OnCreate(Bundle savedInstanceState)

		{
			//Layout gets inflated here
			base.OnCreate(savedInstanceState);

			viewModel = new LoginViewModel();

			signUpButton = FindViewById<Button>(Resource.Id.button_signup);
			loginButton = FindViewById<Button>(Resource.Id.button_login);

			userName = FindViewById<EditText>(Resource.Id.txtUserName);
			password = FindViewById<EditText>(Resource.Id.txtPassword);

			signUpButton.Text = "Sign Up";

			//Turn off back arrows
			SupportActionBar.SetDisplayHomeAsUpEnabled(false);
			SupportActionBar.SetHomeButtonEnabled(false);

		}

		protected override void OnStart()
		{
			base.OnStart();
			signUpButton.Click += SignUpButton_Click;
			loginButton.Click += LoginButton_Click;
		}

		protected override void OnStop()
		{
			base.OnStop();
			signUpButton.Click -= SignUpButton_Click;
			loginButton.Click -= LoginButton_Click;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		private async void LoginButton_Click(object sender, System.EventArgs e)
		{
			var isSessionStarted = await viewModel.TryLoginAsync(userName.Text, password.Text);

			if (!isSessionStarted)
			{
				
			}

			var intent = new Intent(this, typeof(MovieExplorerActivity));
			intent.AddFlags(ActivityFlags.ClearTop);
			StartActivity(intent);
			Finish();
		}

		private async void SignUpButton_Click(object sender, System.EventArgs e)
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

