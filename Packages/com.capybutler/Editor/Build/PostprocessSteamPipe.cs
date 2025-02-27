using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace Capybutler.Editor.Build
{
    public class PostprocessSteamPipe : IPostprocessBuildWithReport
    {
        public int callbackOrder => 2;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (!EditorPrefs.GetBool(PathUtils.GetEditorKey("upload"), false)) {
                Debug.Log("[PostprocessSteamPipe] Skipping SteamPipe upload");
                return;
            }

            var processName = EditorPrefs.GetString(PathUtils.GetEditorKey("steamCmdPath"), "");
            if (!File.Exists(processName)) {
                Debug.LogError("[PostprocessSteamPipe] 'steamcmd.exe' not found");
                return;
            }

            if (!(EditorPrefs.HasKey(PathUtils.GetEditorKey("steamLogin")) && EditorPrefs.HasKey(PathUtils.GetEditorKey("steamPassword")))) {
                Debug.LogError("[PostprocessSteamPipe] Steam Login Information not found");
                return;
            }

            var steamLogin = EditorPrefs.GetString(PathUtils.GetEditorKey("steamLogin"));
            var steamPassword = EditorPrefs.GetString(PathUtils.GetEditorKey("steamPassword"));

            var buildScriptName = EditorPrefs.GetString(PathUtils.GetEditorKey("appIdScriptPath"), "");
            if (!File.Exists(buildScriptName)) {
                Debug.LogError($"[PostprocessSteamPipe] App build script '{buildScriptName}' not found");
                return;
            }

            var arguments = $"+login {steamLogin} \"{steamPassword}\" +run_app_build \"{buildScriptName}\" +quit";

            var steamPipe = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = EditorPrefs.GetString(PathUtils.GetEditorKey("steamCmdPath"), ""),
                    Arguments = arguments,
                    WorkingDirectory = Path.GetDirectoryName(processName) ?? "",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            var outputs = new List<string>();
            var errors = new List<string>();

            steamPipe.OutputDataReceived += (_, args) => outputs.Add(args.Data);
            steamPipe.ErrorDataReceived += (_, args) => errors.Add(args.Data);

            steamPipe.Start();
            steamPipe.BeginOutputReadLine();
            steamPipe.BeginErrorReadLine();
            steamPipe.WaitForExit();

            var outputLog = outputs.Count > 0 ? string.Join('\n', outputs) : "";
            var errorLog = errors.Count > 0 ? string.Join('\n', errors) : "";

            if (!string.IsNullOrWhiteSpace(outputLog))
                Debug.Log(outputLog);

            if (!string.IsNullOrWhiteSpace(errorLog))
                Debug.LogWarning(errorLog);

            Debug.Log("[PostprocessSteamPipe] Done");
        }
    }
}