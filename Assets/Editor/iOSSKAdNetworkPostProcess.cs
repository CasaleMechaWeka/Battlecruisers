using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using System.IO;
using UnityEngine;

/// <summary>
/// Post-process build script that adds required SKAdNetwork IDs to iOS Info.plist
/// Required for iOS 14+ ad attribution tracking with AppLovin MAX, Unity Ads, and other ad networks
/// </summary>
public class iOSSKAdNetworkPostProcess : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 1; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.iOS)
        {
            string plistPath = Path.Combine(report.summary.outputPath, "Info.plist");
            
            if (!File.Exists(plistPath))
            {
                Debug.LogError($"[SKAdNetwork] Info.plist not found at {plistPath}");
                return;
            }

            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict rootDict = plist.root;

            // Create or get the SKAdNetworkItems array
            PlistElementArray skAdNetworkItems;
            if (rootDict.values.ContainsKey("SKAdNetworkItems"))
            {
                skAdNetworkItems = rootDict["SKAdNetworkItems"].AsArray();
                Debug.Log("[SKAdNetwork] Found existing SKAdNetworkItems array");
            }
            else
            {
                skAdNetworkItems = rootDict.CreateArray("SKAdNetworkItems");
                Debug.Log("[SKAdNetwork] Created new SKAdNetworkItems array");
            }

            // List of SKAdNetwork IDs required for AppLovin MAX, Unity Ads, and major ad networks
            // Updated as of 2024 - check monthly for updates
            string[] skAdNetworkIds = new string[]
            {
                // Unity Ads
                "4dzt52r2t5.skadnetwork",
                "bvpn9ufa9b.skadnetwork",
                
                // AppLovin MAX & mediation partners (includes legacy IronSource IDs for compatibility)
                "su67r6k2v3.skadnetwork", // IronSource
                "c6k4g5qg8m.skadnetwork", // IronSource
                "44jx6755aq.skadnetwork", // IronSource
                "6g9af3uyq4.skadnetwork", // IronSource
                "5lm9lj6jb7.skadnetwork", // IronSource
                "pwdxu55a5a.skadnetwork", // IronSource
                "yclnxrl5pm.skadnetwork", // IronSource
                "t38b2kh725.skadnetwork", // IronSource
                "7ug5zh24hu.skadnetwork", // IronSource
                "9rd848q2bz.skadnetwork", // IronSource
                "n6fk4nfna4.skadnetwork", // IronSource
                "kbd757ywx3.skadnetwork", // IronSource
                "9t245vhmpl.skadnetwork", // IronSource
                "4468km3ulz.skadnetwork", // IronSource
                "2u9pt9hc89.skadnetwork", // IronSource
                "8s468mfl3y.skadnetwork", // IronSource
                "av6w8kgt66.skadnetwork", // IronSource
                "klf5c3l5u5.skadnetwork", // IronSource
                "ppxm28t8ap.skadnetwork", // IronSource
                "424m5254lk.skadnetwork", // IronSource
                "uw77j35x4d.skadnetwork", // IronSource
                "578prtvx9j.skadnetwork", // IronSource
                "4dzt52r2t5.skadnetwork", // IronSource (shared with Unity)
                "e5fvkxwrpn.skadnetwork", // IronSource
                "8c4e2ghe7u.skadnetwork", // IronSource
                "zq492l623r.skadnetwork", // IronSource
                "3qcr597p9d.skadnetwork", // IronSource
                
                // Google AdMob
                "cstr6suwn9.skadnetwork",
                "4fzdc2evr5.skadnetwork",
                "4pfyvq9l8r.skadnetwork",
                "2fnua5tdw4.skadnetwork",
                "ydx93a7ass.skadnetwork",
                "5a6flpkh64.skadnetwork",
                "p78axxw29g.skadnetwork",
                "v72qych5uu.skadnetwork",
                "ludvb6z3bs.skadnetwork",
                "cp8zw746q7.skadnetwork",
                "3sh42y64q3.skadnetwork",
                "c6k4g5qg8m.skadnetwork",
                "s39g8k73mm.skadnetwork",
                "3qy4746246.skadnetwork",
                "f38h382jlk.skadnetwork",
                "hs6bdukanm.skadnetwork",
                "v4nxqhlyqp.skadnetwork",
                "wzmmz9fp6w.skadnetwork",
                "yclnxrl5pm.skadnetwork",
                "t38b2kh725.skadnetwork",
                "7ug5zh24hu.skadnetwork",
                "gta9lk7p23.skadnetwork",
                "vutu7akeur.skadnetwork",
                "y5ghdn5j9k.skadnetwork",
                "n6fk4nfna4.skadnetwork",
                "v9wttpbfk9.skadnetwork",
                "n38lu8286q.skadnetwork",
                "47vhws6wlr.skadnetwork",
                "kbd757ywx3.skadnetwork",
                "9t245vhmpl.skadnetwork",
                "eh6m2bh4zr.skadnetwork",
                "a2p9lx4jpn.skadnetwork",
                "22mmun2rn5.skadnetwork",
                "4468km3ulz.skadnetwork",
                "2u9pt9hc89.skadnetwork",
                "8s468mfl3y.skadnetwork",
                "klf5c3l5u5.skadnetwork",
                "ppxm28t8ap.skadnetwork",
                "ecpz2srf59.skadnetwork",
                "uw77j35x4d.skadnetwork",
                "pwa8g5ru9c.skadnetwork",
                "mlmmfzh3r3.skadnetwork",
                "578prtvx9j.skadnetwork",
                "4dzt52r2t5.skadnetwork",
                "e5fvkxwrpn.skadnetwork",
                "8c4e2ghe7u.skadnetwork",
                "zq492l623r.skadnetwork",
                "3qcr597p9d.skadnetwork",
                "3rd42ekr43.skadnetwork",
                "3qy4746246.skadnetwork",
                "av6w8kgt66.skadnetwork",
                "u679fj5vs4.skadnetwork",
                "rx5hdcabgc.skadnetwork",
                "9nlqeag3gk.skadnetwork",
                "275upjj5gd.skadnetwork",
                "wg4vff78zm.skadnetwork",
                "qqp299437r.skadnetwork",
                "mls7yz5dvl.skadnetwork",
                "k674qkevps.skadnetwork",
                "g28c52eehv.skadnetwork",
                "2tdux39lx8.skadnetwork",
                "52fl2v3hgk.skadnetwork",
                "r45fhb6rf7.skadnetwork",
                "n9x2a789qt.skadnetwork",
                "m8dbw4sv7c.skadnetwork",
                "7953jerfzd.skadnetwork",
                "glqzh8vgby.skadnetwork",
                "mtkv5xtk9e.skadnetwork",
                "rvh3l7un93.skadnetwork",
                "252b5q8x7y.skadnetwork",
                "w9q455wk68.skadnetwork",
                "lr83yxwka7.skadnetwork",
                "feyaarzu9v.skadnetwork",
                "s69wq72ugq.skadnetwork",
                "cj5566h2ga.skadnetwork",
                "zmvfpc5aq8.skadnetwork",
                "ggvn48r87g.skadnetwork",
                "523jb4fst2.skadnetwork",
                "294l99pt4k.skadnetwork",
                "24t9a8vw3c.skadnetwork",
                "mj797d8u6f.skadnetwork",
                "5tjdwbrq8w.skadnetwork",
                "nzq8sh4pbs.skadnetwork",
                "cg4yq2srnc.skadnetwork",
                "x8jxxk4ff5.skadnetwork",
                "x8uqf25wch.skadnetwork",
                "32z4fx6l9h.skadnetwork",
                "v79kvwwj4g.skadnetwork",
                "tl55sbb4fm.skadnetwork",
                "6xzpu9s2p8.skadnetwork",
                "yrqqpx2mcb.skadnetwork",
                "5l3tpt7t6e.skadnetwork",
                "3l6bd9hu43.skadnetwork",
                "737z793b9f.skadnetwork",
                "prcb7njmu6.skadnetwork",
                "44n7hlldy6.skadnetwork",
                "488r3q3dtq.skadnetwork",
                "a7xqa6mtl2.skadnetwork",
                "97r2b46745.skadnetwork",
                "8m87ys6875.skadnetwork",
                "9yg77x724h.skadnetwork",
                "g2y4y55b64.skadnetwork",
                "x2jnk7ly8j.skadnetwork",
                "x5l83yy675.skadnetwork",
                "6p4ks3rnbw.skadnetwork",
                "7rz58n8ntl.skadnetwork",
                "9g2aggbj52.skadnetwork",
                "hdw39hrw9y.skadnetwork",
                "xy9t38ct57.skadnetwork",
                "54nzkqm89y.skadnetwork",
                "f73kdq92p3.skadnetwork",
                "bxvub5ada5.skadnetwork",
                "vcra2ehyfk.skadnetwork",
                "4w7y6s5ca2.skadnetwork",
                "6964rsfnh4.skadnetwork",
                "79pbpufp78.skadnetwork",
                "9b89h5y424.skadnetwork",
                "gvmwg8q7h5.skadnetwork",
                "5mv394q32t.skadnetwork",
                "6v7lgmsu45.skadnetwork",
                "pu4na253f3.skadnetwork",
                "6yxyv74ff7.skadnetwork",
                "74b6s63p6l.skadnetwork",
                "v4na6bb69m.skadnetwork",
                "mp6xlyr22a.skadnetwork",
                "mqn7fxpca7.skadnetwork",
                "3l6hd8gxq7.skadnetwork",
                "424m5254lk.skadnetwork",
                "kbmxgpxpgc.skadnetwork",
                "dzg6xy7pwj.skadnetwork",
                "su67r6k2v3.skadnetwork",
                "6g9af3uyq4.skadnetwork",
                "5lm9lj6jb7.skadnetwork",
                "pwdxu55a5a.skadnetwork",
                "9rd848q2bz.skadnetwork",
                "248o8xwuzb.skadnetwork",
                "fz2k2k5tej.skadnetwork",
                "z4gj7hsk7h.skadnetwork",
                "qqq3s2jzvx.skadnetwork",
                "axh5283zss.skadnetwork",
                "9vvzujtq5s.skadnetwork",
                "k6y4y55b64.skadnetwork",
                "z24wtl6j62.skadnetwork",
                "294l3s3j7e.skadnetwork",
                "hb56zgv37p.skadnetwork",
                "c3frkrj4fj.skadnetwork",
                "f7s53z58qe.skadnetwork",
                "3l6bd9hu43.skadnetwork",
                "99hpqd4ggq.skadnetwork",
                "6r8vrj2v8p.skadnetwork"
            };

            // Track which IDs were added
            int addedCount = 0;
            int skippedCount = 0;

            foreach (string skAdNetworkId in skAdNetworkIds)
            {
                // Check if this ID already exists
                bool exists = false;
                foreach (PlistElement item in skAdNetworkItems.values)
                {
                    PlistElementDict itemDict = item.AsDict();
                    if (itemDict.values.ContainsKey("SKAdNetworkIdentifier"))
                    {
                        string existingId = itemDict["SKAdNetworkIdentifier"].AsString();
                        if (existingId == skAdNetworkId)
                        {
                            exists = true;
                            skippedCount++;
                            break;
                        }
                    }
                }

                // Add if it doesn't exist
                if (!exists)
                {
                    PlistElementDict skAdNetworkDict = skAdNetworkItems.AddDict();
                    skAdNetworkDict.SetString("SKAdNetworkIdentifier", skAdNetworkId);
                    addedCount++;
                }
            }

            // Write the modified plist back to file
            plist.WriteToFile(plistPath);

            Debug.Log($"[SKAdNetwork] ✅ Post-process complete!");
            Debug.Log($"[SKAdNetwork] Added {addedCount} new SKAdNetwork IDs");
            Debug.Log($"[SKAdNetwork] Skipped {skippedCount} existing IDs");
            Debug.Log($"[SKAdNetwork] Total SKAdNetwork IDs in Info.plist: {skAdNetworkItems.values.Count}");
            Debug.Log($"[SKAdNetwork] Info.plist updated at: {plistPath}");
        }
    }
}

