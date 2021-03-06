 #region License
 // Licensed to the Apache Software Foundation (ASF) under one
 // or more contributor license agreements.  See the NOTICE file
 // distributed with this work for additional information
 // regarding copyright ownership.  The ASF licenses this file
 // to you under the Apache License, Version 2.0 (the
 // "License"); you may not use this file except in compliance
 // with the License.  You may obtain a copy of the License at
 //
 //  http://www.apache.org/licenses/LICENSE-2.0
 //
 // Unless required by applicable law or agreed to in writing,
 // software distributed under the License is distributed on an
 // "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 // KIND, either express or implied.  See the License for the
 // specific language governing permissions and limitations
 // under the License.
 #endregion
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.42.
// 
namespace NPanday.Model.Settings {
    using System.Xml.Serialization;
    using System.Xml;
using System.IO;
using System;
    using System.Collections.Generic;

    public enum SettingsPath
    {
        UserSettings,
        GlobalSettings
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute()]
    [System.Xml.Serialization.XmlRootAttribute("settings", IsNullable = false)]
    public partial class Settings {
        
        private string localRepositoryField;
        
        private bool interactiveModeField;
        
        private bool usePluginRegistryField;
        
        private bool offlineField;
        
        private Proxy[] proxiesField;
        
        private Server[] serversField;
        
        private Mirror[] mirrorsField;
        
        private ProfileList profilesField = new ProfileList();

        private List<string> activeProfileIds = new List<string>();
        
        private string[] pluginGroupsField;
        
        public Settings() {
            this.interactiveModeField = true;
            this.usePluginRegistryField = false;
            this.offlineField = false;
        }
        
