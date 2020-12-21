﻿using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;

namespace PaladinMod.Modules
{
    public static class Assets
    {
        public static AssetBundle mainAssetBundle;

        public static Texture charPortrait;

        public static Sprite iconP;
        public static Sprite icon1;
        public static Sprite icon2;
        public static Sprite icon3;
        public static Sprite icon3b;
        public static Sprite icon3c;
        public static Sprite icon4;
        public static Sprite icon4b;

        public static GameObject lightningSpear;
        public static GameObject swordBeam;

        public static GameObject healEffectPrefab;
        public static GameObject healZoneEffectPrefab;
        public static GameObject torporEffectPrefab;

        public static GameObject swordSwing;
        public static GameObject spinningSlashFX;
        public static GameObject spinningSlashEmpoweredFX;

        public static GameObject hitFX;
        public static GameObject lightningHitFX;
        public static GameObject lightningImpactFX;

        public static GameObject torporVoidFX;

        public static Mesh defaultMesh;
        public static Mesh defaultSwordMesh;
        public static Mesh lunarMesh;
        public static Mesh lunarSwordMesh;
        public static Mesh hunterMesh;

        public static void PopulateAssets()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PaladinMod.paladin"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    var provider = new AssetBundleResourcesProvider("@Paladin", mainAssetBundle);
                    ResourcesAPI.AddProvider(provider);
                }
            }

            /*using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("MinerV2.MinerBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }*/

            charPortrait = mainAssetBundle.LoadAsset<Sprite>("texPaladinIcon").texture;

            iconP = mainAssetBundle.LoadAsset<Sprite>("PassiveIcon");
            icon1 = mainAssetBundle.LoadAsset<Sprite>("SlashIcon");
            icon2 = mainAssetBundle.LoadAsset<Sprite>("SpinSlashIcon");
            icon3 = mainAssetBundle.LoadAsset<Sprite>("LightningSpearIcon");
            icon3b = mainAssetBundle.LoadAsset<Sprite>("LightningBoltIcon");
            icon3c = mainAssetBundle.LoadAsset<Sprite>("HealIcon");
            icon4 = mainAssetBundle.LoadAsset<Sprite>("HealZoneIcon");
            icon4b = mainAssetBundle.LoadAsset<Sprite>("TorporIcon");

            lightningSpear = mainAssetBundle.LoadAsset<GameObject>("LightningSpear");
            swordBeam = mainAssetBundle.LoadAsset<GameObject>("SwordBeam");

            healEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealEffect");
            healZoneEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealZoneEffect");
            torporEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("TorporEffect");

            GameObject engiShieldObj = Resources.Load<GameObject>("Prefabs/Projectiles/EngiBubbleShield");

            Material shieldFillMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").GetComponent<MeshRenderer>().material);
            Material shieldOuterMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").Find("Edge").GetComponent<MeshRenderer>().material);

            shieldOuterMat.SetTexture("_EmTex", mainAssetBundle.LoadAsset<Texture>("texHealZone"));
            Material torporMat = UnityEngine.Object.Instantiate<Material>(shieldOuterMat);
            torporMat.SetTexture("_EmTex", mainAssetBundle.LoadAsset<Texture>("texTorpor"));

            healZoneEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().material = shieldOuterMat;
            torporEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().material = UnityEngine.Object.Instantiate<Material>(shieldOuterMat);
            torporEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().material.SetColor("_TintColor", Color.red);

            swordSwing = Assets.LoadEffect("PaladinSwing", "");
            spinningSlashFX = Assets.LoadEffect("SpinSlashEffect", "");
            spinningSlashEmpoweredFX = Assets.LoadEffect("EmpSpinSlashEffect", "");
            hitFX = Assets.LoadEffect("ImpactPaladinSwing", "");
            lightningHitFX = Assets.LoadEffect("LightningHitFX", "");
            lightningImpactFX = Assets.LoadEffect("LightningImpact", "Play_mage_R_lightningBlast");
            torporVoidFX = Assets.LoadEffect("TorporVoidFX", "RoR2_nullifier_attack1_explode_02");

            defaultMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladin");
            defaultSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshSword");
            lunarMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinLunar");
            lunarSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshSwordLunar");
            hunterMesh = mainAssetBundle.LoadAsset<Mesh>("HunterMesh");
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            EffectAPI.AddEffect(newEffect);

            return newEffect;
        }
    }
}