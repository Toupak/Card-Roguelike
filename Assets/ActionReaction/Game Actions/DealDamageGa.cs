using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace ActionReaction.Game_Actions
{
    public class DealDamageGA : GameAction
    {
        public class DamagePackage
        {
            public int amount = 0;
            public bool isDamageNegated = false;
            public bool isTargetSwitched = false;

            public CardController target;
            public CardController originalTarget;

            public DamagePackage(int amount, CardController target)
            {
                this.amount = amount;
                this.target = target;
                this.originalTarget = target;
            }
        }
        
        public readonly CardController attacker;
        public List<DamagePackage> packages = new List<DamagePackage>();
        

        public bool bypassArmor = false;
        public bool isBongoAttack = false;

        public DealDamageGA(int damageAmount, CardController attackerController, CardController targetController)
        {
            attacker = attackerController;
            packages.Add(new DamagePackage(Mathf.Max(0, damageAmount), targetController));
        }
        
        public DealDamageGA(int damageAmount, CardController attackerController, List<CardController> targetControllers)
        {
            attacker = attackerController;

            int damage = Mathf.Max(0, damageAmount);
            foreach (CardController targetController in targetControllers)
            {
                packages.Add(new DamagePackage(damage, targetController));
            }
        }
        
        public void NegateDamage(DamagePackage package)
        {
            if (package != null)
            {
                package.amount = 0;
                package.isDamageNegated = true;
            }
        }

        public void NegateDamage(CardController target)
        {
            NegateDamage(GetPackageFromTarget(target));
        }
        
        public void SwitchTarget(DamagePackage package, CardController newTarget)
        {
            if (package != null)
            {
                package.originalTarget = package.target;
                package.target = newTarget;
                package.isTargetSwitched = true;
            }
        }

        public void SwitchTarget(CardController oldTarget, CardController newTarget)
        {
            SwitchTarget(GetPackageFromTarget(oldTarget), newTarget);
        }
        
        public DamagePackage GetPackageFromTarget(CardController target)
        {
            foreach (DamagePackage package in packages)
            {
                if (package.target == target)
                    return package;
            }
            
            return null;
        }
    }
}
