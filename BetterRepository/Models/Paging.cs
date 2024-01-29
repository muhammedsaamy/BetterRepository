using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace BetterRepository.Models
{
    public class PagingDescriptor
    {
        public int ActualPageSize { get; private set; }
        public int NumberOfPages { get; private set; }
        public PageBoundry[] PagesBoundries { get; private set; }

        public PagingDescriptor(
        int actualPageSize,
        int numberOfPages,
        PageBoundry[] pagesBoundries)
        {
            ActualPageSize = actualPageSize;
            NumberOfPages = numberOfPages;
            PagesBoundries = pagesBoundries;
        }

        // This code is added for demonstration purposes only.
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"ActualPageSize: {ActualPageSize}");
            builder.AppendLine($"NumberOfPages: {NumberOfPages}");
            builder.AppendLine("");
            builder.AppendLine($"PagesBoundries:");
            builder.AppendLine($"----------------");

            foreach (var boundry in PagesBoundries)
            {
                builder.Append(boundry.ToString());
            }

            return builder.ToString();
        }
    }

    public class PageBoundry
    {
        public int FirstItemZeroIndex { get; private set; }
        public int LastItemZeroIndex { get; private set; }

        public PageBoundry(int firstItemZeroIndex, int lastItemZeroIndex)
        {
            FirstItemZeroIndex = firstItemZeroIndex;
            LastItemZeroIndex = lastItemZeroIndex;
        }


        // This code is added for demonstration purposes only.
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"FirstItemZeroIndex: {FirstItemZeroIndex}, LastItemZeroIndex: {LastItemZeroIndex}");
            return builder.ToString();
        }
    }

    public static class ListExtensionMethods
    {
        public static PagingDescriptor Page(this IList list, int pageSize)
        {
            var actualPageSize = pageSize;

            if (actualPageSize <= 0)
            {
                actualPageSize = list.Count;
            }

            var maxNumberOfPages = (int)Math.Round(Math.Max(1, Math.Ceiling(((float)list.Count) / ((float)actualPageSize))));

            return new PagingDescriptor(
                actualPageSize,
                maxNumberOfPages,
                Enumerable
                    .Range(0, maxNumberOfPages)
                    .Select(pageZeroIndex => new PageBoundry(
                        pageZeroIndex * actualPageSize,
                        Math.Min((pageZeroIndex * actualPageSize) + (actualPageSize - 1), list.Count - 1)
                    )).ToArray()
            );
        }
        //public static PagingDescriptor Page(this IList list, int pageSize)
        //{
        //    if (list == null)
        //    {
        //        throw new ArgumentNullException(nameof(list));
        //    }

        //    if (pageSize <= 0)
        //    {
        //        pageSize = list.Count;
        //    }

        //    int totalItems = list.Count;

        //    if (totalItems <= 0)
        //    {
        //        return new PagingDescriptor(pageSize, 0, Array.Empty<PageBoundry>());
        //    }

        //    int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //    var pageBoundaries = Enumerable.Range(0, totalPages)
        //        .Select(pageIndex =>
        //        {
        //            int startIndex = pageIndex * pageSize;
        //            int endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

        //            if (endIndex < 0)  // Ensure endIndex is not negative
        //            {
        //                endIndex = 0;
        //            }

        //            return new PageBoundry(startIndex, endIndex);
        //        })
        //        .ToArray();

        //    return new PagingDescriptor(pageSize, totalPages, pageBoundaries);
        //}

    }
}
