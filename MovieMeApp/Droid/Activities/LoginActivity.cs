using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Content.PM;
using MovieMeApp.ViewModels;
using Android.Views;

namespace MovieMeApp.Droid.Activities
{
    [Activity (Label = "@string/login",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : BaseActivity
    {
        #region protected properties
        /// <summary>
        /// Specify the layout to inflace
        /// </summary>
        protected override int LayoutResource => Resource.Layout.activity_login;

        #endregion

        #region private properties
        Button signUpButton, loginButton;
        ProgressBar progressBar;
        LoginViewModel viewModel;
        EditText userName, password;

        #endregion

        #region override methods
        protected override void OnCreate (Bundle savedInstanceState)

        {
            //Layout gets inflated here
            base.OnCreate (savedInstanceState);

            viewModel = new LoginViewModel ();

            progressBar = FindViewById<ProgressBar> (Resource.Id.progressbar_login_loading);
            signUpButton = FindViewById<Button> (Resource.Id.button_signup);
            loginButton = FindViewById<Button> (Resource.Id.button_login);

            userName = FindViewById<EditText> (Resource.Id.txtUserName);
            password = FindViewById<EditText> (Resource.Id.txtPassword);

            signUpButton.Text = "Sign Up";
            progressBar.Visibility = ViewStates.Invisible;
            //Turn off back arrows
            SupportActionBar.SetDisplayHomeAsUpEnabled (false);
            SupportActionBar.SetHomeButtonEnabled (false);

        }

        protected override void OnStart ()
        {
            base.OnStart ();

            signUpButton.Click += SignUpButton_Click;
            loginButton.Click += LoginButton_Click;
        }

        protected override void OnStop ()
        {
            base.OnStop ();
            signUpButton.Click -= SignUpButton_Click;
            loginButton.Click -= LoginButton_Click;
        }

        protected override void OnDestroy ()
        {
            base.OnDestroy ();
        }

        #endregion


        #region private methods
        private async void LoginButton_Click (object sender, System.EventArgs e)
        {
            progressBar.Visibility = ViewStates.Visible;

            var isSessionStarted = await viewModel.TryLoginAsync (userName.Text, password.Text);

            if (!isSessionStarted) {

            }

            var intent = new Intent (this, typeof (MovieExplorerActivity));
            intent.AddFlags (ActivityFlags.ClearTop);
            progressBar.Visibility = ViewStates.Gone;
            StartActivity (intent);
            Finish ();


        }

        private async void SignUpButton_Click (object sender, System.EventArgs e)
        {
            // await viewModel.SignIn();
        }

        private void ViewModel_PropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

        #endregion
    }
}

