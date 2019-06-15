using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrDone.Infrastructure
{
    public class PagedData<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _currentItems;
        private object currentPostPage;
        private int totalPostCount;
        private object postsPerPage;

        public int TotalCount { get; private set; }
        public int Page { get; private set; }
        public int PerPage { get; private set; }
        public int TotalPage { get; private set; }

        public bool HasNextPage { get; private set; }
        public bool HasPreviousPage{ get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            return _currentItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _currentItems.GetEnumerator();

        }

        public int NextPage
        {
            get
            {
                if (!HasNextPage)
                    throw new InvalidOperationException();
                return Page + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (!HasPreviousPage)
                    throw new InvalidOperationException();
                return Page - 1;
            }
        }

        public PagedData(IEnumerable<T> currentItems, int totalCount, int page, int perPage)
        {
            _currentItems = currentItems;

            TotalCount = totalCount;
            Page = page;
            PerPage = perPage;

            TotalPage = (int)Math.Ceiling((float)TotalCount / PerPage);
            HasNextPage = Page < TotalPage;
            HasPreviousPage = Page > 1;
        }

        public PagedData(object currentPostPage, int totalPostCount, int page, object postsPerPage)
        {
            this.currentPostPage = currentPostPage;
            this.totalPostCount = totalPostCount;
            Page = page;
            this.postsPerPage = postsPerPage;
        }
    }
}