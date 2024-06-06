using MudBlazor;
using System;
using TomAndJerry.Model;

namespace TomAndJerry.DataBase
{
    public class Data
    {
        public event Action OnChange; 
        private void NotifyDataChanged() => OnChange?.Invoke();

        private List<Video> _data = [];
        private bool _isDataFetched;
        public List<Video> VideosData
        {
            get => _data;
            private set
            {
                _data = value;
                NotifyDataChanged();
            }
        }

        public bool isDataFetced = false;

        #region permanentUri
        static Random random = new Random();
        static string EpisodeNameUri = "https://raw.githubusercontent.com/TomJerry1940/Database/main/EpisodeNameList.txt";
        static string DriveVideoUri = "https://raw.githubusercontent.com/TomJerry1940/Database/main/GoogleDriveVideoUri.txt";
        static string ThumbnailPath = "https://raw.githubusercontent.com/TomJerry1940/Database/main/ThumbnailPath.txt";
        #endregion
        static List<T> SuffeledArray<T>(List<T> array)
        {

            for (int i = 0; i < array.Count; i++)
            {
                var randIndex = random.Next(i, array.Count); 
                (array[randIndex], array[i] ) = (array[i], array[randIndex]); 
            }
            return array;
        }
        public Video GetVideo(string id)
        {
            return VideosData.FirstOrDefault(x => x.CommentName == id || x.Description==id) ?? VideosData[0];
        }
        public  List<Video> GetRandomVideo()
        {
            return SuffeledArray(VideosData);
        }

        public async Task InitializeAsync()
        {
            if (_isDataFetched) return;
            VideosData = await FetchDataAsync();
            _isDataFetched = true;
        }

        private async Task<List<Video>> FetchDataAsync()
        { 
            using var client = new HttpClient();
            var videoListString = await client.GetStringAsync(DriveVideoUri);
            var EpisodeNameString = await client.GetStringAsync(EpisodeNameUri);
            var ThumbnailString = await client.GetStringAsync(ThumbnailPath);

            var videoUriList = videoListString
                .Split('\n').Where(x => x.Length > 0).Select(x => x.Trim()).ToList();
            var EpisodeList = EpisodeNameString
                .Split('\n').Where(x => x.Length > 0).Select(x => x.Trim()).ToList();
            var ThumbnailList = ThumbnailString
                .Split('\n').Where(x => x.Length > 0).Select(x => x.Trim()).ToList();
            List<Video> Videos = [];
            for (var i = 0; i < Math.Min(videoUriList.Count, EpisodeList.Count); i++)
            {
                Videos.Add(
                    new Video()
                    {
                        Id = i,
                        Thumbnail = ThumbnailList[i],
                        Description = EpisodeList[i],
                        VideoId = videoUriList[i],
                        CommentName = EpisodeList[i],
                        VideoUrl = videoUriList[i]
                    }
               );
            }
            return Videos;
        } 
    }
}
