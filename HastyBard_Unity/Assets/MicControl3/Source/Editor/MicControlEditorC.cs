using System.Collections;
using UnityEditor;
using UnityEngine;

namespace AS
{
    namespace MicControl
    {

        [CustomEditor (typeof (MicControlC))]
        public class MicControlEditorC : Editor
        {

            Texture logoInspector;
            void LoadTex ()
            {
                if (logoInspector == null)
                {
                    logoInspector = Resources.Load ("MicControlGizmo", typeof (Texture)) as Texture;

                }
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////		
            public override void OnInspectorGUI ()
            {
                //draw logo
                if (logoInspector == null)
                {
                    LoadTex ();
                }
                EditorGUILayout.BeginHorizontal ();
                GUILayout.Label ("");
                GUILayout.Box (logoInspector, GUILayout.Width (400), GUILayout.Height (100));
                GUILayout.Label ("");
                EditorGUILayout.EndHorizontal ();

                //draw inspector.

                GameObject ListenToMic = Selection.activeGameObject;
                MicControlC microphone = ListenToMic.GetComponent<MicControlC> ();
                float micInputValue = microphone.loudness;

                //this button copy's the basic code to call the value, for quick acces.
                //use horizontal mapping incase we add more menu buttons later.
                EditorGUILayout.BeginHorizontal ();

                //help button redirects to website
                if (GUILayout.Button (new GUIContent ("Help", "Need help? Check the FAQ or fill in a contact form.")))
                {
                    Application.OpenURL ("http://markduisters.com/asset-portfolio/");
                }
                //contact button redirects to the contact form on the website
                if (GUILayout.Button (new GUIContent ("Contact", "Have a question or found a bug? Let me know!")))
                {
                    Application.OpenURL ("http://markduisters.com/contact/");
                }

                EditorGUILayout.EndHorizontal ();

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //show the selected device and create a device selection drop down.

                //this visually shows if the micrpohone is active or not
                EditorGUILayout.BeginHorizontal ();

                try
                {
                    //if the application is focused (ingame) show a green box
                    if (microphone.focused)
                    {
                        GUI.color = Color.green;
                        GUILayout.Box ("        ");
                        GUI.color = Color.white;
                    }
                    else
                    {
                        GUI.color = Color.red;
                        GUILayout.Box ("        ");
                        GUI.color = Color.white;
                    }

                    //show selected device
                    GUILayout.Label (Microphone.devices[microphone.InputDevice]);
                }
                catch (System.Exception e)
                {
                    Debug.LogError ("No usable device detected, please enable a microphone on your system: " + e);
                }

                EditorGUILayout.EndHorizontal ();

                //Redirect pointer_ShowDeviceName ingame
                //count devices
                int count = 0;
                foreach (string device in Microphone.devices)
                {
                    count++;
                }

                //toggle if a default mic should be used
                microphone.useDefaultMic = GUILayout.Toggle (microphone.useDefaultMic, new GUIContent ("Use default device microphone", "When enabled the controller will always grab the default microphone of the device where the application is run from.(mobile mic, pc mic that is currently set as default,..."), GUILayout.Width (200));

                if (!microphone.useDefaultMic)
                {

                    // instead of using the buttons or default setting to select. Users can manually input a slot number (for android, etc..)
                    microphone.pointer_SetDeviceSlot = GUILayout.Toggle (microphone.pointer_SetDeviceSlot, new GUIContent ("Set device slot", "Handy for building to mobile platyforms that use another microphone other than the internal one. If you know the device location/number position, you can type it here. Keep in mind that although you can set not connected slots now.Your microphone input will not work in the editor. A good rule of thumb to follow is, to always develop in the editor with your workstation default microphone and only at build set the slot to what Android would be using. If your android device only uses the internal microphone, simply select use default device and you are good to go."), GUILayout.Width (200));
                    if (!microphone.pointer_SetDeviceSlot)
                    {

                        microphone.pointer_ShowDeviceName = EditorGUILayout.Foldout (microphone.pointer_ShowDeviceName, new GUIContent ("Detected devices: " + count, "Show a list of all detected devices (1 is showed as default device in the drop down menu)"));
                        if (microphone.pointer_ShowDeviceName)
                        {

                            if (Microphone.devices.Length >= 0)
                            {

                                int i = 0;
                                //count amount of devices connected
                                foreach (string device in Microphone.devices)
                                {
                                    if (device == null)
                                    {
                                        Debug.LogError ("No usable device detected! Try setting your device as the system's default. Or set a slot manually");
                                        return;
                                    }
                                    i++;

                                    GUILayout.BeginVertical ();

                                    //if selected slot is not equal to number count, make button grey.
                                    if (microphone.InputDevice != i - 1)
                                    {
                                        GUI.color = Color.grey;
                                    }

                                    //create a selection button
                                    if (GUILayout.Button (device))
                                    {
                                        microphone.InputDevice = i - 1;
                                    }

                                    GUI.color = Color.white;

                                    GUILayout.EndVertical ();
                                }

                            }

                            //throw error when no device is found.
                            else
                            {
                                Debug.LogError ("No connected device detected! Connect at least one device.");
                                return;
                            }
                        }
                    }
                    else
                    {
                        microphone.InputDevice = EditorGUILayout.IntField ("Slot number =", microphone.InputDevice);

                    }

                }

                if (!microphone.focused)
                {
                    GUILayout.Label ("");
                    GUILayout.Label ("The microphone will only send data when the game window is active!");
                    GUILayout.Label ("");
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //show pointer_advanced variables
                microphone.pointer_advanced = EditorGUILayout.Foldout (microphone.pointer_advanced, new GUIContent ("Advanced settings", "Reveal all tweakable variables"));

                if (microphone.pointer_advanced)
                {

                    GUILayout.Label ("");

                    EditorGUILayout.BeginHorizontal ();
                    //	GUILayout.Label("");

                    //Redirect debug ingame
                    microphone.debug = GUILayout.Toggle (microphone.debug, new GUIContent ("Debug", "This will show the connection progress in the console."), GUILayout.Width (60));

                    //keep this controller persistend between scenes
                    microphone.doNotDestroyOnLoad = GUILayout.Toggle (microphone.doNotDestroyOnLoad, new GUIContent ("Don't destroy on load", "If selected, this controller will be persistend when switching scenes during runtime"), GUILayout.Width (150));

                    //Redirect Mute ingame
                    microphone.Mute = GUILayout.Toggle (microphone.Mute, new GUIContent ("Mute", "Leave enabled when you only need the input value of your device. When dissabled you can listen to the playback of the device"), GUILayout.Width (60));

                    EditorGUILayout.EndHorizontal ();

                    //enable or disable spectrum data analisys
                    microphone.enableSpectrumData = GUILayout.Toggle (microphone.enableSpectrumData, new GUIContent ("enable spectrum data analysis", "When enabled users will have acces to the full frequency spectrum output by MicControl2. This is the big brother of the 'loudness' variable, instead of having a single float to represent the microphone's loudness, a full float array is filled with the frequency spectrum data. "), GUILayout.Width (500));

                    GUILayout.Label ("");

                    //ListenToMic.GetComponent<MicControlC>().maxFreq = EditorGUILayout.FloatField(new GUIContent ("Frequency (Hz)","Set the quality of the received data: It is recommended but not required, to match this to your selected microphone's frequency."), ListenToMic.GetComponent<MicControlC>().maxFreq);
                    microphone.freq = (MicControlC.freqList) EditorGUILayout.EnumPopup (new GUIContent ("Frequency (Hz)", "Select the quality of the received data: It is recommended but not required, to match this to your selected microphone's frequency."), microphone.freq);

                    microphone.bufferTime = EditorGUILayout.IntField (new GUIContent ("Buffer time", "How many seconds of audio should be loaded into memory. This will then be filled up with the Sample amount."), microphone.bufferTime);
                    //always have at least 1 second of audio to fill the ram.
                    if (microphone.bufferTime < 1)
                    {
                        microphone.bufferTime = 1;
                    }

                    microphone.amountSamples = EditorGUILayout.IntSlider (new GUIContent ("Sample amount", "This is basically how much the buffer gets filled (per frame) with samples and determines the precission of the loudness variable, if you are not sure, leave this between 256 and 1024 as it gives more than enough precision for basic tasks/scripts. Higher samples = more precision/quality of the loudness value and smoother results. However for spectrumData this is different. As spectrumData gives you direct acces to this sample buffer. This means that the bigger the sample buffer, the more data you have acces to. "), microphone.amountSamples, 256, 8192);
                    //lock to increments
                    int tempSamples = microphone.amountSamples;
                    if (tempSamples >= 256 && tempSamples <= 512)
                    {
                        microphone.amountSamples = 256;
                    }

                    if (tempSamples >= 512 && tempSamples <= 1024)
                    {
                        microphone.amountSamples = 512;
                    }

                    if (tempSamples >= 1024 && tempSamples <= 2048)
                    {
                        microphone.amountSamples = 1024;
                    }

                    if (tempSamples >= 2048 && tempSamples <= 4096)
                    {
                        microphone.amountSamples = 2048;
                    }

                    if (tempSamples >= 4096 && tempSamples <= 8192)
                    {
                        microphone.amountSamples = 4096;
                    }

                    if (tempSamples >= 8192)
                    {
                        microphone.amountSamples = 8192;
                    }

                    microphone.sensitivity = EditorGUILayout.Slider (new GUIContent ("Sensitivity", "Set the sensitivity of your input: The higher the number, the more sensitive (higher) the -loudness- value will be"), microphone.sensitivity, microphone.minMaxSensitivity.x, microphone.minMaxSensitivity.y);
                    EditorGUILayout.MinMaxSlider (new GUIContent ("Sensitivity range", "Helps you tweak the sensitivity"), ref microphone.minMaxSensitivity.x, ref microphone.minMaxSensitivity.y, 0.0f, 1000.0f);

                    EditorGUILayout.BeginHorizontal ();
                    GUILayout.Label ("");
                    GUILayout.Label ("min: " + microphone.minMaxSensitivity.x, GUILayout.Width (100));
                    GUILayout.Label ("max: " + microphone.minMaxSensitivity.y, GUILayout.Width (100));
                    EditorGUILayout.EndHorizontal ();

                    //show loudness progress bars
                    GUILayout.Label ("" + microphone.rawInput, GUILayout.Width (100));
                    ProgressBar (microphone.rawInput, "Raw Input", 18, 20);
                    GUILayout.Label ("" + micInputValue, GUILayout.Width (100));
                    ProgressBar (micInputValue, "Loudness", 18, 20);
                    microphone.remapRange = GUILayout.Toggle (microphone.remapRange, new GUIContent ("Remap range", "You can remap the loudness and spectrum values to a custom range based on a min max value."));

                    if (microphone.remapRange)
                    {
                        EditorGUILayout.BeginHorizontal ();

                        microphone.minMaxRange = EditorGUILayout.Vector4Field (new GUIContent ("Old to new range", "xy= old range, zw= new range."), microphone.minMaxRange);

                        EditorGUILayout.EndHorizontal ();

                    }

                    //show spectrum data progress bars
                    if (microphone.enableSpectrumData)
                    {
                        GUILayout.Label ("SpectrumData/Curve");
                        EditorGUILayout.CurveField (new GUIContent ("", "Curve visualization of the spectrumData[]array. This curve can also be sampled through the new spectrumCurve property."), microphone.spectrumCurve);

                    }

                }

            }

            // Custom GUILayout progress bar.
            void ProgressBar (float value, string label, int scaleX, int scaleY)
            {

                // Get a rect for the progress bar using the same margins as a textfield:
                Rect rect = GUILayoutUtility.GetRect (scaleX, scaleY, "TextField");
                EditorGUI.ProgressBar (rect, value, label);

                EditorGUILayout.Space ();
            }

        }
    }
}