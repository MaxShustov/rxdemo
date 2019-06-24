using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using rxdemo.core;
using ReactiveUI;
using Android.Widget;

namespace rxdemo.droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IViewFor<MyViewModel>
    {
        public MyViewModel ViewModel { get; set; }

        object IViewFor.ViewModel 
        {
            get => ViewModel;
            set => ViewModel = (MyViewModel)value; 
        }

        public TextView FullName { get; private set; }
        public TextView FirstName { get; private set; }
        public Button SearchButton { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            this.WireUpControls();

            ViewModel = new MyViewModel();

            this.Bind(ViewModel,
                viewModel => viewModel.FirstName,
                view => view.FirstName);

            this.BindCommand(ViewModel,
                viewModel => viewModel.Search,
                view => view.SearchButton);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}