using System;
/// WF.Player.Android - A Wherigo Player User Interface for Android platform.
/// Copyright (C) 2012-2013  Dirk Weltz <web@weltz-online.de>
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
/// 
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
/// 
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using WF.Player.Core;

namespace WF.Player.Android
{
//	public delegate void LocationChangedEventHandler(Object IntentSender, LocationChangedEventArgs e);

	[Application(Debuggable=true)]
	public class MainApp : Application
	{
		private Activity activeActivity;
		private Cartridges cartridges;
		private LocListener locListener;
		private string path;

		public event EventHandler<LocationChangedEventArgs> LocationChanged;
		
		public MainApp(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + Java.IO.File.Separator + "WF.Player";

			Directory.CreateDirectory (path);

			if (!Directory.Exists (path))
			{
				AlertDialog.Builder builder = new AlertDialog.Builder (this);
				builder.SetTitle (GetString (Resource.String.main_error));
				builder.SetMessage(String.Format(GetString(Resource.String.main_error_directory_not_found),path));
				builder.SetCancelable (true);
				builder.SetNeutralButton(Resource.String.ok,(obj,arg) => { });
				builder.Show ();
			}

			// Create LocListener
			locListener = new LocListener (GetSystemService (Context.LocationService) as LocationManager);
			locListener.LocationChanged += OnLocationChanged;
		}

		public Activity ActiveActivity {
			get {
				return activeActivity;
			}
			set {
				if (activeActivity != value) {
					activeActivity = value;
				}
			}
		}

		public Cartridges Cartridges {
			get {
				return cartridges;
			}
			set {
				cartridges = value;
			}
		}

		public string Path {
			get {
				return path;
			}
		}

		private void OnLocationChanged(Object sender, LocationChangedEventArgs e)
		{
			if (LocationChanged != null)
				LocationChanged (this, e);

		}
	}
}
