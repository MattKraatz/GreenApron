using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
