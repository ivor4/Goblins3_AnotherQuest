using UnityEditor;
using UnityEngine;
using MVerse.VARMAP.PlayerMaster;
using MVerse.VARMAP.Types;
using MVerse.FixedConfig;
using System;

namespace MVerse.PlayerMaster
{
    public enum PhysicalState
    {
        PHYSICAL_STATE_FALLING = 0,
        PHYSICAL_STATE_STANDING = 1,
        PHYSICAL_STATE_JUMPING = 2,
        PHYSICAL_STATE_ATTACKING = 4,
        PHYSICAL_STATE_WALL_CLIMBING = 8,
        PHYSICAL_STATE_INVINCIBLE = 16,
    }

    public class PlayerMasterClass : MonoBehaviour
    {
        /* PI Medios = 90º */
        private const double PI_DIV_TWO = Math.PI / 2d;

        /* "Public" fields */
        [SerializeField]
        private PhysicsMaterial fallMaterial;
        [SerializeField]
        private PhysicsMaterial standMaterial;

        /* GameObject components */
        private Collider actualcollider;
        private CapsuleCollider capsulecollider;
        private Rigidbody myrigidbody;
        private MeshRenderer myrenderer;

        /* Precalculated data */
        private int solidMask;

        /* Status */
        private PhysicalState physicalstate;

        /* Receive damage data */
        private float invincibletimeout;

        /* Stand data */
        private GameObject standing_platform;


        /* Climb data */
        private GameObject climbing_wall;
        private float climb_wall_timeout;
        private bool climb_wall_direction_right;

        /* Physics fix */
        private Vector3 velocity_pre_collision;
        private Vector3 surface_stand_normal;

        /* VARMAP cached */
        private Vector3Struct position_struct;
        private Powers cached_powers;
        private Charm cached_charm;

        private void Awake()
        {
            capsulecollider = GetComponent<CapsuleCollider>();
            myrigidbody = GetComponent<Rigidbody>();
            myrenderer = GetComponent<MeshRenderer>();

            actualcollider = capsulecollider;
        }

        private void Start()
        {
            Fall();
            climbing_wall = null;

            solidMask = 1 << (int)CollisionLayer.LAYER_SOLID;

            VARMAP_PlayerMaster.MONO_REGISTER(this, true);
        }


        private void Update()
        {
            Execute_Play();
        }

        private void FixedUpdate()
        {
            velocity_pre_collision = myrigidbody.linearVelocity;
        }


        private void Execute_Play()
        {
            KeyStruct keyInfo = VARMAP_PlayerMaster.GET_PRESSED_KEYS();

            UpdateCachedPowers();
            UpdateCachedCharm();

            /* Physics check */
            if ((physicalstate & PhysicalState.PHYSICAL_STATE_JUMPING) != 0)
            {
                UpdateJump(ref keyInfo);
            }

            if ((physicalstate & PhysicalState.PHYSICAL_STATE_WALL_CLIMBING) != 0)
            {
                UpdateClimb(ref keyInfo);
            }

            if ((physicalstate & PhysicalState.PHYSICAL_STATE_INVINCIBLE) != 0)
            {
                UpdateInvincible();
            }


            /* Key Input */
            if ((physicalstate & (PhysicalState.PHYSICAL_STATE_ATTACKING | PhysicalState.PHYSICAL_STATE_WALL_CLIMBING)) == 0)
            {
                bool comboDone = ReadCasualCombos(ref keyInfo);

                /* In case no combo is executed, read simple movement keys */
                if (!comboDone)
                {
                    ReadMovementKeys(ref keyInfo);
                }
            }


            position_struct.position = transform.position;
            VARMAP_PlayerMaster.SET_PLAYER_POSITION(position_struct);
        }

        /// <summary>
        /// Reads combos and returns true in case one is executed
        /// </summary>
        /// <returns>true if combo is going to be executed</returns>
        private bool ReadCasualCombos(ref KeyStruct keyInfo)
        {
            bool retVal = false;

            switch (keyInfo.activeCombo)
            {
                case KeyCombo.KEY_COMBO_NONE:
                    break;


                default:
                    break;
            }

            return retVal;
        }

