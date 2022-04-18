using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace zoolon_container.player
{
    internal class VideoPlayer : iplayer
    {
        string _source="";
        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;
        VideoView _videoView;

        public VideoPlayer(string source)
        {
            _videoView = new VideoView();
            _source = source;
            _videoView.Loaded += VideoView_Loaded;

        }

        ~VideoPlayer()
        {
            _videoView.Loaded -= VideoView_Loaded;
            //if(_mediaPlayer!= null)
            //{
            //    _mediaPlayer.Stop();
            //    _mediaPlayer.Dispose();
            //}
            //if (_libVLC != null)
            //{
            //    _libVLC.Dispose();
            //}
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            _videoView.MediaPlayer = _mediaPlayer;
            _mediaPlayer.Play(new Media(_libVLC, new Uri(_source)));
        }

        public bool Close()
        {
            _videoView.Loaded -= VideoView_Loaded;
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
            }
            if (_libVLC != null)
            {
                _libVLC.Dispose();
            }
            _videoView.Dispose();
            this.Dispose();
            return true;
        }

        public replyMessage Control(string body)
        {
            replyMessage reply = new replyMessage { };
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
            Dictionary<string, object> arguments = new Dictionary<string, object>();

            if (!deserialized.ContainsKey("action"))
            {
                reply.content = "Action 必须填写";
                return reply;
            }
            string action = deserialized["action"].ToString();

            if (deserialized.ContainsKey("arguments"))
            {

                arguments = JsonConvert.DeserializeObject<Dictionary<string, object>>(deserialized["arguments"].ToString());
            }
            if (action == "play")
            {
                _mediaPlayer.Play();
            }
            else if (action == "stop")
            {
                _mediaPlayer.Stop();
            }
            else if (action == "pause")
            {
                _mediaPlayer.Pause();
            }
            else if (action == "setVolume")
            {
                if (!arguments.ContainsKey("volume"))
                {
                    reply.content = "volume not exists";
                    return reply;
                }
                _mediaPlayer.Volume = Int32.Parse(arguments["volume"].ToString());
            }
            else if (action == "setPosition")
            {
                if (!arguments.ContainsKey("position"))
                {
                    return reply;
                }
                _mediaPlayer.Position = Int32.Parse(arguments["position"].ToString());
            }
           
            return reply;
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
        public ContentControl GetComponents()
        {
            return _videoView;
        }
        public void Dispose()
        {
            _videoView?.Dispose();
            _mediaPlayer.Dispose();
            
        }

        public replyMessage Control(Hashtable args)
        {
            throw new NotImplementedException();
        }

        public PlayerType getType()
        {
            return PlayerType.Video;
        }
    }
}
