using System;
using System.Collections;
using ThunderRoad;
using Unity.XR.CoreUtils;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace kHeadbreaker {
    public class Script : ThunderScript {
        private ItemData brain;
        private EffectData visuals;
        private AudioContainer headbreakSFX;

        public override void ScriptEnable() {
            base.ScriptEnable();
            try {
                brain = Catalog.GetData<ItemData>("Kishi.Headbreaker.Brain");

                visuals = Catalog.GetData<EffectData>("Kishi.Headbreaker.VisualEffects.Headbreak");
                Catalog.LoadAssetAsync<AudioContainer>("Kishi.Headbreaker.Sounds.Headbreak", audioContainer => {
                    headbreakSFX = audioContainer;
                }, "Kishi.Headbreaker.Sounds.Headbreak");

                EventManager.onCreatureHit += EventManager_onCreatureHit;
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        private void EventManager_onCreatureHit(Creature creature, CollisionInstance collisionInstance, EventTime eventTime) {
            try {
                if (!ModOptions.enabled || eventTime == EventTime.OnEnd) {
                    return;
                }

                if (creature != null && !creature.isPlayer
                    && collisionInstance.damageStruct.damageType == DamageType.Blunt
                    && collisionInstance.damageStruct.hitRagdollPart.type is RagdollPart.Type.Head
                    && collisionInstance.impactVelocity.sqrMagnitude > 9.0f * 9.0f) {
                    Headbreak(creature, collisionInstance);
                }
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        private void Headbreak(Creature creature, CollisionInstance collisionInstance) {
            try {
                if (creature == null || collisionInstance == null) {
                    return;
                }

                var hitPart = collisionInstance.damageStruct.hitRagdollPart;
                if (hitPart != null && hitPart.type == RagdollPart.Type.Head) {
                    var position = hitPart.transform.position;
                    var force = (collisionInstance.impactVelocity.sqrMagnitude * Random.Range(1.0f, 2.0f)) * Random.onUnitSphere;

                    SpawnEffects(position);
                    brain?.SpawnAsync(spawnedBrain => {
                        spawnedBrain.transform.position = position;
                        spawnedBrain.physicBody.AddForce(force / 2.0f, ForceMode.Impulse);
                    });
                    SpawnSkullFragments(position, force);

                    creature.Kill();
                    hitPart.TrySlice();
                    hitPart.ragdoll.OnSliceEvent += Ragdoll_OnSliceEvent;
                }
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        private void Ragdoll_OnSliceEvent(RagdollPart ragdollPart, EventTime eventTime) {
            try {
                if (ragdollPart == null || ragdollPart.type != RagdollPart.Type.Head || eventTime != EventTime.OnEnd) {
                    return;
                }

                if (ragdollPart.handles != null && ragdollPart.handles.Count > 0) {
                    foreach (var handle in ragdollPart.handles) {
                        if (handle != null) {
                            foreach (var handler in handle.handlers) {
                                handler?.UnGrab(false);
                            }
                        }
                    }
                }

                ragdollPart.gameObject.SetActive(false);
                ragdollPart.ragdoll.OnSliceEvent -= Ragdoll_OnSliceEvent;
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        private void SpawnSkullFragments(Vector3 spawnPosition, Vector3 force) {
            try {
                InstantiateGameObject("Kishi.Headbreaker.Skull", spawnPosition, parent => {
                    foreach (var children in parent.GetComponentsInChildren<GameObject>()) {
                        if (children != null) {
                            var rb = children.GetOrAddComponent<Rigidbody>();
                            rb?.AddForce(force);
                        }
                    }
                });
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        private void InstantiateGameObject(string prefabPath, Vector3 position, Action<GameObject> callback) {
            try {
                GameManager.local.StartCoroutine(InstantiateGameObjectRoutine(prefabPath, position, callback));
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        private IEnumerator InstantiateGameObjectRoutine(string prefabPath, Vector3 position, Action<GameObject> callback) {
            GameObject go1 = null;
            Catalog.InstantiateAsync(prefabPath, position, Quaternion.identity, null, go => { go1 = go; }, prefabPath);

            yield return new WaitUntil(() => go1 != null);

            callback?.Invoke(go1);
        }

        private void SpawnEffects(Vector3 position) {
            try {
                if (headbreakSFX != null) {
                    var audio = new GameObject().AddComponent<AudioSource>();
                    if (audio != null) {
                        audio.playOnAwake = false;
                        audio.clip = headbreakSFX.PickAudioClip();
                        audio.spatialBlend = 0.75f;
                        audio.Play();
                    }
                }

                Visuals(position);
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        private void Visuals(Vector3 position) {
            try {
                GameManager.local.StartCoroutine(VisualsRoutine(position));
            } catch (Exception e) {
                Debug.LogException(e);
            }
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
            try {
                EventManager.onCreatureHit -= EventManager_onCreatureHit;
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }
    }
}