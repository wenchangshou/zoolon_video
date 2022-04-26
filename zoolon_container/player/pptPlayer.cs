using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ppt = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using System.Collections;
using Newtonsoft.Json;

namespace zoolon_container.player
{
    class PptCmdArguments
    {
        public int Page { get; set; } = -1;
    }
    class PptCmdStruct
    {
        public string Action { get; set; } = "";
        public PptCmdArguments? Arguments { get; set; } 
    }
    class PptPlayer : iplayer
    {
        private customPptPlayer player;
        string _source;
        public PptPlayer(string source)
        {
         
            _source = source;
            player = new customPptPlayer(source);

        }

       
        public bool Close()
        {

            player.Close();
            return true;
        }

        public replyMessage Control(string body)
        {
            replyMessage reply = new replyMessage { };
            PptCmdStruct jsonBody = JsonConvert.DeserializeObject<PptCmdStruct>(body);

            if (jsonBody == null || jsonBody.Action == null || jsonBody.Action == "")
            {
                return reply;
            }
            switch (jsonBody.Action)
            {
                case "next":
                    player.Next();
                    break;
                case "pre":
                    player.Prev();
                    break;
                case "first":
                    player.First();
                    break;
                case "last":
                    player.Last();
                    break;
                case "goPage":
                    if (jsonBody.Arguments != null)
                    {
                        player.GoPage(jsonBody.Arguments.Page);
                    }
                    break;
            }
            return reply;
        }

        public replyMessage Control(Hashtable args)
        {
            return null;
        }

        public bool Exit()
        {
            throw new NotImplementedException();
        }

        public ContentControl GetComponents()
        {
            return player;
        }

        public PlayerType getType()
        {
            return PlayerType.PPT;
        }

        public bool Open(string sourceDir)
        {
            player.OpenPpt(sourceDir);
            return true;
        }
    }
}
