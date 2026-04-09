using UnityEngine;

namespace Player
{
    public class PlayerSystem : MonoBehaviour
    {
        protected PlayerMain Main;
        
        protected virtual void Awake()
        {
            Main = transform.root.GetComponent<PlayerMain>();
        }
    
    }
}
