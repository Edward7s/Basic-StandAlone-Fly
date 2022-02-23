using System.Collections;
using MelonLoader;
using UnityEngine;

namespace Fly
{
    class Main : MelonMod
    {
        private bool flytoggle = false;

        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(waitforui());
        }
     
        private Transform camera()
        {
            return GameObject.Find("Camera (eye)").transform; 
        }
        private IEnumerator waitforui()
        {
            MelonLogger.Msg("Waiting For Ui");
            while (GameObject.Find("UserInterface") == null)
                yield return null;

            while (GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)") == null)
                yield return null;

            MelonLogger.Msg("Ui loaded");

            var toinst = GameObject.Find("/UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Right/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Emotes");
            var inst = GameObject.Instantiate(toinst, toinst.parent).gameObject;
            var txt = inst.transform.Find("Container/Text_QM_H3").GetComponent<TMPro.TextMeshProUGUI>();
            txt.richText = true;
            txt.text = $"<color=#000080ff>Fly</color>";
            GameObject.DestroyImmediate(inst.transform.Find("Container/Icon").gameObject);
            var btn = inst.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(new System.Action(() => { flytoggle = !flytoggle; _ = flytoggle ? txt.text = $"<color=#ff0000ff>Fly</color>" : txt.text = $"<color=#000080ff>Fly</color>"; VRC.Player.prop_Player_0.gameObject.GetComponent<CharacterController>().enabled = !flytoggle; }));

            

        }
        
        public override void OnUpdate()
        {
            if (!flytoggle) return;

            if (VRC.Player.prop_Player_0 == null) return;

                float flyspeed = Input.GetKey(KeyCode.LeftShift) ? Time.deltaTime * 50 : Time.deltaTime * 25;
                if (VRC.Player.prop_Player_0.field_Private_VRCPlayerApi_0.IsUserInVR())
                {
                    if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") < 0f)
                        VRC.Player.prop_Player_0.transform.position += camera().up * flyspeed;
                    if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") > 0f)
                    VRC.Player.prop_Player_0.transform.position -= camera().up * flyspeed;

                    if (Input.GetAxis("Vertical") != 0f)
                        VRC.Player.prop_Player_0.transform.position += camera().forward * (flyspeed * Input.GetAxis("Vertical"));

                    if (Input.GetAxis("Horizontal") != 0f)
                    VRC.Player.prop_Player_0.transform.position += camera().transform.right * (flyspeed * Input.GetAxis("Horizontal"));
                }
                else
                {
                if (Input.GetKey(KeyCode.W))
                    VRC.Player.prop_Player_0.transform.position += camera().forward * flyspeed;

                if (Input.GetKey(KeyCode.S))
                    VRC.Player.prop_Player_0.transform.position -= camera().forward * flyspeed;

                if (Input.GetKey(KeyCode.A))
                    VRC.Player.prop_Player_0.transform.position -= camera().right * (flyspeed / 2);

                if (Input.GetKey(KeyCode.D))
                    VRC.Player.prop_Player_0.transform.position += camera().right * (flyspeed / 2);

                if (Input.GetKey(KeyCode.LeftControl))
                    VRC.Player.prop_Player_0.transform.position -= camera().up * (flyspeed / 2);

                if (Input.GetKey(KeyCode.Space))
                    VRC.Player.prop_Player_0.transform.position += camera().up * (flyspeed / 2);
            }


        }

    }
}
