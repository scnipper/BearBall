using UnityEngine;

namespace Common
{
    public class ColliderIdentificator : MonoBehaviour
    {
        public enum TypeCollides
        {
            Bear,
            Basket,
            Ground
        }

        public TypeCollides type;
    }
}