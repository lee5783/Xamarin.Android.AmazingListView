using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Xamarin.AmazingListView.Demo
{
	[Activity (Label = "AndroidAmazingListviewDemo", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_main);
			FindViewById<Button> (Resource.Id.sectionBtn).Click += SectionButtonClickAction;
			FindViewById<Button> (Resource.Id.paginationBtn).Click += PaginationButtonClickAction;
		}

		void SectionButtonClickAction (object sender, EventArgs e)
		{
			var intent = new Intent (this, typeof(SectionActivity));
			StartActivity (intent);
		}

		void PaginationButtonClickAction (object sender, EventArgs e)
		{
			var intent = new Intent (this, typeof(PaginationActivity));
			StartActivity (intent);
		}
	}
}


