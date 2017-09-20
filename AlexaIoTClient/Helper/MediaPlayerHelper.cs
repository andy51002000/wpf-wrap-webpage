using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaIoTClient.Helper
{
    public class MediaPlayerHelper
    {

        public void RemovePlayList()
        {
            //precheck
            var pathPlaylist =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Playlists\\playlist.wpl");
            if (File.Exists(pathPlaylist))
            {
                try
                {
                    File.Delete(pathPlaylist);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        public void CreatePlayList()
        {

            
            WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
            WMPLib.IWMPPlaylist playlist = wplayer.playlistCollection.newPlaylist("playlist");
            
            
            //Find media source
            var medias = EnumMusicSource();
            foreach ( var m in medias)
            {
                try
                {
                    //add media
                    var media = wplayer
                        .newMedia($"{Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)}\\{m}");
                    playlist.appendItem(media);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

        }

        public  IEnumerable<string> EnumMusicSource()
        {
            var extensions = new List<string>() { ".mp3", ".wav" };
            var pathMusicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            var files = 
                new List<string>(Directory.EnumerateFiles(pathMusicFolder, "*.*", SearchOption.TopDirectoryOnly))
                .Where( f => extensions.Contains(Path.GetExtension(f))); //use constain find particular item inside extentions

            foreach (var f in files)
            {
                FileInfo fi = new FileInfo(f);
                yield return fi.Name;
            }

        }
    }
}
