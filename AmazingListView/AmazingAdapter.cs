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

namespace Xamarin.AmazingListView
{			
	public abstract class AmazingAdapter : BaseAdapter, ISectionIndexer, AbsListView.IOnScrollListener
	{
		//public const string Tag = "AmazingAdapter";
		/**
	 	* The <em>current</em> page, not the page that is going to be loaded.
	 	*/
		int _page = 1;
		int _initialPage = 1;
		bool _automaticNextPageLoading = false;
		IHasMorePagesListener _hasMorePagesListener;

		public void SetHasMorePagesListener(IHasMorePagesListener hasMorePagesListener) {
			this._hasMorePagesListener = hasMorePagesListener;
		}

		/**
     	* Pinned header state: don't show the header.
     	*/
		public const int PinnedHeaderGone = 0;

		/**
	     * Pinned header state: show the header at the top of the list.
	     */
		public const int PinnedHeaderVisible = 1;

		/**
	     * Pinned header state: show the header. If the header extends beyond
	     * the bottom of the first shown element, push it up and clip.
	     */
		public const int PinnedHeaderPushedUp = 2;

		/**
	     * Computes the desired state of the pinned header for the given
	     * position of the first visible list item. Allowed return values are
	     * {@link #PINNED_HEADER_GONE}, {@link #PINNED_HEADER_VISIBLE} or
	     * {@link #PINNED_HEADER_PUSHED_UP}.
	     */
		public int GetPinnedHeaderState(int position) {
			if (position < 0 || Count == 0) {
				return PinnedHeaderGone;
			}

			// The header should get pushed up if the top item shown
			// is the last item in a section for a particular letter.
			int section = GetSectionForPosition (position);
			int nextSectionPosition = GetPositionForSection(section + 1);
			if (nextSectionPosition != -1 && position == nextSectionPosition - 1) {
				return PinnedHeaderPushedUp;
			}

			return PinnedHeaderVisible;
		}

		/**
	     * Sets the initial page when {@link #resetPage()} is called.
	     * Default is 1 (for APIs with 1-based page number).
	     */
		public void SetInitialPage(int initialPage) {
			this._initialPage = initialPage;
		}

		/**
	     * Resets the current page to the page specified in {@link #setInitialPage(int)}.
	     */
		public void ResetPage() {
			this._page = this._initialPage;
		}

		/**
	     * Increases the current page number.
	     */
		public void NextPage() {
			this._page++;
		}

		#region override adapter func 
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View res = GetAmazingView(position, convertView, parent);

			if (position == Count - 1 && _automaticNextPageLoading) {
				OnNextPageRequested(_page + 1);
			}

			int section = GetSectionForPosition(position);
			bool displaySectionHeaders = (GetPositionForSection(section) == position);

			BindSectionHeader(res, position, displaySectionHeaders);

			return res;
		}
		#endregion



		#region HandleScroll
		public void OnScroll (AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
		{
			if (view.GetType().Equals(typeof(AmazingListview))) {
				((AmazingListview) view).ConfigureHeaderView(firstVisibleItem);
			}
		}

		public void OnScrollStateChanged (AbsListView view, ScrollState scrollState)
		{
			//nop
		}
		#endregion

		public void notifyNoMorePages() {
			_automaticNextPageLoading = false;
			if (_hasMorePagesListener != null) _hasMorePagesListener.NoMorePages();
		}

		public void notifyMayHaveMorePages() {
			_automaticNextPageLoading = true;
			if (_hasMorePagesListener != null) _hasMorePagesListener.MayHaveMorePages();
		}

		/**
		 * The last item on the list is requested to be seen, so do the request 
		 * and call {@link AmazingListView#tellNoMoreData()} if there is no more pages.
		 * 
		 * @param page the page number to load.
		 */
		protected abstract void OnNextPageRequested(int page);

		/**
		 * Configure the view (a listview item) to display headers or not based on displaySectionHeader 
		 * (e.g. if displaySectionHeader header.setVisibility(VISIBLE) else header.setVisibility(GONE)).
		 */
		protected abstract void BindSectionHeader(View view, int position, bool displaySectionHeader);

		/**
	 	* read: get view too
	 	*/
		public abstract View GetAmazingView(int position, View convertView, ViewGroup parent);

		/**
	     * Configures the pinned header view to match the first visible list item.
	     *
	     * @param header pinned header view.
	     * @param position position of the first visible list item.
	     * @param alpha fading of the header view, between 0 and 255.
	     */
		public abstract void ConfigurePinnedHeader(View header, int position, int alpha);

		#region HandleSectionIndex
		public abstract int GetPositionForSection(int section);
		public abstract int GetSectionForPosition(int position);
		public abstract Java.Lang.Object[] GetSections();


		#endregion
	}
}

