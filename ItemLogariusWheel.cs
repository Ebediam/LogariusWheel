using BS;
using UnityEngine;

namespace LogariusWheel
{

    public class ItemLogariusWheel : MonoBehaviour
    {
       
        protected Item item;
        public ItemModuleLogariusWheel module;

        
        public Transform otherWheel;
        public Transform otherWheelInterior;
        public Transform interiorWheel;

        public AudioSource trickSFX;
        public AudioSource spookySFX;

        public ParticleSystem spookyVFX;


        public Rigidbody interiorWheelRB;

        public bool isExtended = false;
        public bool isChanging = false;
        public float position = 0.07f;
        public float count = 0;        
        
        protected void Awake()
        {

            item = this.GetComponent<Item>();            
            item.OnCollisionEvent += OnWheelCollision;
            module = item.data.GetModule<ItemModuleLogariusWheel>();


            interiorWheel = item.transform.Find("LogariusWheelInterior");
            otherWheel = item.transform.Find("OtherWheel");
            otherWheelInterior = otherWheel.Find("OtherWheelInterior");
            interiorWheelRB = interiorWheel.GetComponent<Rigidbody>();

            trickSFX = item.transform.Find("TrickSFX").GetComponent<AudioSource>();
            spookySFX = item.transform.Find("SpookySFX").GetComponent<AudioSource>();
            spookyVFX = item.transform.Find("SpookyVFX").GetComponent<ParticleSystem>();

        }


        public void OnWheelCollision(ref CollisionStruct collisionInstance)
        {
            if (isExtended && collisionInstance.targetType == CollisionStruct.TargetType.NPC)
            {
                if(collisionInstance.targetCollider.transform.root.GetComponent<Creature>().state != Creature.State.Dead)
                {
                    Creature.player.health.currentHealth += collisionInstance.damageStruct.damage * 0.33f;
                }
                

            }

        }
       

        void FixedUpdate()
        {


            if(Mathf.Abs(interiorWheelRB.transform.InverseTransformVector(interiorWheelRB.angularVelocity).z) >= 5f && !isExtended && !isChanging || Mathf.Abs(interiorWheelRB.transform.InverseTransformVector(interiorWheelRB.angularVelocity).z) <= 1f && isExtended && !isChanging)
            {
                isChanging = true;
            }

            if (isExtended && item.IsHanded())
            {
                Creature.player.health.currentHealth -= 1f * Time.deltaTime;

                if (Creature.player.health.currentHealth <= 0f)
                {
                    Creature.player.health.Kill();
                }
            }

            if (isChanging )
            {
                ChangeWeapon();
            }

            otherWheelInterior.rotation = interiorWheel.rotation;

        }

      
        public void ChangeWeapon()
        {
            if (isExtended)
            {
                otherWheel.position = otherWheel.position + otherWheel.forward.normalized * (position / module.switchSpeed);

                count++;
                if(count >= module.switchSpeed)
                {
                    
                    count = 0f;
                    isExtended = false;
                    isChanging = false;
                    spookyVFX.Stop();
                    spookySFX.Stop();
                    trickSFX.Play();

                    item.damagers[0].data.addForce = 300;
                    item.damagers[0].data.damageMinMax = new Vector2(8f, 30f);
                }
            }
            else
            {
                otherWheel.position = otherWheel.position - otherWheel.forward.normalized * (position / module.switchSpeed);
                count++;
                if (count >= module.switchSpeed)
                {
                    
                    count = 0f;
                    isExtended = true;
                    isChanging = false;
                    spookySFX.Play();
                    spookyVFX.Play();
                    trickSFX.Play();
                    item.damagers[0].data.addForce = 500;
                    item.damagers[0].data.damageMinMax = new Vector2(12f, 50f);


                   

                }
            }
        }        
    }
}