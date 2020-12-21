﻿namespace PaladinMod.States
{
    public class CastTorpor : BaseCastSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 1.8f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.torpor;

            base.OnEnter();
        }
    }
}