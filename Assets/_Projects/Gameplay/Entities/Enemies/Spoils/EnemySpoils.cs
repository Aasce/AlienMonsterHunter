using Asce.Core;
using Asce.Core.Attributes;
using Asce.Game.Interactions;
using Asce.Game.Items;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class EnemySpoils : GameComponent
    {
        [SerializeField, Readonly] private Enemy _owner;
        [SerializeField] private string _expOrbName = "Exp Orb";
        [SerializeField] private string _droppedItemName = "Dropped Item";

        public Enemy Owner
        {
            get => _owner;
            set => _owner = value;
        }

        public virtual void Initialize()
        {
            Owner.OnDead += Owner_OnDead;
        }

        public virtual void ResetStatus()
        {

        }

        private void Owner_OnDead(Combats.DamageContainer container)
        {
            if (container == null) return;
            if (container.Sender is Enemies.Enemy) return;

            this.SpawnExpOrb();
            this.SpawnSpoils();
        }

        private void SpawnExpOrb()
        {
            int level = Owner.Leveling.CurrentLevel;
            int exp = Owner.Information.DroppedSpoils.Exp;
            int addExp = Owner.Information.DroppedSpoils.AdditionalExpPerLevel * level;

            ExpOrb_InteractiveObject expOrb = InteractionController.Instance.Spawn(_expOrbName) as ExpOrb_InteractiveObject;
            if (expOrb == null) return;
            expOrb.transform.position = Owner.transform.position;
            expOrb.ExpAmount = exp + addExp;
            expOrb.OnActive();
        }

        private void SpawnSpoils()
        {
            foreach (Spoil spoil in Owner.Information.DroppedSpoils.Spoils) 
            {
                float dropChance = Random.Range(0f, 1f);
                bool isDrop = spoil.DropRate >= dropChance;

                if (!isDrop) continue;
                int quantity = Random.Range(spoil.MinQuantity, spoil.MaxQuantity + 1);

                DroppedItem_InteractiveObject droppedItem = InteractionController.Instance.Spawn(_droppedItemName) as DroppedItem_InteractiveObject;
                if (droppedItem == null) continue;

                droppedItem.transform.position = Owner.transform.position;
                droppedItem.Force();
                droppedItem.SetItem(spoil.Name, quantity);
                droppedItem.OnActive();
            }
        }
    }
}