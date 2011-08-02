#region licence
/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NPanday.Model;
using NPanday.Model.Settings;
#endregion

namespace NPanday.Model.Settings
{
    #region SettingsUtil

    // An XmlTextReader that overrides the namespaces to always be empty, effectively ignoring them when deserializing
    // This avoids inconsistencies in Maven settings.xml files that sometimes include the namespace declaration and
    // sometimes omit it
    // See http://stackoverflow.com/questions/870293/can-i-make-xmlserializer-ignore-the-namespace-on-deserialization
    public class NamespaceIgnorantXmlTextReader : XmlTextReader
    {
        public NamespaceIgnorantXmlTextReader(System.IO.TextReader reader) : base(reader) { }

        public override string NamespaceURI
        {
            get { return ""; }
        }
    }

    /// <summary>
    /// TODO: finish merging settingsUtil with Settings
    /// </summary>
    public static class SettingsUtil
    {
        #region GetDefaultSettingsPath()
        /// <summary>
        /// Gets the default settings path.
        /// </summary>
        /// <returns>
        /// The default settings path.
        /// </returns>
        internal static string GetDefaultSettingsPath()
        {
            string m2Dir = Environment.GetEnvironmentVariable("M2_HOME");

            if ((!string.IsNullOrEmpty(m2Dir)) && Directory.Exists(m2Dir))
            {
                string confDir = m2Dir + @"\conf";

                if (Directory.Exists(confDir))
                {
                    string settingsPath = confDir + @"\settings.xml";

                    if (File.Exists(settingsPath))
                    {
                        return settingsPath;
                    }
                }
            }

            return null;
        } 
        #endregion

        #region GetUserSettingsPath()
        /// <summary>
        /// Gets the user settings path.
        /// </summary>
        /// <returns>
        /// The user settings path.
        /// </returns>
        public static string GetUserSettingsPath()
        {
            string profileDir = Environment.GetEnvironmentVariable("USERPROFILE");

            if ((!string.IsNullOrEmpty(profileDir)) && Directory.Exists(profileDir))
            {
                string m2Dir = profileDir + @"\.m2";

                if (Directory.Exists(m2Dir))
                {
                    string settingsPath = m2Dir + @"\settings.xml";

                    if (File.Exists(settingsPath))
                    {
                        return settingsPath;
                    }
                }
            }

            return null;
        }
        #endregion

        #region GetLocalRepositoryPath()
        /// <summary>
        /// Gets the local repository path.
        /// </summary>
        /// <returns>
        /// The local repository path
        /// </returns>
        public static string GetLocalRepositoryPath()
        {
            string path = null;
            string userSettingsPath = GetUserSettingsPath();

            if (!string.IsNullOrEmpty(userSettingsPath))
            {
                try
                {
                    Settings settings = Settings.Read(userSettingsPath);

                    if (settings != null)
                    {
                        path = settings.localRepository;
                    }
                }
                catch
                {
                }

                if (!string.IsNullOrEmpty(path))
                {
                    return path;
                } 
            }

            string defaultSettingsPath = GetDefaultSettingsPath();

            if (!string.IsNullOrEmpty(defaultSettingsPath))
            {
                try
                {
                    Settings settings = Settings.Read(defaultSettingsPath);

                    if (settings != null)
                    {
                        path = settings.localRepository;
                    }
                }
                catch
                {
                }

                if (!string.IsNullOrEmpty(path))
                {
                    return path;
                } 
            }

            string profileDir = Environment.GetEnvironmentVariable("USERPROFILE");

            return profileDir + @"\.m2\repository";
        } 
        #endregion

   


