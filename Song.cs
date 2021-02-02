using System;
using TagLib;

namespace MusicPlayer
{
    class Song
    {
        private string path;
        private string title;
        private string album;
        private string[] artists;

        public string GetPath => path;
        public string GetTitle => title;
        public string GetAlbum => album;
        public string[] GetArtists => artists;

        public Song(string path)
        {
            var tag = TagLib.File.Create(path);
            this.path = path;
            this.title = tag.Tag.Title;
            this.album = tag.Tag.Album;
            this.artists = tag.Tag.AlbumArtists;
        }
    }
}
