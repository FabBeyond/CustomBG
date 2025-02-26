using MelonLoader;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using System.Diagnostics;

[assembly: MelonInfo(typeof(CustomBG.Core), "CustomBG", "1.0.0", "fabbeyond", null)]
[assembly: MelonGame("TraipseWare", "Peaks of Yore")]

namespace CustomBG
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonPreferences.CreateCategory("CBG_ImageFilePath", "CustomBG");

            MelonPreferences.CreateEntry("CBG_ImageFilePath", "Path", "Default");

            base.OnInitializeMelon();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            GameObject bgImage = GameObject.Find("bg_vista");

            Texture2D bgImgNew = LoadFromFile(MelonPreferences.GetEntryValue<string>("CBG_ImageFilePath", "Path"));
            Sprite bgSprite = Sprite.Create(bgImgNew, new Rect(0, 0, bgImgNew.width, bgImgNew.height), new Vector2(0.5f, 0.5f));

            bgImage.GetComponent<Image>().sprite = bgSprite;


            base.OnSceneWasLoaded(buildIndex, sceneName);
        }

        public Texture2D LoadFromFile(string path)
        {
            if (!File.Exists(path)) return null;

            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            return texture;
        }
    }

    [HarmonyPatch(typeof(GameObject), "SetActive")]
    class Patch_SetActive
    {
        [HarmonyPostfix]
        static void Postfix(GameObject __instance, bool value, StackTrace __state)
        {
            if (value)
            {
                if (__instance.name == "bg_vista_alps") __instance.SetActive(false);

            }
        }
    }
}