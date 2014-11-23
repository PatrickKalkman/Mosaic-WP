using System;

namespace Mosaic.Common
{
    public class FilenameGenerator
    {
        public string Generate()
        {
            return string.Format("Mosaic_{0}", DateTime.Now.ToString("yyMMddhhmmss"));
        }
    }
}
