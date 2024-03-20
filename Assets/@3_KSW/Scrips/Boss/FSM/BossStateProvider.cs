using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossStateProvider // 객체화만 막으려고 추상화 (불필요한지 확인 필요)
{
    protected BossStateMachine _context;
    public BossStateProvider(BossStateMachine context)
    {
        _context = context;
    }

    // Root
    public abstract BossBaseState Chasing();
    public abstract BossBaseState Attack();
    public abstract BossBaseState Dead();
}
