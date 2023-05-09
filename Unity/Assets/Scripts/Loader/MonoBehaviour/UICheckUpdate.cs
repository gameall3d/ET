using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UICheckUpdate : MonoBehaviour
    {
        public Slider DownloadProgress;

        public Text Msg;

        private static UICheckUpdate _instance;
        public static UICheckUpdate Instance
        {
            get
            {
                if (_instance == null)
                {
                    var uiPrefab = Resources.Load<GameObject>("UICheckUpdate");
                    GameObject gameObject = Instantiate<GameObject>(uiPrefab);
                    _instance = gameObject.GetComponent<UICheckUpdate>();
                }

                return _instance;
            }
        }
        
        // Start is called before the first frame update
        void Start()
        {
            SetMessage("");
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetMessage(string text)
        {
            this.Msg.text = text;
        }

        public void Remove()
        {
            Destroy(this.gameObject);
        }
    }
}
