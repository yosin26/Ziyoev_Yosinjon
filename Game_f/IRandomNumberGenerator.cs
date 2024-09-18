using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_f
{
    public interface IRandomNumberGenerator
    {
        int Generate(int minNumber, int maxNumber);
    }
}
