using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    class ShowsModel
    {
        public int responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public ShowsData data { get; set; }
    }

    public class ShowsData
    {
        public Playlists playlists { get; set; }
    }

    public class Playlists
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }         
        public Pageinfo pageInfo { get; set; }
        public VideoItem[] items { get; set; }
    }

    public class Pageinfo
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }

    public class VideoItem
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet snippet { get; set; }
    }

    public class Snippet
    {
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails { get; set; }
        public string channelTitle { get; set; }
        public Localized localized { get; set; }
        public string defaultLanguage { get; set; }
        public Resourceid resourceId { get; set; }
    }

    public class Thumbnails
    {
        public Thumbnail @default { get; set; }
        public Thumbnail medium { get; set; }
        public Thumbnail high { get; set; }
        public Thumbnail standard { get; set; }
    }

    public class Thumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    //public class Medium
    //{
    //    public string url { get; set; }
    //    public int width { get; set; }
    //    public int height { get; set; }
    //}

    //public class High
    //{
    //    public string url { get; set; }
    //    public int width { get; set; }
    //    public int height { get; set; }
    //}

    //public class Standard
    //{
    //    public string url { get; set; }
    //    public int width { get; set; }
    //    public int height { get; set; }
    //}

    //public class Maxres
    //{
    //    public string url { get; set; }
    //    public int width { get; set; }
    //    public int height { get; set; }
    //}

    public class Localized
    {
        public string title { get; set; }
        public string description { get; set; }
    }

    public class Resourceid
    {
        public string kind { get; set; }
        public string videoId { get; set; }
    }
}
