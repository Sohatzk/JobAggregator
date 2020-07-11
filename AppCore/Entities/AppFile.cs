using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Entities
{
    public class AppFile
    {
        public long AppFileId { get; set; }

        public byte[] Content { get; set; }
    }
}
