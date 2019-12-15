using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class WaitingUpdateLoopBox
    {
        private bool _initializationComplete;

        public static void Update(ref WaitingUpdateLoopBox box, Func<bool> condition, Action initialization, Action update=null)
        {
            if (box == null)
            {
                box = new WaitingUpdateLoopBox();
            }

            if (!box._initializationComplete)
            {
                if (condition())
                {
                    initialization();
                    box._initializationComplete = true;
                }
                else
                {
                    return;
                }
            }

            update?.Invoke();
        }
    }
}
