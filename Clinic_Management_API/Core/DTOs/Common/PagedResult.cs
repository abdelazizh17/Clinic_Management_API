namespace Clinic_Management_API.Core.DTOs.Common
{
    /// <summary>
    /// A generic wrapper for paginated API responses.
    /// It contains the current page items plus the metadata the client needs
    /// to build page numbers (TotalCount / TotalPages).
    /// </summary>
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

        public int PageNumber { get; init; }

        public int PageSize { get; init; }

        /// <summary>Total number of records that match the filter (before paging).</summary>
        public int TotalCount { get; init; }

        /// <summary>Number of pages = ceil(TotalCount / PageSize).</summary>
        public int TotalPages => PageSize <= 0
            ? 0
            : (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}
