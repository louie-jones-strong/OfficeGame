using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchNotes
{
    public static bool HasUpdated { get { return LastBootVersion != CurrentVersion; } }
    public static string LastBootVersion { private set; get; }
    public static string CurrentVersion { get { return Application.version; } }

    const string LastVersionKey = "LastBootVersionKey";

    public static void GameBooted()
    {
        LastBootVersion = PlayerPrefsHelper.GetString(LastVersionKey);
        PlayerPrefsHelper.SetString(LastVersionKey, CurrentVersion);
    }

    public static string BuildPatchNotes(string startVersion = "")
    {
        var patchNotes = "";
        var version = startVersion;

        while (version != CurrentVersion)
        {
            version = VersionChangeNotes(version, ref patchNotes);
        }
        return patchNotes;
    }

    static string VersionChangeNotes(string version, ref string notes)
    {
        var newVersion = "";
        var versionName = "";
        var versionNotes = "";

        var majorTitleText = "<color=orange><size=30><b>Major updates:</b></size></color>\n";
        var minorTitleText = "<color=orange><size=30><b>Minor updates:</b></size></color>\n";
        var fixesTitleText = "<color=orange><size=30><b>Fixes:</b></size></color>\n";

        switch (version)
        {
            case ""://[build 1]
            {
                newVersion = "1";
                versionName = "The Game Release";
                versionNotes = "\n";
                versionNotes += "The original release.\n";
                break;
            }
            case "1"://[build 1.1]
            {
                newVersion = "1.1";
                versionName = "Second Level";
                versionNotes = "\n";
                // ------------------------------------------------------------------
                versionNotes += "\n";
                versionNotes += majorTitleText;
                versionNotes += " - Added patch notes.\n";
                versionNotes += " - The time is shown to higher precision for speed runners\n";
                // ------------------------------------------------------------------
                versionNotes += "\n";
                versionNotes += minorTitleText;
                // Map minors
                versionNotes += " - Added inverted mouse toggle\n";
                versionNotes += " - Added version number to menu\n";
                // ------------------------------------------------------------------
                versionNotes += "\n";
                versionNotes += fixesTitleText;
                versionNotes += " - Fixed glass walls not being clear\n";
                break;
            }
            default:
            {
                Logger.LogError($"No Version Change Notes for Version: \"{version}\" to version \"{CurrentVersion}\"");
                newVersion = CurrentVersion;
                versionName = "<color=magenta>A future patch</color>";
                versionNotes = "<color=magenta>Coming \"soon\"</color>\n";
                break;
            }
        }

        var newNotes = $"<color=yellow><size=40><b>[{newVersion}] {versionName}</b></size></color>\n";
        newNotes += versionNotes;
        newNotes += "\n";
        newNotes += "\n";
        newNotes += notes;

        notes = newNotes;
        return newVersion;
    }
}