        /// <remarks/>
        public string localRepository {
            get {
                return this.localRepositoryField;
            }
            set {
                this.localRepositoryField = value;
            }
        }

     

        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool interactiveMode {
            get {
                return this.interactiveModeField;
            }
            set {
                this.interactiveModeField = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool usePluginRegistry {
            get {
                return this.usePluginRegistryField;
            }
            set {
                this.usePluginRegistryField = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool offline {
            get {
                return this.offlineField;
            }
            set {
                this.offlineField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("proxy", IsNullable=false)]
        public Proxy[] proxies {
            get {
                return this.proxiesField;
            }
            set {
                this.proxiesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("server", IsNullable=false)]
        public Server[] servers {
            get {
                return this.serversField;
            }
            set {
                this.serversField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("mirror", IsNullable=false)]
        public Mirror[] mirrors {
            get {
                return this.mirrorsField;
            }
            set {
                this.mirrorsField = value;
            }
        }
        
        [XmlArray("profiles")]
        [XmlArrayItem("profile", IsNullable = false)]
        public ProfileList Profiles
        {
            get
            {
                return this.profilesField;
            }
        }

        /// <summary>
        /// convenience method to call Profiles.ActiveList
        /// </summary>
        [XmlIgnore]
        public ProfileList ActiveProfiles
        {
            get
            {
                return Profiles.ActiveList;
            }
        }
        


        [XmlArray("activeProfiles")]
        [XmlArrayItem("activeProfile", IsNullable=false)]
        public List<string> ActiveProfileIds {
            get {
                return this.activeProfileIds;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("pluginGroup", IsNullable=false)]
        public string[] pluginGroups {
            get {
                return this.pluginGroupsField;
            }
            set {
                this.pluginGroupsField = value;
            }
        }

        /// <summary>
        /// Get repository from settings by url
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <param name="url">The repository url</param>
        /// <returns>The repository from the settings.xml</returns>
        public Repository GetRepository(string url)
        {
            foreach (Profile profile in this.Profiles)
            {
                return profile.Repositories[url];
            }
            return null;
        }

        /// <summary>
        /// Returns the default profile.  If the default profile does not exist it is created
        /// </summary>
        /// <returns></returns>
        public Profile getDefaultProfile()
        {
            Profile p = this.Profiles[Profile.DefaultProfileID];

            if (p==null) {
                p = new Profile();
                p.id = Profile.DefaultProfileID;
                this.Profiles.Add(p);
            }
            return p;
        }

        public static Settings Read(SettingsPath path)
        {
            if (path == SettingsPath.UserSettings) return Read(SettingsUtil.GetUserSettingsPath());
            else if (path == SettingsPath.UserSettings) return Read(SettingsUtil.GetDefaultSettingsPath());
            else throw new NotImplementedException();
        }

        public static Settings Read(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            path = path.Trim();

            if (path.Length == 0)
            {
                throw new ArgumentException("Value is empty", "path");
            }

            return Read(new FileInfo(path));
        }

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <returns>
        /// The settings read from the file
        /// </returns>
        public static Settings Read(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException("fileInfo");
            }

            TextReader reader = null;
            try
            {
                reader = new System.IO.StreamReader(fileInfo.FullName);
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                Settings s = (Settings)serializer.Deserialize(new NamespaceIgnorantXmlTextReader(reader));
                s.setActiveProfiles();
                return s;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// Sets the isActive property of the Profiles based on the ids in activeProfiles
        /// </summary>
        private void setActiveProfiles() {
            foreach (string activeProfileId in activeProfileIds)
            {
                Profiles[activeProfileId].isActive = true;
            }
        }

        /// <summary>
        /// Get all repositories from the settings.xml
        /// </summary>
        /// <param name="settings">The settings info</param>
        /// <returns>
        /// All repositories from the settings
        /// </returns>
        public RepositoryList GetAllRepositories()
        {
            RepositoryList repos = new RepositoryList();

            foreach (Profile profile in this.Profiles)
            {
               repos.AddRange(profile.Repositories);
            }
            return repos;
        }

        /// <summary>
        /// Write the settings
        /// </summary>
        /// <param name="settings">The settings to write</param>
        /// <param name="path">The settings path</param>
        public void Write(string path)
        {
            // read original settings.xml
            XmlDocument settingsXmlDoc = new XmlDocument();
            settingsXmlDoc.Load(path);

            Profile profile = this.getDefaultProfile();
            //convert NPanday Profile to XmlNode
            XmlNode newProfileNode = new XmlDocument();
            ((XmlDocument)newProfileNode).LoadXml(profile.Serialize());

            newProfileNode = newProfileNode.SelectSingleNode("//profile[id='" + Profile.DefaultProfileID + "']");
            XmlNode importedNewProfileNode = settingsXmlDoc.ImportNode(newProfileNode, true);

            // search for npanday profile in settings.xml
            XmlNode oldProfileNode = settingsXmlDoc.SelectSingleNode("//profiles/profile[id='" + Profile.DefaultProfileID + "']");
            if (oldProfileNode != null)
            {
                oldProfileNode.ParentNode.ReplaceChild(importedNewProfileNode, oldProfileNode);
            }
            else
            {
                XmlNode profilesNode = settingsXmlDoc.SelectSingleNode("//profiles");
                if (profilesNode == null)
                {
                    // create profiles
                    profilesNode = settingsXmlDoc.CreateElement("", "profiles", "");
                    settingsXmlDoc.DocumentElement.AppendChild(profilesNode);
                }

                profilesNode.AppendChild(importedNewProfileNode);
            }

            bool createActiveProfile = false;
            XmlNode activeProfileNode;

            // search for activeProfiles in settings.xml
            XmlNode activeProfilesNode = settingsXmlDoc.SelectSingleNode("//activeProfiles");
            if (activeProfilesNode == null)
            {
                activeProfilesNode = settingsXmlDoc.CreateElement("", "activeProfiles", "");
                settingsXmlDoc.DocumentElement.AppendChild(activeProfilesNode);
                createActiveProfile = true;
            }
            else
            {
                activeProfileNode = settingsXmlDoc.SelectSingleNode("//activeProfiles/activeProfile[.='" + Profile.DefaultProfileID + "']");
                if (activeProfileNode == null)
                {
                    createActiveProfile = true;
                }
            }

            // add NPanday.id to <activeProfiles>
            if (createActiveProfile)
            {
                activeProfileNode = settingsXmlDoc.CreateElement("", "activeProfile", "");
                activeProfileNode.InnerText = Profile.DefaultProfileID;
                activeProfilesNode.AppendChild(activeProfileNode);
            }

            settingsXmlDoc.Save(path);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class Proxy {
        
        private bool activeField;
        
        private string protocolField;
        
        private string usernameField;
        
        private string passwordField;
        
        private int portField;
        
        private string hostField;
        
        private string nonProxyHostsField;
        
        private string idField;
        
        public Proxy() {
            this.activeField = false;
            this.protocolField = "http";
            this.portField = 9191;
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool active {
            get {
                return this.activeField;
            }
            set {
                this.activeField = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("http")]
        public string protocol {
            get {
                return this.protocolField;
            }
            set {
                this.protocolField = value;
            }
        }
        
        /// <remarks/>
        public string username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
            }
        }
        
        /// <remarks/>
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(9191)]
        public int port {
            get {
                return this.portField;
            }
            set {
                this.portField = value;
            }
        }
        
        /// <remarks/>
        public string host {
            get {
                return this.hostField;
            }
            set {
                this.hostField = value;
            }
        }
        
        /// <remarks/>
        public string nonProxyHosts {
            get {
                return this.nonProxyHostsField;
            }
            set {
                this.nonProxyHostsField = value;
            }
        }
        
        /// <remarks/>
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class RepositoryPolicy {
        
        private bool enabledField;
        
        private string updatePolicyField;
        
        private string checksumPolicyField;
        
        public RepositoryPolicy() {
            this.enabledField = true;
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool enabled {
            get {
                return this.enabledField;
            }
            set {
                this.enabledField = value;
            }
        }
        
        /// <remarks/>
        public string updatePolicy {
            get {
                return this.updatePolicyField;
            }
            set {
                this.updatePolicyField = value;
            }
        }
        
        /// <remarks/>
        public string checksumPolicy {
            get {
                return this.checksumPolicyField;
            }
            set {
                this.checksumPolicyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class Repository {
        
        private RepositoryPolicy releasesField;
        
        private RepositoryPolicy snapshotsField;
        
        private string idField;
        
        private string nameField;
        
        private string urlField;
        
        private string layoutField;
        
        public Repository() {
            this.layoutField = "default";
        }
        
        /// <remarks/>
        public RepositoryPolicy releases {
            get {
                return this.releasesField;
            }
            set {
                this.releasesField = value;
            }
        }
        
        /// <remarks/>
        public RepositoryPolicy snapshots {
            get {
                return this.snapshotsField;
            }
            set {
                this.snapshotsField = value;
            }
        }
        
        /// <remarks/>
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string url {
            get {
                return this.urlField;
            }
            set {
                this.urlField = value;
            }
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute("default")]
        public string layout {
            get {
                return this.layoutField;
            }
            set {
                this.layoutField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class ActivationFile {
        
        private string missingField;
        
        private string existsField;
        
        /// <remarks/>
        public string missing {
            get {
                return this.missingField;
            }
            set {
                this.missingField = value;
            }
        }
        
        /// <remarks/>
        public string exists {
            get {
                return this.existsField;
            }
            set {
                this.existsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class ActivationProperty {
        
        private string nameField;
        
        private string valueField;
        
        /// <remarks/>
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class ActivationOS {
        
        private string nameField;
        
        private string familyField;
        
        private string archField;
        
        private string versionField;
        
        /// <remarks/>
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string family {
            get {
                return this.familyField;
            }
            set {
                this.familyField = value;
            }
        }
        
        /// <remarks/>
        public string arch {
            get {
                return this.archField;
            }
            set {
                this.archField = value;
            }
        }
        
        /// <remarks/>
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class Activation {
        
        private bool activeByDefaultField;
        
        private string jdkField;
        
        private ActivationOS osField;
        
        private ActivationProperty propertyField;
        
        private ActivationFile fileField;
        
        public Activation() {
            this.activeByDefaultField = false;
        }
        
        /// <remarks/>
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool activeByDefault {
            get {
                return this.activeByDefaultField;
            }
            set {
                this.activeByDefaultField = value;
            }
        }
        
        /// <remarks/>
        public string jdk {
            get {
                return this.jdkField;
            }
            set {
                this.jdkField = value;
            }
        }
        
        /// <remarks/>
        public ActivationOS os {
            get {
                return this.osField;
            }
            set {
                this.osField = value;
            }
        }
        
        /// <remarks/>
        public ActivationProperty property {
            get {
                return this.propertyField;
            }
            set {
                this.propertyField = value;
            }
        }
        
        /// <remarks/>
        public ActivationFile file {
            get {
                return this.fileField;
            }
            set {
                this.fileField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
   public partial class Profile {
        internal static string DefaultProfileID = "NPanday.id";
        private Activation activationField;
        
        private ProfileProperties propertiesField;

        private RepositoryList repositoriesField = new RepositoryList();

        private RepositoryList pluginRepositoriesField = new RepositoryList();
        
        private string idField;
        
        private bool is_Active;

        public bool isActive
        {
            get
            {
                return is_Active;
            }
            set
            {
                is_Active = value;
            }
        }

        /// <remarks/>
        public Activation activation {
            get {
                return this.activationField;
            }
            set {
                this.activationField = value;
            }
        }
        
        /// <remarks/>
        public ProfileProperties properties
        {
            get
            {
                return this.propertiesField;
            }
            set
            {
                this.propertiesField = value;
            }


        }
        internal string Serialize()
        {
            XmlRootAttribute profileRoot = new XmlRootAttribute("profile");

            XmlAttributes attributes = new XmlAttributes();
            attributes.XmlRoot = profileRoot;

            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            overrides.Add(this.GetType(), attributes);

            XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");

            XmlSerializer xs = new XmlSerializer(this.GetType(), overrides);
            StringWriter xout = new StringWriter();
            xs.Serialize(xout, this, xmlnsEmpty);
            String SerializedProfile = xout.ToString();
            xout.Close();

            return SerializedProfile;
        }
        [XmlArray("repositories")]
        [XmlArrayItem("repository", IsNullable = false)]
        public RepositoryList Repositories
        {
            get
            {
                return this.repositoriesField;
            }
        }
        [XmlArray("pluginRepositories")]
        [XmlArrayItem("pluginRepository", IsNullable = false)]
        public RepositoryList PluginRepositories
        {
            get
            {
                return this.pluginRepositoriesField;
            }
        }


   
        /// <remarks/>
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="")]
    public partial class ProfileProperties {
        
        private System.Xml.XmlElement[] anyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class Mirror {
        
        private string mirrorOfField;
        
        private string nameField;
        
        private string urlField;
        
        private string idField;
        
        /// <remarks/>
        public string mirrorOf {
            get {
                return this.mirrorOfField;
            }
            set {
                this.mirrorOfField = value;
            }
        }
        
        /// <remarks/>
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public string url {
            get {
                return this.urlField;
            }
            set {
                this.urlField = value;
            }
        }
        
        /// <remarks/>
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="")]
    public partial class Server {
        
        private string usernameField;
        
        private string passwordField;
        
        private string privateKeyField;
        
        private string passphraseField;
        
        private string filePermissionsField;
        
        private string directoryPermissionsField;
        
        private ServerConfiguration configurationField;
        
        private string idField;
        
        /// <remarks/>
        public string username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
            }
        }
        
        /// <remarks/>
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        public string privateKey {
            get {
                return this.privateKeyField;
            }
            set {
                this.privateKeyField = value;
            }
        }
        
        /// <remarks/>
        public string passphrase {
            get {
                return this.passphraseField;
            }
            set {
                this.passphraseField = value;
            }
        }
        
        /// <remarks/>
        public string filePermissions {
            get {
                return this.filePermissionsField;
            }
            set {
                this.filePermissionsField = value;
            }
        }
        
        /// <remarks/>
        public string directoryPermissions {
            get {
                return this.directoryPermissionsField;
            }
            set {
                this.directoryPermissionsField = value;
            }
        }
        
        /// <remarks/>
        public ServerConfiguration configuration {
            get {
                return this.configurationField;
            }
            set {
                this.configurationField = value;
            }
        }
        
        /// <remarks/>
        public string id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="")]
    public partial class ServerConfiguration {
        
        private System.Xml.XmlElement[] anyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
    }
}