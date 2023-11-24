using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pinfull {
    // ServerInfo.cs
    public class ServerInfo {
        public bool IsOnline { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Color StatusColor { get; set; } = Color.Gray;
        public long PingTime { get; set; } = -1;
    }

}
