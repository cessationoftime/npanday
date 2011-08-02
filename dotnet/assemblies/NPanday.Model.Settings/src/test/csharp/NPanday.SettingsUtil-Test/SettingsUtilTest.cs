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

using NUnit.Framework;
using NPanday.Model.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NPanday.Model;
namespace NPanday.SettingsUtil_Test
{
    [TestFixture]
    public class SettingsUtilTest
    {
        private string defaultProfileId = "npanday.test.id";
        private string repoUrl1 = "http://repo1.maven.org/maven2";
        private string repoUrl2 = "http://repo.exist.com/maven2";
        private string settingsPath;
        private Settings settings;

        [Test]
        public void AddRepositoryToEmptyProfileTest()
        {
            settings = GetSettings("test-settings.xml");
            
            Profile profile = new Profile();
            profile.id = defaultProfileId;

            profile.Repositories.Add(repoUrl1, true, false);

            Assert.AreEqual(0, settings.Profiles.Length);

            //previously it expected the profile to be add to the settings too.
            //Assert.AreEqual(1, settings.profiles.Length, "Settings does not contain a profile");

            Repository repository = profile.Repositories[repoUrl1];

            Assert.IsNotNull(repository, "Repository '" + repoUrl1 + "' was not added to profile");

            Assert.AreEqual("npanday.repo.0", repository.id);
            Assert.AreEqual(repoUrl1, repository.url);
        }
        
        [Test]
        public void AddRepositoryToExistingProfileTest()
        {
            settings = GetSettings("test-settings-with-profile.xml");

            Assert.AreEqual(1,settings.Profiles.Length);

            Profile profile = settings.Profiles[defaultProfileId];
            profile.Repositories.Add(repoUrl2, true, false);
            

            Assert.AreEqual(1, settings.Profiles.Length, "Settings does not contain a profile");
            Assert.AreEqual(2, settings.Profiles[0].Repositories.Length);

            Repository repository = profile.Repositories[repoUrl1];
            Assert.IsNotNull(repository, "Repository '" + repoUrl1 + "' was not in the profile");
            Assert.AreEqual("npanday.repo.0", repository.id);
            Assert.AreEqual(repoUrl1, repository.url);

            repository = profile.Repositories[repoUrl2];
            Assert.IsNotNull(repository, "Repository '" + repoUrl2 + "' was not added to profile");
            Assert.AreEqual("npanday.repo.1", repository.id);
            Assert.AreEqual(repoUrl2, repository.url);
        }



        private Settings GetSettings(string settingsXml)
        {

            string path = new ProjectStructure("NPanday.Model.Settings").TestResource.Folder().FullName;

            path = Path.Combine(path, "m2");
            settingsPath = new FileInfo(Path.Combine(path, settingsXml)).FullName;

            return Settings.Read(settingsPath);
        }
    }
}
