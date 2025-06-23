using System;
using BepInEx;
using UnityEngine;
using GorillaBrainFuck.Types;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using System.Reflection;

namespace GorillaBrainFuck
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private string codeInput = string.Empty;
        private string lastOutput = string.Empty;
        private bool guiOpen = false;
        private GUILayoutOption heightOption;
        private GUILayoutOption expandWidthOption;
        private Rect guiArea = new Rect(10, 10, 400, 250);
        private bool isDragging = false;
        private Vector2 dragOffset = Vector2.zero;

        public void Start() => GorillaTagger.OnPlayerSpawned(Init);

        public void Init()
        {
            Debug.Log($"{PluginInfo.Name} initialized.");
            heightOption = GUILayout.Height(60);
            expandWidthOption = GUILayout.ExpandWidth(true);
        }

        public void Update()
        {
            if (Keyboard.current.f6Key.wasPressedThisFrame)
            {
                guiOpen = !guiOpen;
            }
        }

        public async Task Execute(bool res = true)
        {
            var input = new StringReader(codeInput);
            var output = new StringWriter();
            var interpreter = new Interpreter(input, output);

            if (res)
            {
                lastOutput = await interpreter.Execute(codeInput);
            }
            else
            {
                await interpreter.Execute(codeInput);
                lastOutput = output.ToString();
            }
        }

        public void OnGUI()
        {
            if (!guiOpen) return;

            Rect dragHandleRect = new Rect(guiArea.x, guiArea.y, guiArea.width, 20);
            GUI.Box(dragHandleRect, "");

            if (Event.current.type == EventType.MouseDown && dragHandleRect.Contains(Event.current.mousePosition))
            {
                isDragging = true;
                dragOffset = Event.current.mousePosition - new Vector2(guiArea.x, guiArea.y);
                Event.current.Use();
            }
            else if (Event.current.type == EventType.MouseUp && isDragging)
            {
                isDragging = false;
                Event.current.Use();
            }
            else if (Event.current.type == EventType.MouseDrag && isDragging)
            {
                guiArea.x = Mathf.Clamp(Event.current.mousePosition.x - dragOffset.x, 0, Screen.width - guiArea.width);
                guiArea.y = Mathf.Clamp(Event.current.mousePosition.y - dragOffset.y, 0, Screen.height - guiArea.height);
                Event.current.Use();
            }

            GUILayout.BeginArea(guiArea);
            GUILayout.Label("Gorilla BrainFuck Interpreter - Drag to move");
            codeInput = GUILayout.TextField(codeInput, heightOption, expandWidthOption);

            if (GUILayout.Button("Execute Code [Result]"))
            {
                _ = Execute();
            }

            if (GUILayout.Button("Execute Code [Output]"))
            {
                _ = Execute(false);
            }

            GUILayout.Label("Output:");
            GUILayout.TextArea(lastOutput, heightOption, expandWidthOption);

            if (GUILayout.Button("Close"))
            {
                guiOpen = false;
            }

            GUILayout.EndArea();
        }
    }

    public class PluginInfo
    {
        internal const string
            GUID = "Steve.GorillaBrainFuck",
            Name = "GorillaBrainFuck",
            Version = "1.0.0";
    }
}
