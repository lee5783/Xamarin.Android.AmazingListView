using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Xamarin.AmazingListView.Demo
{
	[Activity (Label = "SectionActivity")]			
	public class SectionActivity : Activity
	{
		AmazingListView _lv;
		AmazingAdapter _adapter;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_section_demo);
			_lv = FindViewById<AmazingListView> (Resource.Id.lsComposer);
			_lv.SetPinnedHeaderView (LayoutInflater.Inflate (Resource.Layout.item_composer_header, _lv, false));
			_lv.FastScrollEnabled = true;
			_lv.Adapter = _adapter = new SectionComposerAdapter (this);
		}
	}

	public class SectionComposerAdapter : AmazingAdapter
	{
		protected static List <Composer> _data = Data.GetAllData();

		protected Dictionary<string, int> _sectionDict;
		protected Java.Lang.Object[] _sections;

		protected Context _context;
		public SectionComposerAdapter(Context context)
		{
			_context = context;
			CreateSectionDictionary ();
		}

		void CreateSectionDictionary ()
		{
			_sectionDict = new Dictionary<string, int> ();
			for (int i = 0 ; i < Count; i++) {
				var item = _data [i];
				string key = item.Name [0].ToString ().ToUpper ();
				if(_sectionDict.ContainsKey(key) == false){
					_sectionDict.Add (key, i);
				}
			}

			_sections = new Java.Lang.Object[_sectionDict.Count];
			var listKey = _sectionDict.Keys.ToList();
			for (int i = 0; i < listKey.Count; i++) {
				_sections [i] = listKey [i];
			}
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public Composer GetComposer (int position)
		{
			if (position >= 0 && position < Count) {
				return _data [position];
			}
			return null;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count {
			get {
				return _data.Count;
			}
		}

		protected override void OnNextPageRequested (int page)
		{
			//nop
		}

		protected override void BindSectionHeader (View view, int position, bool displaySectionHeader)
		{
			//TextView lSectionTitle = view.FindViewById<TextView> (Resource.Id.header);
			if (displaySectionHeader) {
				TextView lSectionTitle = view.FindViewById<TextView> (Resource.Id.header);
				lSectionTitle.Visibility = ViewStates.Visible;
				lSectionTitle.Text = GetSections () [GetSectionForPosition (position)].ToString ();
			} else {
				TextView lSectionTitle = view.FindViewById<TextView> (Resource.Id.header);
				lSectionTitle.Visibility = ViewStates.Gone;
			}
		}

		public override void ConfigurePinnedHeader (View header, int position, int alpha)
		{
			TextView lSectionHeader = (TextView)header;
			lSectionHeader.Text = GetSections () [GetSectionForPosition (position)].ToString ();
			lSectionHeader.SetBackgroundColor (new Color (alpha << 24 | (0xbbffbb)));
			lSectionHeader.SetTextColor (new Color (alpha << 24 | (0x000000)));
		}

		public override View GetAmazingView (int position, View convertView, ViewGroup parent)
		{
			View v = convertView;
			if (v == null) {
				v = LayoutInflater.From (_context).Inflate (Resource.Layout.item_composer, null);
			}
			TextView tvName = v.FindViewById<TextView> (Resource.Id.lName);
			TextView tvYear = v.FindViewById<TextView> (Resource.Id.lYear);
			var item = GetComposer (position);
			tvName.Text = item.Name;
			tvYear.Text = item.Year;
			return v;
		}

		public override int GetPositionForSection (int section)
		{
			if (section < 0)
				section = 0;
			if (section >= _sections.Length)
				section = _sections.Length - 1;
			var key = _sections [section].ToString ();
			int position = 0;
			try {
				position = _sectionDict[key];
			} catch (Exception ex) {
				Console.WriteLine("Fail to get value for key " + key);
				Console.WriteLine("Error" + ex.StackTrace);
				return -1;
			}

			return position;
		}

		public override int GetSectionForPosition (int position)
		{
			if (position < 0)
				position = 0;
			if (position >= Count)
				position = Count - 1;
			for (int i = 1; i < _sections.Length; i++) {
				if (GetPositionForSection (i - 1) <= position 
					&& GetPositionForSection (i) > position) {
					return i - 1;
				}
			}
			return _sections.Length - 1;

		}

		public override Java.Lang.Object[] GetSections ()
		{
			return _sections;
		}
	}
}

