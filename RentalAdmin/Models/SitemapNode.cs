using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalAdmin.Models
{
    public class SitemapNode
    {
        public SitemapFrequency? Frequency { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public string Url { get; set; }
    }
    /// <summary>
    /// sitemap video E. zaboonesh en asst vali bekhooni vazeh ast.
    /// </summary>
    public class SitemapVideoNode
    {
        //Start <video:video>	Required	Encloses all information about the video.
        /// <summary>
        /// <video:thumbnail_loc> Required
        /// A URL pointing to the video thumbnail image file. Images must be at least 160x90 pixels and at most 1920x1080 pixels. We recommend images in .jpg, .png, or. gif formats.
        /// </summary>
        public string thumbnail_loc { get; set; }
        /// <summary>
        /// <video:title> Required
        /// The title of the video. Maximum 100 characters. All HTML entities should be escaped or wrapped in a CDATA block.
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// <video:description> Required
        /// The description of the video. Maximum 2048 characters. All HTML entities should be escaped or wrapped in a CDATA block.
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// <video:content_loc>
        /// You must specify at least one of <video:player_loc> or <video:content_loc>.
        ///A URL pointing to the actual video media file. This file should be in .mpg, .mpeg, .mp4, .m4v, .mov, .wmv, .asf, .avi, .ra, .ram, .rm, .flv, or other video file format.
        ///Providing this file allows Google to generate video thumbnails and video previews, and can help Google verify your video.
        ///Best practice: Ensure that only Googlebot accesses your content by using a reverse DNS lookup.
        /// </summary>
        public string content_loc { get; set; }
        /// <summary>
        /// <video:player_loc>	Depends
        /// You must specify at least one of <video:player_loc> or <video:content_loc>.
        ///A URL pointing to a player for a specific video. Usually this is the information in the src element of an <embed> tag and should not be the same as the content of the <loc> tag.
        ///The optional attribute allow_embed specifies whether Google can embed the video in search results. Allowed values are Yes or No.
        ///The optional attribute autoplay has a user-defined string (in the example above, ap=1) that Google may append (if appropriate) to the flashvars parameter to enable autoplay of the video. For example: <embed src="http://www.example.com/videoplayer.swf?video=123" autoplay="ap=1"/>.
        ///Example player URL for Dailymotion: http://www.dailymotion.com/swf/x1o2g
        ///Best practice: Ensure that only Googlebot accesses your content by using a reverse DNS lookup.
        /// </summary>
        public string player_loc { get; set; }
        /// <summary>
        /// duration Recommended
        /// The duration of the video in seconds. Value must be between 0 and 28800 (8 hours).
        /// </summary>
        public int duration { get; set; }
        /// <summary>
        /// <video:expiration_date>	Recommended when applicable
        /// The date after which the video will no longer be available, in W3C format. Don't supply this information if your video does not expire.
        ///Allowed values are complete date (YYYY-MM-DD) and complete date plus hours, minutes and seconds, and timezone (YYYY-MM-DDThh:mm:ss+TZD). For example, 2012-07-16T19:20:30+08:00.
        /// </summary>
        public DateTime expiration_date { get; set; }
        /// <summary>
        /// <video:rating>	Optional
        /// The rating of the video. Allowed values are float numbers in the range 0.0 to 5.0.
        /// </summary>
        public double rating { get; set; }
        /// <summary>
        /// <video:view_count> Optional 
        /// The number of times the video has been viewed
        /// </summary>
        public long view_count { get; set; }
        /// <summary>
        /// <video:publication_date>	Optional
        /// The date the video was first published, in W3C format. Acceptable values are complete date (YYYY-MM-DD) and complete date plus hours, minutes and seconds, and timezone (YYYY-MM-DDThh:mm:ss+TZD). For example, 2007-07-16T19:20:30+08:00.
        /// </summary>
        public DateTime publication_date { get; set; }
        /// <summary>
        /// <video:family_friendly>	Optional
        /// No if the video should be available only to users with SafeSearch turned off.
        /// for limoodan always dont need to this tag
        /// </summary>
        public string family_friendly { get; set; }
        /// <summary>
        /// <video:tag>	Optional
        /// A tag associated with the video. Tags are generally very short descriptions of key concepts associated with a video or piece of content. A single video could have several tags, although it might belong to only one category. For example, a video about grilling food may belong in the Grilling category, but could be tagged "steak", "meat", "summer", and "outdoor". Create a new <video:tag> element for each tag associated with a video. A maximum of 32 tags is permitted.
        /// </summary>
        public string[] tag { get; set; }
        /// <summary>
        /// <video:category>	Optional
        /// The video's category. For example, cooking. The value should be a string no longer than 256 characters. In general, categories are broad groupings of content by subject. Usually a video will belong to a single category. For example, a site about cooking could have categories for Broiling, Baking, and Grilling.
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// <video:restriction>	Optional
        /// A space-delimited list of countries where the video may or may not be played. Allowed values are country codes in ISO 3166 format. Only one <video:restriction> tag can appear for each video. If there is no <video:restriction> tag, it is assumed that the video can be played in all territories.
        ///The required attribute relationship specifies whether the video is restricted or permitted for the specified countries. Allowed values are allow or deny.
        ///  A list of space-delimited => "iran arab usa"
        /// </summary>
        public string restriction { get; set; }
        /// <summary>
        /// <video:gallery_loc>	Optional
        /// A link to the gallery (collection of videos) in which this video appears. Only one <video:gallery_loc> tag can be listed for each video. The optional attribute title indicates the title of the gallery.
        /// </summary>
        public string gallery_loc { get; set; }
        /// <summary>
        /// <video:price>	Optional
        /// Iranian rial    in  ISO 4217 format.
        /// The price to download or view the video. Do not use this tag for free videos.The required attribute currency specifies the currency in ISO 4217 format.
        /// The optional attribute type specifies the purchase option. Allowed values are rent and own. If this is not specified, the default value is own.
        /// The optional attribute resolution specifies the purchased resolution. Allows values are HD and SD.
        /// More than one <video:price> element can be listed (for example, in order to specify various currencies, purchasing options, or resolutions).
        /// </summary>
        public long price { get; set; }
        /// <summary>
        /// <video:requires_subscription>	Optional
        /// Indicates whether a subscription (either paid or free) is required to view the video. Allowed values are yes or no.
        /// </summary>
        public string requires_subscription { get; set; }
        /// <summary>
        /// <video:uploader>	Optional
        /// The video uploader's name. Only one <video:uploader> is allowed per video.
        /// </summary>
        public string uploader { get; set; }
        /// <summary>
        /// <video:platform>	Optional
        /// A list of space-delimited platforms where the video may or may not be played. Allowed values are web, mobile, and tv. Only one <video:platform> tag can appear for each video. If there is no <video:platform> tag, it is assumed that the video can be played on all platforms.
        /// A list of space-delimited => "mobile tv" or "tv web"
        /// The required attribute relationship specifies whether the video is restricted or permitted for the specified platforms. Allowed values are allow or deny.
        /// </summary>
        public string platform { get; set; }
        /// <summary>
        /// <video:live>	Optional
        /// Indicates whether the video is a live stream. Allowed values are yes or no.
        /// </summary>
        public string live { get; set; }

//        <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"
//        xmlns:video="http://www.google.com/schemas/sitemap-video/1.1"> 
//   <url> 
//     <loc>http://www.example.com/videos/some_video_landing_page.html</loc>
//     <video:video>
//       <video:thumbnail_loc>http://www.example.com/thumbs/123.jpg</video:thumbnail_loc> 
//       <video:title>Grilling steaks for summer</video:title>
//       <video:description>Alkis shows you how to get perfectly done steaks every            
//         time</video:description>
//       <video:content_loc>http://www.example.com/video123.flv</video:content_loc>
//       <video:player_loc allow_embed="yes" autoplay="ap=1">
//         http://www.example.com/videoplayer.swf?video=123</video:player_loc>
//       <video:duration>600</video:duration>
//       <video:expiration_date>2009-11-05T19:20:30+08:00</video:expiration_date>
//       <video:rating>4.2</video:rating> 
//       <video:view_count>12345</video:view_count>    
//       <video:publication_date>2007-11-05T19:20:30+08:00</video:publication_date>
//       <video:family_friendly>yes</video:family_friendly>   
//       <video:restriction relationship="allow">IE GB US CA</video:restriction> 
//       <video:gallery_loc title="Cooking Videos">http://cooking.example.com</video:gallery_loc>
//       <video:price currency="EUR">1.99</video:price>
//       <video:requires_subscription>yes</video:requires_subscription>
//       <video:uploader info="http://www.example.com/users/grillymcgrillerson">GrillyMcGrillerson
//         </video:uploader>
//       <video:live>no</video:live>
//     </video:video> 
//   </url> 
//</urlset>
    }
    public class SitemapWithVideosNode
    {
        /// <summary>
        /// <loc> Required
        /// This tag specifies the landing page (aka play page, referrer page) for the video. When a user clicks on a video result on a search results page, they will be sent to this landing page. Must be a unique URL.
        ///If your landing page features multiple videos, don't create a separate <loc> tag for each video. Instead, create a single <loc> tag that includes a <video:video> element for each video on the landing page.
        /// </summary>
        public string loc { get; set; }
        public List<SitemapVideoNode> videosNode { get; set; }
    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }
    public class RSS
    {
        public string title { get; set; }
        public string description { get; set; }
        public DateTime publishDate { get; set; }
        public string Url { get; set; }
    }
}