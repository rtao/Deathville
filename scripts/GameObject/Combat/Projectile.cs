using Deathville.Component;
using Deathville.Environment;
using Godot;
using GodotApiTools.Util;

namespace Deathville.GameObject.Combat
{
    public abstract class Projectile : Node2D
    {
        [Export]
        protected PackedScene _deathScene;

        public float Speed;
        public float Range;

        protected Vector2 _direction;
        protected float _distanceTravelled;

        private int _hitCount = 0;

        public void RegisterHit(RaycastResult raycastResult)
        {
            if (raycastResult.Collider is DamageReceiverComponent drc)
            {
                drc.RegisterHit(this, raycastResult);
            }
            _hitCount++;
            if (_hitCount >= 1)
            {
                GlobalPosition = raycastResult.Position;
                DieWithEffect(raycastResult);
            }
        }

        public virtual void DieWithEffect(RaycastResult raycastResult = null)
        {
            var death = _deathScene.Instance() as Node2D;
            Zone.Current.EffectsLayer.AddChild(death);
            death.Rotation = (raycastResult == null ? _direction.Angle() - Mathf.Pi : raycastResult.Normal.Angle());
            death.GlobalPosition = GlobalPosition;
        }

        public virtual void Die()
        {
            QueueFree();
        }

        public abstract void Start(Vector2 chamberPos, Vector2 spawnPos, Vector2 toPos);
        public abstract void SetPlayer();
        public abstract void SetEnemy();
    }
}