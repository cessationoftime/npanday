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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NPanday.Utils;
using System.IO;
using NPanday.Model;

namespace ConnectTest.UtilsTest
{
    [TestFixture]
    public class MavenResxPluginConfigurationTest
    {
        private PomXml pomCopy;
        private String pomPath;
        private String pomCopyPath;


        
        public MavenResxPluginConfigurationTest()
        {

        }

        [SetUp]
        public void TestSetUp()
        {
            ResourceFolder classLibraryResource = new SimpleSrcStructure(MainOrTest.Test, "NPanday.VisualStudio.Addin").Resources.Folder("ClassLibrary1\\ClassLibrary1\\");
            pomPath = classLibraryResource.File("pom.xml").FullName;
           

            pomCopyPath = pomPath.Replace("pom.xml", "pomCopy.xml");

            pomCopy = new PomXml(pomCopyPath);

            File.Copy(pomPath, pomCopyPath);
        }
 
        [Test]
        public void AddMavenResxPluginConfigurationTest()
        {
            pomCopy.AddMavenResxPluginConfiguration("org.apache.npanday.plugins", "maven-resgen-plugin", "embeddedResources", "embeddedResource", "Copy of Resource1.resx", "ClassLibrary1.Copy of Resource1");
        }

        [Test]
        public void RenameMavenResxPluginConfigurationTest()
        {
            pomCopy.RenameMavenResxPluginConfiguration("org.apache.npanday.plugins", "maven-resgen-plugin", "embeddedResources", "embeddedResource", "Copy of Resource1.resx", "ClassLibrary1.Copy of Resource1", "ToBeDeleted.resx", "ClassLibrary1.ToBeDeleted");
        }

        [Test]
        public void RemoveMavenResxPluginConfigurationTest()
        {
            pomCopy.RemoveMavenResxPluginConfiguration("org.apache.npanday.plugins", "maven-resgen-plugin", "embeddedResources", "embeddedResource", "ToBeDeleted.resx", "ClassLibrary1.ToBeDeleted");
            //File.Delete(pomCopyPath);
        }

        [TearDown]
        public void TestCleanUp()
        {
            File.Delete(pomCopyPath);
        }

    }
}
