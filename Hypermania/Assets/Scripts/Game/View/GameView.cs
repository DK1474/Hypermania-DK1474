using System;
using System.Collections.Generic;
using Design;
using Game.Sim;
using UnityEngine;

namespace Game.View
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Conductor))]
    public class GameView : MonoBehaviour
    {
        private Conductor _conductor;
        public FighterView[] Fighters => _fighters;

        private FighterView[] _fighters;
        private ManiaView[] _manias;
        private CharacterConfig[] _characters;

        public ManiaViewConfig Config;

        public void Init(CharacterConfig[] characters)
        {
            _conductor = GetComponent<Conductor>();
            if (_conductor == null)
            {
                throw new InvalidOperationException(
                    "Conductor was null. Did you forget to assign a conductor component to the GameView?"
                );
            }
            _fighters = new FighterView[characters.Length];
            _manias = new ManiaView[characters.Length];
            _characters = characters;
            for (int i = 0; i < characters.Length; i++)
            {
                _fighters[i] = Instantiate(_characters[i].Prefab);
                _fighters[i].name = "Fighter View";
                _fighters[i].transform.SetParent(transform, true);
                _fighters[i].Init(characters[i]);

                float xPos = i - ((float)characters.Length - 1) / 2;
                GameObject maniaView = new GameObject("Mania View");
                _manias[i] = maniaView.AddComponent<ManiaView>();
                _manias[i].transform.SetParent(transform, true);
                _manias[i].Init(new Vector2(8f * xPos, 0f), Config);
            }
            _conductor.Init();
        }

        public void Render(in GameState state, GlobalConfig config)
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _fighters[i].Render(state.Frame, state.Fighters[i]);
                _manias[i].Render(state.Frame, state.Manias[i]);
            }
            _conductor.RequestSlice(state.Frame);

            List<Vector2> interestPoints = new List<Vector2>();
            for (int i = 0; i < _characters.Length; i++)
            {
                interestPoints.Add((Vector2)state.Fighters[i].Position);
            }
            DoCamera(interestPoints, config);
        }

        public void DeInit()
        {
            for (int i = 0; i < _characters.Length; i++)
            {
                _fighters[i].DeInit();
                Destroy(_fighters[i].gameObject);
                _manias[i].DeInit();
                Destroy(_manias[i].gameObject);
            }
            _fighters = null;
            _characters = null;
        }

        public void DoCamera(List<Vector2> interestPoints, GlobalConfig config)
        {
            const float PADDING = 0.5f;
            const float ASPECT = 16f / 9;
            const float MIN_ZOOM = 3f;
            const float MAX_ZOOM = 8f;

            if (interestPoints.Count == 0)
            {
                throw new InvalidOperationException("Camera needs at least one point of interest");
            }
            Vector2 min = Vector2.positiveInfinity;
            Vector2 max = Vector2.negativeInfinity;
            foreach (Vector2 pt in interestPoints)
            {
                min = Vector2.Min(min, pt);
                max = Vector2.Max(max, pt);
            }
            Vector2 center = (min + max) / 2f;

            float totalWidth = max.x - min.x + PADDING * 2;
            float totalHeight = max.y - min.y + PADDING * 2;
            float horizAspect = totalWidth / (2 * ASPECT);
            float vertAspect = totalHeight / 2;
            Camera.main.orthographicSize +=
                (Mathf.Clamp(Mathf.Max(horizAspect, vertAspect), MIN_ZOOM, MAX_ZOOM) - Camera.main.orthographicSize)
                * 0.1f;
            Camera.main.transform.position +=
                (new Vector3(center.x, center.y, Camera.main.transform.position.z) - Camera.main.transform.position)
                * 0.1f;
        }
    }
}