        private void ReadMovementKeys(ref KeyStruct keyInfo)
        {
            if (((keyInfo.cyclepressedKeys & KeyFunctions.KEYFUNC_JUMP) != 0) && ((physicalstate & PhysicalState.PHYSICAL_STATE_STANDING) != 0))
            {
                Jump(0);
            }

            if ((keyInfo.pressedKeys & KeyFunctions.KEYFUNC_LEFT) != 0)
            {
                MoveLeft();
            }
            else if ((keyInfo.pressedKeys & KeyFunctions.KEYFUNC_RIGHT) != 0)
            {
                MoveRight();
            }
            else
            {

            }
        }


        private void OnTriggerStay(Collider other)
        {
            GameObject otherObject = other.gameObject;

            CollisionLayer clayer = (CollisionLayer)otherObject.layer;

            switch (clayer)
            {
                case CollisionLayer.LAYER_ENEMY:
                    if ((physicalstate & PhysicalState.PHYSICAL_STATE_INVINCIBLE) == 0)
                    {
                        ReceiveDamage(1, transform.position - otherObject.transform.position);
                    }
                    break;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            GameObject otherObject = collision.gameObject;
            CollisionLayer clayer = (CollisionLayer)otherObject.layer;
            ContactPoint cpoint = collision.GetContact(0);


            switch (clayer)
            {
                case CollisionLayer.LAYER_SOLID:

                    /* Annulate deflect velocity due to collision [x axis]
                     * (this happens when the round part of capsule collides an edge and the whole body gets deflected) */
                    if (((physicalstate & PhysicalState.PHYSICAL_STATE_STANDING) == 0) &&
                        (Math.Abs(cpoint.normal.x) < GameFixedConfig.CLIMB_WALL_MIN_NORMAL_X))
                    {
                        myrigidbody.linearVelocity = new Vector3(velocity_pre_collision.x, myrigidbody.linearVelocity.y, myrigidbody.linearVelocity.z);
                        velocity_pre_collision = myrigidbody.linearVelocity;
                    }

                    /* Decide if standing or climbing (if power is achieved) */
                    if (cpoint.normal.y > GameFixedConfig.COLLISION_STAND_MIN_NORMAL_Y)
                    {
                        Stand(otherObject);
                        surface_stand_normal = cpoint.normal;
                    }
                    else if (((cached_powers & Powers.POWER_CLIMB) != 0) && (climbing_wall != otherObject) &&
                        ((physicalstate & PhysicalState.PHYSICAL_STATE_JUMPING) != 0) &&
                        (Math.Abs(cpoint.normal.x) > GameFixedConfig.CLIMB_WALL_MIN_NORMAL_X))
                    {
                        ClimbWall(otherObject, cpoint.normal.x);
                    }
                    break;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            GameObject otherObject = collision.gameObject;
            CollisionLayer clayer = (CollisionLayer)otherObject.layer;
            ContactPoint cpoint = collision.GetContact(0);


            switch (clayer)
            {
                case CollisionLayer.LAYER_SOLID:
                    /* Annulate deflect velocity due to collision [x axis]
                     * (this happens when the round part of capsule collides an edge and the whole body gets deflected) */
                    if (((physicalstate & PhysicalState.PHYSICAL_STATE_STANDING) == 0) &&
                        (Math.Abs(cpoint.normal.x) < GameFixedConfig.CLIMB_WALL_MIN_NORMAL_X))
                    {
                        myrigidbody.linearVelocity = new Vector3(velocity_pre_collision.x, myrigidbody.linearVelocity.y, myrigidbody.linearVelocity.z);
                        velocity_pre_collision = myrigidbody.linearVelocity;
                    }

                    if (otherObject == standing_platform)
                    {
                        surface_stand_normal = cpoint.normal;
                    }
                    break;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            GameObject otherObject = collision.gameObject;

            if (standing_platform == otherObject)
            {
                Fall();
            }

            if ((climbing_wall == otherObject) && ((physicalstate & PhysicalState.PHYSICAL_STATE_WALL_CLIMBING) != 0))
            {
                Fall();
            }
        }


        private void Stand(GameObject platform)
        {
            AddPhysicalFlag(PhysicalState.PHYSICAL_STATE_STANDING);
            RemovePhysicalFlag(PhysicalState.PHYSICAL_STATE_JUMPING);
            RemovePhysicalFlag(PhysicalState.PHYSICAL_STATE_WALL_CLIMBING);

            standing_platform = platform;
            capsulecollider.material = standMaterial;
            climbing_wall = null;
        }

        private void Fall()
        {
            RemovePhysicalFlag(PhysicalState.PHYSICAL_STATE_STANDING);
            RemovePhysicalFlag(PhysicalState.PHYSICAL_STATE_WALL_CLIMBING);

            standing_platform = null;
            capsulecollider.material = fallMaterial;

            surface_stand_normal = Vector3.zero;

            climb_wall_timeout = 0f;
            climb_wall_direction_right = false;
        }

        private void Jump(float additionalSpeedX)
        {
            Fall();
            AddPhysicalFlag(PhysicalState.PHYSICAL_STATE_JUMPING);
            myrigidbody.linearVelocity = new Vector3(myrigidbody.linearVelocity.x, 10f, myrigidbody.linearVelocity.z);
        }

        private void MoveLeft()
        {
            MoveLeftRight(-1f);
        }

        private void MoveRight()
        {
            MoveLeftRight(1f);
        }

        private void MoveLeftRight(float multiplier)
        {
            Ray ray = new Ray(new Vector3(actualcollider.bounds.center.x, actualcollider.bounds.max.y,
                actualcollider.bounds.center.z), multiplier*Vector3.right);

            float deltax;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, GameFixedConfig.PLAYER_NORMAL_MOVEMENT_SPEED * Time.fixedDeltaTime,
                solidMask, QueryTriggerInteraction.Ignore))
            {
                deltax = hitInfo.distance - actualcollider.bounds.extents.x;
            }
            else
            {
                deltax = GameFixedConfig.PLAYER_NORMAL_MOVEMENT_SPEED * Time.fixedDeltaTime;
            }

            deltax *= multiplier;

            float deltay = 0;

            if ((physicalstate & PhysicalState.PHYSICAL_STATE_STANDING) != 0)
            {
                /* Producto escalar de toda la vida =>  Va·Vb = |Va|*|Vb|*cos(Va^Vb)  [Los modulos de ambos es 1],
                 * nos interesa sacar Va^Vb con el arcocoseno */
                double scalar = Vector3.Dot(-surface_stand_normal, Vector3.right);
                double angle_normal_to_movement = Math.Acos(scalar);
                /* Un triangulo suma angulos internos a 180 (PI), si a PI le restamos el angulo recto de 90º (PI/2),
                 * nos quedamos en PI/2, si le restamos el angulo que tenemos, sacamos el que falta */
                double angle_movement_to_surface = PI_DIV_TWO - angle_normal_to_movement;
                /* Tan(alpha) = DeltaY/DeltaX    =>    DeltaY = Tan(alpha)*DeltaX    */
                deltay = (float)(Math.Tan(angle_movement_to_surface) * deltax);
            }

            myrigidbody.MovePosition(new Vector3(transform.position.x + deltax, transform.position.y + deltay, transform.position.z));
        }

        private void ClimbWall(GameObject wall, float normalX)
        {
            AddPhysicalFlag(PhysicalState.PHYSICAL_STATE_WALL_CLIMBING);
            RemovePhysicalFlag(PhysicalState.PHYSICAL_STATE_JUMPING);
            capsulecollider.material = standMaterial;
            myrigidbody.linearVelocity = Vector3.zero;

            climb_wall_timeout = 0f;
            climbing_wall = wall;
            
            if(normalX > 0f)
            {
                climb_wall_direction_right = true;
            }
            else
            {
                climb_wall_direction_right = false;
            }
        }

        private void UpdateJump(ref KeyStruct keyInfo)
        {
            if (myrigidbody.linearVelocity.y < 0)
            {
                RemovePhysicalFlag(PhysicalState.PHYSICAL_STATE_JUMPING);
            }
            else
            {
                /* If player released jump key before speed changing sign */
                if((keyInfo.pressedKeys & KeyFunctions.KEYFUNC_JUMP) == 0)
                {
                    myrigidbody.linearVelocity = new Vector3(myrigidbody.linearVelocity.x,
                        myrigidbody.linearVelocity.y * GameFixedConfig.PLAYER_JUMP_SPEED_REDUCTION_SHORT_PRESS, myrigidbody.linearVelocity.z);
                    RemovePhysicalFlag(PhysicalState.PHYSICAL_STATE_JUMPING);
                }
            }
        }

        private void UpdateClimb(ref KeyStruct keyInfo)
        {
            climb_wall_timeout += Time.deltaTime;

            if(climb_wall_timeout >= GameFixedConfig.CLIMB_WALL_TIMEOUT)
            {
                Fall();
            }
            else
            {
                myrigidbody.linearVelocity = Vector3.zero;

                switch (keyInfo.activeCombo)
                {
                    case KeyCombo.KEY_COMBO_NONE:
                        break;
                    case KeyCombo.KEY_COMBO_CLIMB_WALL_LEFT:
                        if (((physicalstate & PhysicalState.PHYSICAL_STATE_WALL_CLIMBING) != 0) && (!climb_wall_direction_right))
                        {
                            Jump(0);
                        }
                        break;
                    case KeyCombo.KEY_COMBO_CLIMB_WALL_RIGHT:
                        if (((physicalstate & PhysicalState.PHYSICAL_STATE_WALL_CLIMBING) != 0) && (climb_wall_direction_right))
                        {
                            Jump(0);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void UpdateInvincible()
        {
            invincibletimeout += Time.deltaTime;

            if(invincibletimeout >= GameFixedConfig.RECEIVE_DAMAGE_INVINCIBLE_TIMEOUT)
            {
                invincibletimeout = 0f;
                RemovePhysicalFlag(PhysicalState.PHYSICAL_STATE_INVINCIBLE);
                myrenderer.material.color = Color.white;
            }
        }


        private void ReceiveDamage(byte damage, Vector3 deltaFromEnemy)
        {
            AddPhysicalFlag(PhysicalState.PHYSICAL_STATE_INVINCIBLE);
            invincibletimeout = 0f;
            myrigidbody.linearVelocity = new Vector3(Mathf.Sign(deltaFromEnemy.x) * 2f, 3f, 0);
            myrenderer.material.color = Color.red;

            VARMAP_PlayerMaster.SET_LIFE_ACTUAL((byte)Mathf.Max(0, VARMAP_PlayerMaster.GET_LIFE_ACTUAL() - damage));
        }

        /// <summary>
        /// MUST be called every Update, so Collide/Trigger events can read cached value.
        /// This cached value must be constantly updated for safety reason and should not be stored with
        /// same value (by event) for a long time as it would be target for memory attacks
        /// </summary>
        private void UpdateCachedPowers()
        {
            cached_powers = Powers.POWER_NONE;

            ReadOnlySpan<bool> tempPowers = VARMAP_PlayerMaster.GET_ARRAY_POWERS();

            for(int i=0;i<tempPowers.Length;i++)
            {
                cached_powers |= (Powers)(tempPowers[i] ? (1 << i) : 0);
            }
        }

        /// <summary>
        /// MUST be called every Update, so Collide/Trigger events can read cached value.
        /// This cached value must be constantly updated for safety reason and should not be stored with
        /// same value (by event) for a long time as it would be target for memory attacks
        /// </summary>
        private void UpdateCachedCharm()
        {
            cached_charm = Charm.CHARM_NONE;

            ReadOnlySpan<byte> tempCharms = VARMAP_PlayerMaster.GET_ARRAY_SELECTED_CHARMS();

            for (int i = 0; i < tempCharms.Length; i++)
            {
                cached_charm |= (Charm)tempCharms[i];
            }
        }

        private void AddPhysicalFlag(PhysicalState addedState)
        {
            physicalstate |= addedState;
            Debug.Log("Set to: " + physicalstate.ToString() + " physical state");
        }

        private void RemovePhysicalFlag(PhysicalState removedState)
        {
            physicalstate &= ~removedState;
            Debug.Log("Set to: " + physicalstate.ToString() + " physical state");
        }



        private void OnDestroy()
        {
            VARMAP_PlayerMaster.MONO_REGISTER(this, false);
        }
    }
}