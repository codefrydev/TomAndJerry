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

        public bool IsDataFetced = false;

        private List<Video> _filteredData = [];

        public List<Video> FilteredData
        {
            get => _filteredData;
            set
            {
                _filteredData = value;
                NotifyDataChanged();
            }
        }


        #region permanentUri

        private static Random _random = new Random();
        private const string EpisodeNameUri = "https://raw.githubusercontent.com/TomJerry1940/Database/main/EpisodeNameList.txt";
        private const string DriveVideoUri = "https://raw.githubusercontent.com/TomJerry1940/Database/main/GoogleDriveVideoUri.txt";
        private const string ThumbnailPath = "https://raw.githubusercontent.com/TomJerry1940/Database/main/ThumbnailPath.txt";

        #endregion
        static List<T> SuffeledArray<T>(List<T> array)
        {

            for (var i = 0; i < array.Count; i++)
            {
                var randIndex = _random.Next(i, array.Count);
                (array[randIndex], array[i]) = (array[i], array[randIndex]);
            }
            return array;
        }
        public Video GetVideo(string id)
        {
            return VideosData.FirstOrDefault(x => x.Id == id) ?? VideosData[0];
        }
        public List<Video> GetRandomVideo()
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
            if (IsDataFetced) return VideosData;
            using var client = new HttpClient();
            var videoListString = await client.GetStringAsync(DriveVideoUri);
            var episodeNameString = await client.GetStringAsync(EpisodeNameUri);
            var thumbnailString = await client.GetStringAsync(ThumbnailPath);

            var videoUriList = videoListString
                .Split('\n').Where(x => x.Length > 0).Select(x => x.Trim()).ToList();
            var episodeList = episodeNameString
                .Split('\n').Where(x => x.Length > 0).Select(x => x.Trim()).ToList();
            var thumbnailList = thumbnailString
                .Split('\n').Where(x => x.Length > 0).Select(x => x.Trim()).ToList();
            VideosData = [];
            for (var i = 0; i < Math.Min(videoUriList.Count, episodeList.Count); i++)
            {
                VideosData.Add(
                    new Video()
                    {
                        Id = $"{i+1}",
                        Thumbnail = thumbnailList[i],
                        Description = episodeList[i],
                        VideoId = videoUriList[i],
                        CommentName = episodeList[i],
                        VideoUrl = videoUriList[i]
                    }
               );
            }
            IsDataFetced = true;
            return VideosData;
        }
    }
}
