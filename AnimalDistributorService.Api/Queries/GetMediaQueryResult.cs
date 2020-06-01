using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalDistributorService.Api.Queries
{
    public class GetMediaQueryResult
    {
        public List<GetMediaQueryResultItem> list { get; set; } = new List<GetMediaQueryResultItem>();
}

    public class GetMediaQueryResultItem
    {
        public Guid mediaId { get; set; }
        public string fileName { get; set; }
        public int contentType { get; set; }
    }
}
