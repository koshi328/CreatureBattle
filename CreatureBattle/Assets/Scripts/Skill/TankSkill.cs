//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Skill
//{
//    public class BackJump : SkillBase
//    {
//        public override void Initialize(Actor owner)
//        {
//            base.Initialize(owner);
//            REQUIREMENT_CAST_TIME = 0.0f;
//            REQUIREMENT_RECAST_TIME = 5.0f;
//            _castTime = REQUIREMENT_CAST_TIME;
//            _recastTime = REQUIREMENT_RECAST_TIME;
//        }
//        public override SkillBase Execute(Actor owner)
//        {
//            base.Execute(owner);

//            if (_castTime <= 0)
//            {
                


//                owner.CancelAction();

//                OnFinished();

//                return null;
//            }
//            return this;
//        }

//        public override void Dispose()
//        {

//        }

//        private void OnFinished()
//        {
//            _owner.AnimationSetTrigger("Idle");
//        }
//    }

//}
