using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPongV1 {
    class PanelBufered : Panel {
        public PanelBufered() {
            this.DoubleBuffered = true;
        }
    }
}
