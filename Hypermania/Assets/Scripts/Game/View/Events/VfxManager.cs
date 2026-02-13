using System;
using System.Collections;
using UnityEngine;

namespace Game.View.Events
{
    public struct VfxEvent : IEquatable<VfxEvent>
    {
        public VfxKind Kind;
        public Vector2 Position;
        public Vector2 KnockbackVector;

        public bool Equals(VfxEvent other)
        {
            return Kind == other.Kind;
        }
    }

    // TODO: should create vfxeffect base component that determines how to start/stop/play each effect, and then call
    // that here
    public class VfxManager : ViewEventManager<VfxEvent, Animator>
    {
        [SerializeField]
        private VfxLibrary _vfxLibrary;

        public override Animator OnStartEffect(ViewEvent<VfxEvent> ev)
        {
            GameObject vfx = Instantiate(_vfxLibrary.Library[ev.Event.Kind].Effect);
            vfx.transform.SetParent(transform);
            vfx.transform.position = new Vector3(ev.Event.Position.x, ev.Event.Position.y, transform.position.z);
            vfx.transform.rotation = Quaternion.FromToRotation(Vector3.right, -ev.Event.KnockbackVector);
            Animator animator = vfx.GetComponent<Animator>();
            animator.Play("Block");
            return animator;
        }

        public override void OnEndEffect(Animator anim)
        {
            Destroy(anim.gameObject);
        }

        public override bool EffectIsFinished(Animator anim)
        {
            return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
    }
}
