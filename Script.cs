using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace kHeadbreaker {
    public class Script : ThunderScript {
        private ItemData brain;
        private EffectData visuals;
        private AudioContainer headbreakSFX;

        public override void ScriptEnable() {
            base.ScriptEnable();
            brain = Catalog.GetData<ItemData>("Kishi.Headbreaker.Brain");

            visuals = Catalog.GetData<EffectData>("Kishi.Headbreaker.VisualEffects.Headbreak");
            Catalog.LoadAssetAsync<AudioContainer>("Kishi.Headbreaker.Sounds.Headbreak", audioContainer => {
                headbreakSFX = audioContainer;
            }, "Kishi.Headbreaker.Sounds.Headbreak");

            EventManager.onCreatureHit += EventManager_onCreatureHit;
        }

        private void EventManager_onCreatureHit(Creature creature, CollisionInstance collisionInstance, EventTime eventTime) {
            if (eventTime == EventTime.OnEnd) {
                return;
            }

            if (creature != null && !creature.isPlayer
                && collisionInstance.damageStruct.damageType == DamageType.Blunt
                && collisionInstance.damageStruct.hitRagdollPart.type is RagdollPart.Type.Head
                && collisionInstance.impactVelocity.sqrMagnitude > 9.0f * 9.0f) {
                Headbreak(creature, collisionInstance);
            }
        }

        private void Headbreak(Creature creature, CollisionInstance collisionInstance) {
            if (creature == null || collisionInstance == null) {
                return;
            }

            var creatureHead = creature.Head();
            if (creatureHead != null) {
                var headPosition = creature.Head().transform.position;
                var force = (collisionInstance.impactVelocity.sqrMagnitude * Random.Range(1.0f, 2.0f)) * Random.onUnitSphere;

                SpawnEffects(headPosition);

                brain?.SpawnAsync(spawnedBrain => {
                    spawnedBrain.transform.position = headPosition;
                    spawnedBrain.physicBody.AddForce(force / 3.0f, ForceMode.Impulse);
                });
                SpawnFragments(headPosition, force / 2.0f);

                creature.Kill();
                creatureHead.TrySlice();
                creatureHead.ragdoll.OnSliceEvent += Ragdoll_OnSliceEvent;
            }
        }

        private void Ragdoll_OnSliceEvent(RagdollPart ragdollPart, EventTime eventTime) {
            if (eventTime == EventTime.OnEnd && ragdollPart.type is RagdollPart.Type.Head) {
                foreach (var handle in ragdollPart.handles) {
                    if (handle != null) {
                        foreach (var handler in handle.handlers) {
                            handler?.UnGrab(false);
                        }
                    }
                }
                Object.Destroy(ragdollPart.gameObject);
                ragdollPart.ragdoll.OnSliceEvent -= Ragdoll_OnSliceEvent;
            }
        }

        private void SpawnFragments(Vector3 spawnPosition, Vector3 force) {
            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.Ethmoid", spawnPosition, out var ethmoid);
            ethmoid?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.Frontal", spawnPosition, out var frontal);
            frontal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.InferiorConchae", spawnPosition, out var inferiorConchae);
            inferiorConchae?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.Jaw", spawnPosition, out var jaw);
            jaw?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.LeftLacrimal", spawnPosition, out var leftLacrimal);
            leftLacrimal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.LeftMaxilla", spawnPosition, out var leftMaxilla);
            leftMaxilla?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.LeftNasal", spawnPosition, out var leftNasal);
            leftNasal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.LeftPalatine", spawnPosition, out var leftPalatine);
            leftPalatine?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.LeftParietal", spawnPosition, out var leftParietal);
            leftParietal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.LeftTemporal", spawnPosition, out var leftTemporal);
            leftTemporal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.LeftZygomatic", spawnPosition, out var leftZygomatic);
            leftZygomatic?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.UpperTeeth", spawnPosition, out var upperTeeth);
            upperTeeth?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.LowerTeeth", spawnPosition, out var lowerTeeth);
            lowerTeeth?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.Occipital", spawnPosition, out var occipital);
            occipital?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.RightLacrimal", spawnPosition, out var rightLacrimal);
            rightLacrimal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.RightMax", spawnPosition, out var rightMax);
            rightMax?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.RightNasal", spawnPosition, out var rightNasal);
            rightNasal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.RightPalatine", spawnPosition, out var rightPalatine);
            rightPalatine?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.RightParietal", spawnPosition, out var rightParietal);
            rightParietal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.RightTemporal", spawnPosition, out var rightTemporal);
            rightTemporal?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.RightZygomatic", spawnPosition, out var rightZygomatic);
            rightZygomatic?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.Sphenoid", spawnPosition, out var sphenoid);
            sphenoid?.GetOrAddComponent<Rigidbody>()?.AddForce(force);

            InstantiateGameObject("Kishi.Headbreaker.SkullFragments.Vomer", spawnPosition, out var vomer);
            vomer?.GetOrAddComponent<Rigidbody>()?.AddForce(force);
        }
        
        private void InstantiateGameObject(string prefabPath, Vector3 position, out GameObject gameObject) {
            GameObject go1 = null;
            Catalog.InstantiateAsync(prefabPath, position, Quaternion.identity, null, go => { go1 = go; }, prefabPath);
            gameObject = go1;
        }

        private void SpawnEffects(Vector3 position) {
            if (headbreakSFX != null) {
                var audio = new GameObject().AddComponent<AudioSource>();
                if (audio != null) {
                    audio.playOnAwake = false;
                    audio.clip = headbreakSFX.PickAudioClip();
                    audio.spatialBlend = 0.75f;
                    audio.Play();
                }
            }

            GameManager.local.StartCoroutine(VisualsRoutine(position));
        }

        private IEnumerator VisualsRoutine(Vector3 position) {
            if (visuals != null) {
                var effectInstance = visuals.Spawn(position, Quaternion.identity);
                effectInstance?.Play();
                yield return new WaitForSeconds(0.2f);
                effectInstance?.Stop();
            }
        }


        public override void ScriptDisable() {
            base.ScriptDisable();
            EventManager.onCreatureHit -= EventManager_onCreatureHit;
        }
    }
}