using PotionBlues.Prototypes.InfiniteWaves;
using System.Collections;
using UnityEngine;

namespace PotionBlues.Prototypes.Autodispense
{
    public class CustomerScript : MonoBehaviour
    {
        public float WalkSpeed = 2;
        public float Patience;
        public PotionScript Potion;

        private Animator _anim;
        private float _walkSpeed;

        // Start is called before the first frame update
        void Start()
        {
            _anim = GetComponent<Animator>();
            _walkSpeed = WalkSpeed;

            var matProps = new MaterialPropertyBlock();
            var renderer = GetComponent<SpriteRenderer>();
            renderer.GetPropertyBlock(matProps);
            matProps.SetColor("_VestColor", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            renderer.SetPropertyBlock(matProps);
        }

        // Update is called once per frame
        void Update()
        {
            _anim.SetBool("Walking", Mathf.Abs(_walkSpeed) > WalkSpeed * 0.9f);

            transform.position += Vector3.right * _walkSpeed * Time.deltaTime;

            if (Mathf.Abs(_walkSpeed) < 0.01f)
            {
                Patience -= Time.deltaTime;
            }

            if (Patience < 0 || Potion != null)
            {
                _walkSpeed = -WalkSpeed;
                transform.localScale = new Vector3(_walkSpeed >= 0 ? 1 : -1, 1, 1);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<CounterScript>() != null)
            {
                StartCoroutine(StopWalking());
            }

            if (other.gameObject.GetComponent<DoorScript>() != null)
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator StopWalking()
        {
            for (int i = 0; i < 10; i++)
            {
                _walkSpeed -= WalkSpeed / 10;
                yield return new WaitForSeconds(0.1f);
            }

            _walkSpeed = 0;
        }

        public void BuyPotion(PotionScript potion)
        {
            Potion = potion;
            potion.gameObject.transform.SetParent(transform, true);
            potion.gameObject.transform.localPosition = Vector3.zero;
        }
    }
}
