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

namespace Xamarin.AmazingListView.Demo
{
	public class Data
	{
		public static Dictionary<string, List<Composer>> GetAllComporser()
		{
			Dictionary <string, List<Composer>> res = new Dictionary<string, List<Composer>> ();
			return res;
		}

		public static List<Composer> GetAllData(){
			List<Composer> res = new List<Composer> ();
			for (int i = 100; i > 0; i--) {
				if (i > 90) {
					res.Add (new Composer () {Name = "A" + " Name " + i, Year = "Year " + i });
				} else if (i > 80) {
					res.Add (new Composer () {Name = "B" + " Name " + i, Year = "Year " + i });
				} else if (i > 70) {
					res.Add (new Composer () {Name = "C" + " Name " + i, Year = "Year " + i });
				} else if (i > 60) {
					res.Add (new Composer () {Name = "D" + " Name " + i, Year = "Year " + i });
				} else if (i > 50) {
					res.Add (new Composer () {Name = "E" + " Name " + i, Year = "Year " + i });
				} else if (i > 40) {
					res.Add (new Composer () {Name = "F" + " Name " + i, Year = "Year " + i });
				} else if (i > 30) {
					res.Add (new Composer () {Name = "G" + " Name " + i, Year = "Year " + i });
				} else if (i > 20) {
					res.Add (new Composer () {Name = "H" + " Name " + i, Year = "Year " + i });
				} else if (i > 10) {
					res.Add (new Composer () {Name = "I" + " Name " + i, Year = "Year " + i });
				} else {
					res.Add (new Composer () {Name = "K" + " Name " + i, Year = "Year " + i });
				}
			}
			return res;
		}
	}
}

