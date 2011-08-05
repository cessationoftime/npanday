#region Apache License, Version 2.0
//
// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.
//
#endregion

using System;
using System.IO;
using NPanday.Model.Settings;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;

namespace NPanday.Artifact
{
    public sealed partial class Artifact
    {

        public bool Download(NPanday.Logging.Logger logger)
        {
            Application.DoEvents();
            return downloadArtifactFromRemoteRepository(logger);
        }


        bool downloadArtifactFromRemoteRepository(NPanday.Logging.Logger logger)
        {
            try
            {
                Settings settings = Settings.Read(SettingsPath.UserSettings);
                if (settings == null || settings.Profiles == null)
                {
                    MessageBox.Show("Cannot add reference of " + this.ArtifactId + ", no valid Remote Repository was found that contained the Artifact to be Resolved. Please add a Remote Repository that contains the Unresolved Artifact.");
                    return false;
                }


                Dictionary<string, string> mirrors = new Dictionary<string, string>();

                if (settings.mirrors != null)
                {
                    foreach (Mirror mirror in settings.mirrors)
                    {
                        string id = mirror.mirrorOf;
                        if (id.StartsWith("external:*"))
                        {
                            id = "*";
                        }
                        // TODO: support '!' syntax
                        mirrors.Add(id, mirror.url);
                    }
                }

                List<Repository> repos;

                if (this.isSnapshot)
                {
                    repos = settings.ActiveProfiles.Repositories.SnapshotRepositoriesOnly;
                }
                else
                {
                    repos = settings.ActiveProfiles.Repositories.ReleaseRepositoriesOnly;

                    // ensure there is at least one repo defined
                    // TODO: this is just a temporary implementation
                    if (repos.Count == 0)
                    {
                        Repository central = new Repository();
                        central.id = "central";
                        central.url = "http://repo1.maven.org/maven2";
                        central.snapshots = new RepositoryPolicy();
                        central.releases = new RepositoryPolicy();
                        central.snapshots.enabled = false;
                        central.releases.enabled = true;
                        repos.Add(central);
                    }
                }
                // TODO: sustain correct ordering from settings.xml
                foreach (Repository repo in repos)
                {
                    string url = repo.url;
                    if (mirrors.ContainsKey(repo.id))
                    {
                        url = mirrors[repo.id];
                    }
                    if (mirrors.ContainsKey("*"))
                    {
                        url = mirrors["*"];
                    }

                    ArtifactContext artifactContext = new ArtifactContext();

                    if (this.isSnapshot)
                    {
                        string newVersion = GetSnapshotVersion(url, logger);

                        if (newVersion != null)
                        {
                            this.RemotePath = artifactContext.GetArtifactRepository().GetRemoteRepositoryPath(this, this.Version.Replace("SNAPSHOT", newVersion), url, this.FileInfo.Extension);
                        }

                        else
                        {
                            this.RemotePath = artifactContext.GetArtifactRepository().GetRemoteRepositoryPath(this, url, this.FileInfo.Extension);
                        }

                    }
                    else
                    {
                        this.RemotePath = artifactContext.GetArtifactRepository().GetRemoteRepositoryPath(this, url, this.FileInfo.Extension);
                    }

                    if (downloadArtifact(logger))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot add reference of " + this.ArtifactId + ", an exception occurred trying to download it: " + e.Message);
                return false;
            }
        }


        private string GetSnapshotVersion(string repo, NPanday.Logging.Logger logger)
        {
            WebClient client = new WebClient();
            string timeStampVersion = null;
            string metadataPath = repo + "/" + this.GroupId.Replace('.', '/') + "/" + this.ArtifactId;
            string snapshot = "<snapshot>";
            string metadata = "/maven-metadata.xml";

            try
            {
                metadataPath = metadataPath + "/" + this.Version + metadata;

                string content = client.DownloadString(metadataPath);
                string[] lines = content.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                string timeStamp = null;
                string buildNumber = null;

                foreach (string line in lines)
                {
                    int startIndex;
                    int len;

                    if (line.Contains("<timestamp>"))
                    {
                        startIndex = line.IndexOf("<timestamp>") + "<timestamp>".Length;
                        len = line.IndexOf("</timestamp>") - startIndex;

                        timeStamp = line.Substring(startIndex, len);
                    }

                    if (line.Contains("<buildNumber>"))
                    {
                        startIndex = line.IndexOf("<buildNumber>") + "<buildNumber>".Length;
                        len = line.IndexOf("</buildNumber>") - startIndex;

                        buildNumber = line.Substring(startIndex, len);
                    }
                }

                if (timeStamp == null)
                {
                    logger.Log(NPanday.Logging.Level.WARNING, "Timestamp was not specified in maven-metadata.xml - using default snapshot version");
                    return null;
                }

                if (buildNumber == null)
                {
                    logger.Log(NPanday.Logging.Level.WARNING, "Build number was not specified in maven-metadata.xml - using default snapshot version");
                    return null;
                }

                timeStampVersion = timeStamp + "-" + buildNumber;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                client.Dispose();
            }

            return timeStampVersion;
        }

        bool downloadArtifact(NPanday.Logging.Logger logger)
        {
            WebClient client = new WebClient();
            bool dirCreated = false;

            try
            {
                if (!this.FileInfo.Directory.Exists)
                {
                    this.FileInfo.Directory.Create();
                    dirCreated = true;
                }


                logger.Log(NPanday.Logging.Level.INFO, string.Format("Download Start: {0} Downloading From {1}\n", DateTime.Now, this.RemotePath));

                client.DownloadFile(this.RemotePath, this.FileInfo.FullName);

                logger.Log(NPanday.Logging.Level.INFO, string.Format("Download Finished: {0}\n", DateTime.Now));

                string artifactDir = GetLocalUacPath();

                if (!Directory.Exists(Path.GetDirectoryName(artifactDir)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(artifactDir));
                }
                if (!File.Exists(artifactDir))
                {
                    File.Copy(this.FileInfo.FullName, artifactDir);
                }

                return true;

            }

            catch (Exception e)
            {
                if (dirCreated)
                {
                    this.FileInfo.Directory.Delete();
                }

                logger.Log(NPanday.Logging.Level.WARNING, string.Format("Download Failed {0}\n", e.Message));

                return false;
            }

            finally
            {
                client.Dispose();
            }
        }


        public string GetLocalUacPath()
        {
            return Path.Combine(SettingsUtil.GetLocalRepositoryPath(), string.Format(@"{0}\{1}\{1}{2}-{3}", Tokenize(this.GroupId), this.ArtifactId, this.Version, this.FileInfo.Extension));
        }
        private string Tokenize(string id)
        {
            return id.Replace(".", Path.DirectorySeparatorChar.ToString());
        }    
    }     
}
