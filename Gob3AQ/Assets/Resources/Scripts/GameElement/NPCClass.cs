using UnityEngine;
using Gob3AQ.VARMAP.Types;
using Gob3AQ.VARMAP.NPCMaster;
using Gob3AQ.Waypoint;
using System.Collections;
using Unity.VisualScripting;


namespace Gob3AQ.GameElement.NPC
{
    [System.Serializable]
    public class NPCClass : MonoBehaviour
    {
        [SerializeField]
        public NPCType _npcType;

        private SpriteRenderer _sprRend;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private WaypointClass _waypoint;

        public NPCType NPType => _npcType;
        public Collider2D Collider => _collider;
        public WaypointClass Waypoint => _waypoint;


        public void InteractWithPlayer(CharacterType character)
        {
            Debug.Log("Interaction");
        }

        void Awake()
        {
            _sprRend = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            VARMAP_NPCMaster.NPC_REGISTER(this, true);
            StartCoroutine(_Execute_Loading());
        }


        void OnDestroy()
        {
            VARMAP_NPCMaster.NPC_REGISTER(this, false);
        }

        private IEnumerator _Execute_Loading()
        {
            yield return new WaitForNextFrameUnit();
            VARMAP_NPCMaster.GET_NEAREST_WP(transform.position, float.MaxValue, out _waypoint);
        }
    }
}