        #region GetProfile(Settings, string)
        ///// <summary>
        ///// Get profile
        ///// </summary>
        ///// <param name="settings">The settings info</param>
        ///// <param name="profileId">The profile id</param>
        ///// <returns>The profile from the settings</returns>
        //public static Profile GetProfile(Settings settings, string profileId)
        //{
        //    if (settings.Profiles != null)
        //    {
        //        foreach (Profile profile in settings.Profiles)
        //        {
        //            if (profileId.Equals(profile.id))
        //            {
        //                return profile;
        //            }
        //        }
        //    }
        //    return null;
        //}
        #endregion

      

       
    #endregion
        //#region GetRepositoryFromProfile(Profile, string)
        ///// <summary>
        ///// Gets the repository object from the profile
        ///// </summary>
        ///// <param name="profile">The profile</param>
        ///// <param name="url">The repository url</param>
        ///// <returns>
        ///// The repository object from the profile
        ///// </returns>
        //public static Repository GetRepositoryFromProfile(Profile profile, string url)
        //{
        //    if (profile.Repositories != null)
        //    {
        //        foreach (Repository repo in profile.Repositories)
        //        {
        //            if (url.Equals(repo.url))
        //            {
        //                return repo;
        //            }
        //        }
        //    }
        //    return null;
        //}
        //#endregion

        //#region AddRepositoryToProfile(Profile, string, bool, bool, Settings)
        ///// <summary>
        ///// Add repository to profile
        ///// </summary>
        ///// <param name="profile">The profile</param>
        ///// <param name="url">The repository url to be added</param>
        ///// <param name="isReleaseEnabled">True if release is enabled</param>
        ///// <param name="isSnapshotEnabled">True if snapshot is enabled</param>
        ///// <param name="settings">The settings</param>
        ///// <returns>The repository</returns>
        //public static Repository AddRepositoryToProfile(Profile profile, string url, bool isReleaseEnabled, bool isSnapshotEnabled, Settings settings)
        //{
        //    Repository repository = GetRepositoryFromProfile(profile, url);

        //    if (repository == null)
        //    {
        //        repository = new Repository();
        //        repository.url = url;
        //        UpdateRepository(profile, repository, isReleaseEnabled, isSnapshotEnabled);

        //        // add repository to profile
        //        if (profile.Repositories == null)
        //        {
        //            profile.setRepositories = new Repository[] { repository };
        //        }
        //        else
        //        {
        //            List<Repository> repositories = new List<Repository>();
        //            repositories.AddRange(profile.Repositories);
        //            repositories.Insert(0, repository);
        //            profile.setRepositories = repositories.ToArray();
        //        }

        //        if (settings.Profiles == null)
        //        {
        //            settings.Profiles = new Profile[] { profile };
        //        }
        //        else if (GetProfile(settings, profile.id) == null)
        //        {
        //            List<Profile> profiles = new List<Profile>();
        //            profiles.AddRange(settings.Profiles);
        //            profiles.Add(profile);
        //            settings.Profiles = profiles.ToArray();
        //        }
        //    }
        //    return repository;
        //}
        //#endregion

        //#region RemoveRepositoryFromProfile
        ///// <summary>
        ///// Remove repository from profile
        ///// </summary>
        ///// <param name="profile">The profile</param>
        ///// <param name="repository">The repository to remove</param>
        //public static void RemoveRepositoryFromProfile(Profile profile, Repository repository)
        //{
        //    List<Repository> repositories = new List<Repository>();
        //    repositories.AddRange(profile.Repositories);
        //    repositories.Remove(repository);
        //    profile.setRepositories = repositories.ToArray();
        //}
      //  #endregion

        

        //private static string generateRepositoryId(Profile profile)
        //{
        //    int ctr = 0;
        //    if (profile.Repositories != null)
        //    {
        //        foreach (Repository repo in profile.Repositories)
        //        {
        //            if (repo.id != null && repo.id.StartsWith("npanday.repo."))
        //            {
        //                int index = int.Parse(repo.id.Substring(13));

        //                if (index >= ctr)
        //                {
        //                    ctr = index + 1;
        //                }
        //            }
        //        }
        //    }
        //    return "npanday.repo." + ctr;
        //}
    }

  
}
