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
using Android.Util;
using Android.Graphics;

namespace Xamarin.AmazingListView
{
	public class AmazingListView : ListView, IHasMorePagesListener
	{
		//public const string Tag = "AmazingListview";

		protected View _listFooter;
		protected bool _footerViewAttached = false;

		private View _headerView;
		private bool _headerViewVisible;

		protected int _headerViewWidth;
		protected int _headerViewHeight;

		protected AmazingAdapter _adapter;

		public AmazingListView(Context context) : base(context)
		{
		}

		public AmazingListView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		public AmazingListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
		}

		public AmazingListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		#region custom func
		public void SetPinnedHeaderView(View view) {
			_headerView = view;

			// Disable vertical fading when the pinned header is present
			// TODO change ListView to allow separate measures for top and bottom fading edge;
			// in this particular case we would like to disable the top, but not the bottom edge.
			if (_headerView != null) {
				SetFadingEdgeLength(0);
			}
			RequestLayout();
		}

		public void setLoadingView(View listFooter) {
			this._listFooter = listFooter;
		}

		public void ConfigureHeaderView(int position) {
			if (_headerView == null) {
				return;
			}

			int state = _adapter.GetPinnedHeaderState(position);
			switch (state) {
				case AmazingAdapter.PinnedHeaderGone: {
				_headerViewVisible = false;
				break;
			}

				case AmazingAdapter.PinnedHeaderVisible: {
				_adapter.ConfigurePinnedHeader(_headerView, position, 255);
				if (_headerView.Top != 0) {
					_headerView.Layout(0, 0, _headerViewWidth, _headerViewHeight);
				}
				_headerViewVisible = true;
				break;
			}

				case AmazingAdapter.PinnedHeaderPushedUp: {
				View firstView = GetChildAt(0);
				if (firstView != null) {
					int bottom = firstView.Bottom;
					int headerHeight = _headerView.Height;
					int y;
					int alpha;
					if (bottom < headerHeight) {
						y = (bottom - headerHeight);
						alpha = 255 * (headerHeight + y) / headerHeight;
					} else {
						y = 0;
						alpha = 255;
					}
					_adapter.ConfigurePinnedHeader(_headerView, position, alpha);
					if (_headerView.Top != y) {
						_headerView.Layout(0, y, _headerViewWidth, _headerViewHeight + y);
					}
					_headerViewVisible = true;
				}
				break;
			}
			}
		}
		#endregion


		#region override listview func
		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
			if (_headerView != null) {
				MeasureChild(_headerView, widthMeasureSpec, heightMeasureSpec);
				_headerViewWidth = _headerView.MeasuredWidth;
				_headerViewHeight = _headerView.MeasuredHeight;
			}
		}

		protected override void OnLayout (bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout (changed, left, top, right, bottom);
			if (_headerView != null) {
				_headerView.Layout(0, 0, _headerViewWidth, _headerViewHeight);
				ConfigureHeaderView(FirstVisiblePosition);
			}
		}

		protected override void DispatchDraw(Canvas canvas)
		{
			base.DispatchDraw (canvas);
			if (_headerViewVisible) {
				DrawChild(canvas, _headerView, DrawingTime);
			}
		}

		public override IListAdapter Adapter {
			get {
				return this._adapter;
			}
			set {
				if (!value.GetType ().Equals (typeof(AmazingAdapter))) {

				}
				// previous adapter
				if (this._adapter != null) {
					this._adapter.SetHasMorePagesListener(null);
					this.SetOnScrollListener(null);
				}

				this._adapter = (AmazingAdapter) value;
				((AmazingAdapter)_adapter).SetHasMorePagesListener (this);
				this.SetOnScrollListener((AmazingAdapter) _adapter);

				View dummy = new View(Context);
				base.AddFooterView (dummy);
				base.Adapter = _adapter;
				base.RemoveFooterView(dummy);
			}
		}
		#endregion

		#region implement IHaveMorePageListener
		public void NoMorePages ()
		{
			if (_listFooter != null) {
				this.RemoveFooterView(_listFooter);
			}
			_footerViewAttached = false;
		}

		public void MayHaveMorePages ()
		{
			if (! _footerViewAttached && _listFooter != null) {
				this.AddFooterView (_listFooter);
				_footerViewAttached = true;
			}
		}
		#endregion
	}
}

