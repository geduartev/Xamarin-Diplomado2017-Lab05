using Android.App;
using Android.Widget;
using Android.OS;

namespace PhoneApp
{
    [Activity(Label = "Phone App", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Validate();

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberEditText);
            var TranslateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var CallButton = FindViewById<Button>(Resource.Id.CallButton);

            CallButton.Enabled = false;

            var TranslatedNumber = string.Empty;

            TranslateButton.Click += (object sender, System.EventArgs e) =>
            {
                var Translator = new PhoneTranslator();
                TranslatedNumber = Translator.ToNumber(PhoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(TranslatedNumber))
                {
                    CallButton.Text = "Llamar";
                    CallButton.Enabled = false;
                }

                if (!string.IsNullOrWhiteSpace(TranslatedNumber))
                {
                    CallButton.Text = $"Llamar al {TranslatedNumber}";
                    CallButton.Enabled = true;
                }
            };

            CallButton.Click += (object sender, System.EventArgs e) =>
            {
                var CallDialog = new AlertDialog.Builder(this);
                CallDialog.SetMessage($"Llamar al número {TranslatedNumber}?");
                CallDialog.SetNeutralButton("Llamar", delegate
                {
                    var CallIntent = new Android.Content.Intent(Android.Content.Intent.ActionCall);
                    CallIntent.SetData(Android.Net.Uri.Parse($"tel:{TranslatedNumber}"));
                    StartActivity(CallIntent);
                });
                CallDialog.SetNegativeButton("Cancelar", delegate { });
                CallDialog.Show();
            };            
        }

        private async void Validate()
        {
            string StudentEmail = "email";
            string Password = "password";
            string myDevice = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            var ServiceClient = new SALLab05.ServiceClient();
            var Result = await ServiceClient.ValidateAsync(StudentEmail, Password, myDevice);

            var ResultText = FindViewById<TextView>(Resource.Id.ResultTextView);
            ResultText.SetPadding(40, 20, 0, 0);
            ResultText.Text = $"{Result.Status}\n{Result.Fullname}\n{Result.Token}";
        }
    }
}

