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
    class VideoCmdArgumentsStruct
    {
        public int PlayMode { get; set; } = 0;
        public int Position { get; set; } = 0;
        public int Volume { get; set; } = 0;
    }
    class VideoCmdStruct
    {
        public string Action { get; set; } = "";
        
    }
    internal class VideoControlArgumentsStruct
    {
        public int PlayMode { get; set; } = -1;
        public int Position { get; set; } = -1;
        public int? Volume { get; set; }
    }
    internal class VideoControlStruct
    {
        public string Action { get; set; } = "";
        public VideoControlArgumentsStruct? Arguments { get; set; }
    }
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
     
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            _videoView.MediaPlayer = _mediaPlayer;
            _mediaPlayer.Play(new Media(_libVLC, new Uri(_source)));
        }
        public void next()
        {

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
        /// <summary>
        /// 开始播放
        /// </summary>
        private void Play()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Play();
            }
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        private void Stop()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
            }
        }
        private void SetPlayMode(int mode)
        {
            if (_mediaPlayer != null)
            {
                
            }
        }
        /// <summary>
        /// 设置播放进度
        /// </summary>
        /// <param name="position">进度</param>
        private void SetPlayPosition(int position)
        {
            if (_mediaPlayer != null && _mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Position = position;
            }
        }
        /// <summary>
        /// 暂停播放
        /// </summary>
        private void Pause()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Pause();
            }
        }
        private void SetPlayVolume(int volume)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Volume = volume;
            }
        }
        /// <summary>
        /// 控制播放器
        /// </summary>
        /// <param name="body">接收到的命令</param>
        /// <returns></returns>
        public replyMessage Control(string body)
        {
            replyMessage reply = new replyMessage { };
            VideoControlStruct jsonBody = JsonConvert.DeserializeObject<VideoControlStruct>(body);

            if (jsonBody == null||jsonBody.Action == null || jsonBody.Action == "")
            {
                return reply;
            }
            switch (jsonBody.Action)
            {
                case "play":
                    Play();
                    break;
                case "stop":
                    Stop();
                    break;
                case "setPlayMode":
                    if (jsonBody.Arguments != null && jsonBody.Arguments.PlayMode != -1)
                    {
                        SetPlayMode(jsonBody.Arguments.PlayMode);
                    }
                    break;
                case "setPosition":
                    if (jsonBody.Arguments != null && jsonBody.Arguments.Position != -1)
                    {
                        SetPlayPosition(jsonBody.Arguments.Position);
                    }
                    break;
                    
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
