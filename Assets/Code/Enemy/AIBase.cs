using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Enemy
{
    public interface AIBase
    {
        AIState ai { get; set; }
        void IdleBehaviour();
        void AggroBehaviour();
        void SearchBehaviour();


    }

    public enum AIState
    {
        idle,
        aggro,
        search,
        dead
    }
}
