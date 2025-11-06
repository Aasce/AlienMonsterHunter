using Asce.Game.Entities.Characters;
using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Supports;
using Asce.Managers;
using Asce.Managers.Utils;
using Asce.PrepareGame.Manager;
using Asce.PrepareGame.Players;
using Asce.PrepareGame.SaveLoads;
using Asce.SaveLoads;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.PrepareGame.Picks
{
    public class PickController : MonoBehaviourSingleton<PickController>, ISaveable<LastPickSaveData>, ILoadable<LastPickSaveData>
    {
        [SerializeField] private int _maxSupport = 2;

        [Header("Picked Prefabs")]
        [SerializeField] private Character _characterPrefab;
        [SerializeField] private Gun _gunPrefab;
        [SerializeField] private List<Support> _supportPrefabs = new(2);

        private Character _characterInstance;
        private Gun _gunInstance;

        public event Action<Character> OnPickCharacter;
        public event Action<Gun> OnPickGun;
        public event Action<int, Support> OnPickSupport;

        public int MaxSupport => _maxSupport;
        public Character CharacterPrefab => _characterPrefab;
        public Gun GunPrefab => _gunPrefab;
        public List<Support> SupportPrefabs => _supportPrefabs;

        public Character CharacterInstance => _characterInstance;
        public Gun GunInstance => _gunInstance;

        private void Start()
        {
            this.PickCharacter(null);
            this.PickGun(null);
            for (int i  = 0; i < _maxSupport; i++) 
            {
                this.PickSupport(i, null);
            }
        }

        public void PickCharacter(Character prefab)
        {
            if (CharacterPrefab == prefab) return;
            this.UnregisterCharacter();
            _characterPrefab = prefab;
            this.RegisterCharacter();
            this.OnPickCharacter?.Invoke(CharacterInstance);
        }

        public void PickGun(Gun prefab)
        {
            if (GunPrefab == prefab) return; 
            this.UnregisterGun();
            _gunPrefab = prefab;
            this.RegisterGun();
            this.OnPickGun?.Invoke(GunInstance);
        }

        public void PickSupport(int index, Support support)
        {
            if (index < 0) return;
            _supportPrefabs.InsertOrExpandAt(index, support);
            OnPickSupport?.Invoke(index, support);
        }

        private void RegisterCharacter()
        {
            Character oldCharacter = PrepareGameManager.Instance.Player.Character;
            if (_characterPrefab == null)
            {
                // Clear current character
                if (oldCharacter != null)
                {
                    // If gun exists, detach from old character
                    if (_gunInstance != null && _gunInstance.transform.IsChildOf(oldCharacter.transform))
                    {
                        _gunInstance.transform.SetParent(null);
                    }

                    Destroy(oldCharacter.gameObject);
                    PrepareGameManager.Instance.Player.Character = null;
                    _characterInstance = null;
                }
                return;
            }

            // Instantiate new character
            Character newCharacter = Instantiate(CharacterPrefab);

            // If player already has character, keep transform before replacing
            if (oldCharacter != null)
            {
                Vector2 position = oldCharacter.transform.position;
                Quaternion rotation = oldCharacter.transform.rotation;
                if (_gunInstance != null && _gunInstance.transform.IsChildOf(oldCharacter.transform))
                {
                    _gunInstance.transform.SetParent(null); // detach before destroy
                }
                Destroy(oldCharacter.gameObject);

                newCharacter.transform.SetPositionAndRotation(position, rotation);
            }

            // Assign new character
            newCharacter.Initialize();
            _characterInstance = newCharacter;
            PrepareGameManager.Instance.Player.Character = newCharacter;

            // If gun instance already exists, transfer it to new character
            if (_gunInstance != null)
            {
                newCharacter.Gun = _gunInstance;
            }
        }

        private void UnregisterCharacter()
        {
            // No need to destroy here, handled in RegisterCharacter
        }

        private void RegisterGun()
        {
            if (GunPrefab == null) return;

            Character playerCharacter = PrepareGameManager.Instance.Player.Character;

            // Instantiate new gun
            _gunInstance = Instantiate(GunPrefab);
            _gunInstance.Initialize();

            // If character exists
            if (playerCharacter != null)
            {
                // Destroy old gun if exists
                if (playerCharacter.Gun != null)
                {
                    Destroy(playerCharacter.Gun.gameObject);
                }

                playerCharacter.Gun = _gunInstance;
            }
        }

        private void UnregisterGun()
        {
            if (_gunInstance == null) return;

            // If attached to a character, clear reference first
            Character playerCharacter = PrepareGameManager.Instance.Player.Character;
            if (playerCharacter != null && playerCharacter.Gun == _gunInstance)
            {
                playerCharacter.Gun = null;
            }

            Destroy(_gunInstance.gameObject);
            _gunInstance = null;
        }

        LastPickSaveData ISaveable<LastPickSaveData>.Save()
        {
            LastPickSaveData lastPickData = new();
            lastPickData.characterName = CharacterPrefab != null ? CharacterPrefab.Information.Name : string.Empty;
            lastPickData.gunName = GunPrefab != null ? GunPrefab.Information.Name : string.Empty;
            for (int i = 0; i < _supportPrefabs.Count; i++)
            {
                Support support = _supportPrefabs[i];
                if (support == null) continue;
                lastPickData.supportIds.InsertOrExpandAt(i, support.Information.Key);
            }

            return lastPickData;
        }

        void ILoadable<LastPickSaveData>.Load(LastPickSaveData data)
        {
            if (data == null) return;

            Character character = GameManager.Instance.AllCharacters.Get(data.characterName);
            this.PickCharacter(character);

            Gun gun = GameManager.Instance.AllGuns.Get(data.gunName);
            this.PickGun(gun);

            for (int i = 0; i < data.supportIds.Count; i++)
            {
                string supportId = data.supportIds[i];
                Support support = GameManager.Instance.AllSupports.Get(supportId);
                if (support == null) continue;
                this.PickSupport(i, support);
            }
        }

    }
}
