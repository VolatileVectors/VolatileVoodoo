using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace VolatileVoodooBuild.Editor
{
    public class PostprocessSteamPipe : IPostprocessBuildWithReport
    {
        public int callbackOrder => 2;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (!EditorPrefs.GetBool(BuildTool.GetEditorKeyPrefix + "upload", false)) {
                Debug.Log("[PostprocessSteamPipe] Skipping SteamPipe upload");
                return;
            }

            var processName = EditorPrefs.GetString(BuildTool.GetEditorKeyPrefix + "steamCmdPath", "");
            if (!File.Exists(processName)) {
                Debug.LogError("[PostprocessSteamPipe] 'steamcmd.exe' not found");
                return;
            }

            if (!(EditorPrefs.HasKey(BuildTool.GetEditorKeyPrefix + "steamLogin") &&
                  EditorPrefs.HasKey(BuildTool.GetEditorKeyPrefix + "steamPassword"))) {
                Debug.LogError("[PostprocessSteamPipe] Steam Login Information not found");
                return;
            }

            var steamLogin = EditorPrefs.GetString(BuildTool.GetEditorKeyPrefix + "steamLogin");
            var steamPassword = EditorPrefs.GetString(BuildTool.GetEditorKeyPrefix + "steamPassword");

            var buildScriptName = EditorPrefs.GetString(BuildTool.GetEditorKeyPrefix + "appIdScriptPath", "");
            if (!File.Exists(buildScriptName)) {
                Debug.LogError($"[PostprocessSteamPipe] App build script '{buildScriptName}' not found");
                return;
            }

            var arguments = $"+login {steamLogin} \"{steamPassword}\" +run_app_build \"{buildScriptName}\" +quit";

            var steamPipe = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = EditorPrefs.GetString(BuildTool.GetEditorKeyPrefix + "steamCmdPath", ""),
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