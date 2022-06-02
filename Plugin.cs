using BepInEx;
using System;
using UnityEngine;
using Utilla;
using UnityEngine.XR;
using System.IO;
using System.Reflection;

namespace Octavius
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        GameObject DocOck;

        GameObject _Sound;

        GameObject HandR;

        private readonly XRNode rNode = XRNode.RightHand;

        private readonly XRNode lNode = XRNode.RightHand;

        bool isgrip;

        bool cangrip = true;

        bool istrigger;

        bool cantrigger = true;

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("Octavius.Assets.octavius");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject DrOctopusObject = bundle.LoadAsset<GameObject>("doc ock");
            DocOck = Instantiate(DrOctopusObject);

            Stream _str = Assembly.GetExecutingAssembly().GetManifestResourceStream("Octavius.Assets.octaviustheme");
            AssetBundle _bundle = AssetBundle.LoadFromStream(_str);
            GameObject SoundObject = _bundle.LoadAsset<GameObject>("theme");
            _Sound = Instantiate(SoundObject);

            HandR = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/");
            DocOck.transform.SetParent(HandR.transform, false);
            DocOck.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            DocOck.transform.localRotation = Quaternion.Euler(351f, 86f, 359f);
            DocOck.transform.localPosition = new Vector3(-0.1f, 0.7f, -0.02f);
        }

        void Update()
        {
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out isgrip);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out istrigger);

            if (isgrip)
            {
                // This is so we cannot spam the noise
                if (cangrip)
                {
                    _Sound.GetComponent<AudioSource>().Play();
                    cangrip = false;
                }
            }
            else
            {
                // This is where grip is not pressed so here we will make it so u can grip
                cangrip = true;
            }

            if (istrigger)
            {
                if (cantrigger)
                {
                    _Sound.GetComponent<AudioSource>().Stop();
                }
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}