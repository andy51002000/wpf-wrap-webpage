using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        AlexaIoTClient.Helper.MediaPlayerHelper mediaPlayerHelper = new AlexaIoTClient.Helper.MediaPlayerHelper();


        [TestMethod]
        public void TestEnumMusicSource()
        {
            var output = mediaPlayerHelper.EnumMusicSource();
            List<string> expected = new List<string>()
            {
                "Counting Stars.mp3",
                "Canon in C Major.mp3",
                "Counting.wav"

            };
            var rst = expected.Where(x => !output.Contains(x)).FirstOrDefault();
            //They should habe the same items
            Assert.IsTrue(string.IsNullOrEmpty(rst));
        }

        [TestMethod]
        public void TestCreatePlayList()
        {

            var pathPlaylist = 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Playlists\\playlist.wpl");
            Assert.IsTrue(pathPlaylist == @"C:\Users\andyl\Music\Playlists\playlist.wpl");

            mediaPlayerHelper.CreatePlayList();
            Assert.IsTrue(File.Exists(pathPlaylist));
        }

        [TestMethod]
        public void TestRemovePlayList()
        {

            var pathPlaylist =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Playlists\\playlist.wpl");
            Assert.IsTrue(pathPlaylist == @"C:\Users\andyl\Music\Playlists\playlist.wpl");

            //pre-check, must have this befor testing remove
            Assert.IsTrue(File.Exists(pathPlaylist));

            mediaPlayerHelper.RemovePlayList();
            Assert.IsTrue(!File.Exists(pathPlaylist));
        }

    }
}
