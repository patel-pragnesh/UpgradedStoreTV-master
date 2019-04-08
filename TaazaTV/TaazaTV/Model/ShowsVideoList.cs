using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    public class ShowsVideoList
    {
        public int responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public ShowsVideoListData data { get; set; }
    }

    public class ShowsVideoListData
    {
        public Playlists1 playlists { get; set; }
    }

    public class Playlists1
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public Pageinfo pageInfo { get; set; }
        public Item1[] items { get; set; }
    }

    public class Item1
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet1 snippet { get; set; }
    }

    public class Snippet1
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public string playlistId { get; set; }
        public int position { get; set; }
        public Resourceid resourceId { get; set; }
    }

    //public class Resourceid
    //{
    //    public string kind { get; set; }
    //    public string videoId { get; set; }
    //}
}
