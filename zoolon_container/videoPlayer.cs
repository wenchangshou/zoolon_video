using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace zoolon_container
{
    internal class VideoPlayer : VideoView, iplayer
    {
        string _source="";
        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;

        public VideoPlayer(string source)
        {
            _source = source;
            this.Loaded += VideoView_Loaded;

        }

        ~VideoPlayer()
        {
            this.Loaded -= VideoView_Loaded;
            if(_mediaPlayer!= null)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
            }
            if (_libVLC != null)
            {
                _libVLC.Dispose();
            }
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            this.MediaPlayer = _mediaPlayer;
            _mediaPlayer.Play(new Media(_libVLC, new Uri(_source)));
        }

        public bool Close()
        {
            this.Loaded -= VideoView_Loaded;
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
            }
            if (_libVLC != null)
            {
                _libVLC.Dispose();
            }
            
            return true;
        }

        public replyMessage Control(string body)
        {
            throw new NotImplementedException();
        }

        public bool Exit()
        {
            throw new NotImplementedException();
        }

        public bool Open(string sourceDir)
        {
            _mediaPlayer.Play(new Media(_libVLC, new Uri(sourceDir)));
            return true;
        }

    }
}
