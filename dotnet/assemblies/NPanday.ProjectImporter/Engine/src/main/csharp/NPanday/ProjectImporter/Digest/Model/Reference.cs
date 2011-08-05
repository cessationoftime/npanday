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
using System.IO;

using NPanday.Utils;
using System.Reflection;
using NPanday.Artifact;
using NPanday.Model.Settings;
using System.Windows.Forms;
using System.Net;
using NPanday.Model.Settings;

/// Author: Leopoldo Lee Agdeppa III

namespace NPanday.ProjectImporter.Digest.Model
{

    public class Reference : IncludeBase
    {
        #region Constructors

        public Reference(string projectBasePath)
            : base(projectBasePath)
        {
        }

        #endregion

        #region Properties

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string hintPath;
        public string HintPath
        {
            get { return hintPath; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                hintPath = value; 
                SetReferenceFromFile(value); 
            }
        }

        public string HintFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(hintPath))
                {
                    return null;
                }
                else if (Path.IsPathRooted(hintPath))
                {
                    return Path.GetFullPath(hintPath);
                }
                else
                {
                    return Path.GetFullPath(Path.Combine(projectBasePath, hintPath));
                }

            }
        }

        private string version;
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private string publicKeyToken;
        public string PublicKeyToken
        {
            get { return publicKeyToken; }
            set { publicKeyToken = value; }
        }

        private string culture;
        public string Culture
        {
            get { return culture; }
            set { culture = value; }
        }

        private string processorArchitecture;
        public string ProcessorArchitecture
        {
            get { return processorArchitecture; }
            set { processorArchitecture = value; }
        }

        #endregion


        #region HelperMethods

        private void SetReferenceFromFile(string dll)
        {
            if (string.IsNullOrEmpty(dll))
            {
                return;
            }
            SetReferenceFromFile(new FileInfo(dll));
        }


        private void SetReferenceFromFile(FileInfo dll)
        {
            Assembly asm = null ;
            string path = string.Empty;

            //if (dll.Exists)
            if (dll.Exists)
            {
                //asm = Assembly.ReflectionOnlyLoadFrom(dll.FullName);
                path = dll.FullName;
            }
            else
            {
                ArtifactContext artifactContext = new ArtifactContext();
                Artifact.Artifact a = artifactContext.GetArtifactRepository().GetArtifact(dll);
                
                if (a != null)
				{
					if (!a.FileInfo.Exists)
                    {
						if (!a.FileInfo.Directory.Exists)
							a.FileInfo.Directory.Create();

						string localRepoPath = artifactContext.GetArtifactRepository().GetLocalRepositoryPath(a, dll.Extension);
						if (File.Exists(localRepoPath))
						{
							File.Copy(localRepoPath, a.FileInfo.FullName);
							//asm = Assembly.ReflectionOnlyLoadFrom();
							path = a.FileInfo.FullName;
						}
						else
						{
							if (a.Download(null))
							{
								//asm = Assembly.ReflectionOnlyLoadFrom(a.FileInfo.FullName);
								path = a.FileInfo.FullName;
							}
							else
							{
								path = getBinReference(dll.Name);
								if (!string.IsNullOrEmpty(path))
								{
									File.Copy(path, a.FileInfo.FullName);
								}
							}
							//copy assembly to repo if not found.
							if (!string.IsNullOrEmpty(path) && !File.Exists(localRepoPath))
							{
								if (!Directory.Exists(Path.GetDirectoryName(localRepoPath)))
									Directory.CreateDirectory(Path.GetDirectoryName(localRepoPath));

								File.Copy(path, localRepoPath);
							}
						}
					}
					else
					{
						path = a.FileInfo.FullName;
					}
				}
                if (a == null || string.IsNullOrEmpty(path))
                {
                    MessageBox.Show("Cannot find or download the artifact " + dll.Name + ",  project may not build properly.");
                    return;
                }
            }

            bool asmNotLoaded = true;
            foreach (Assembly asmm in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
            {
                // compare the assembly name to the filename of the reference to determine if it is a match
                // as the location might not be set
                // TODO: why do we need to load the assembly?
                // added StringComparison.OrdinalIgnoreCase to assembly name compratison in order to avoid errors with 
                // already loaded assemblies like nunit.framework and NUnit.Framework etc (note this can be reconsidered)
                if (asmm.GetName().Name.Equals(Path.GetFileNameWithoutExtension(path), StringComparison.OrdinalIgnoreCase))
                {
                    asm = asmm;
                    asmNotLoaded = false;
                    break;
                }
            }
            if (asmNotLoaded)
            {
                asm = Assembly.ReflectionOnlyLoadFrom(path);
            }

            SetAssemblyInfoValues(asm.ToString());
            //asm = null;

        }

        string getBinReference(string fileName) {
            string path = Path.Combine(this.IncludeFullPath, @"bin\" + Path.GetFileName(fileName));
            
            if (File.Exists(path))
                return path;

            path = Path.Combine(this.IncludeFullPath, @"bin\debug\" + Path.GetFileName(fileName));
            if (File.Exists(path))
                return path;

            path = Path.Combine(this.IncludeFullPath, @"bin\release\" + Path.GetFileName(fileName));
            if (File.Exists(path))
                return path;

            path = Path.Combine(this.IncludeFullPath, @"obj\debug\" + Path.GetFileName(fileName));
            if (File.Exists(path))
                return path;

            path = Path.Combine(this.IncludeFullPath, @"obj\release\" + Path.GetFileName(fileName));
            if (File.Exists(path))
                return path;

            return string.Empty;
        }

       

        
          

        public void SetAssemblyInfoValues(string assemblyInfo)
        {
            if (!string.IsNullOrEmpty(assemblyInfo))
            {
                string[] referenceValues = assemblyInfo.Split(',');
                this.Name = referenceValues[0].Trim();

                if (referenceValues.Length > 1)
                {
                    for (int i = 1; i < referenceValues.Length; i++)
                    {
                        if (referenceValues[i].Contains("Version="))
                        {
                            this.Version = referenceValues[i].Replace("Version=", "").Trim();
                        }
                        else if (referenceValues[i].Contains("PublicKeyToken="))
                        {
                            this.PublicKeyToken = referenceValues[i].Replace("PublicKeyToken=", "").Trim();
                        }
                        else if (referenceValues[i].Contains("Culture="))
                        {
                            this.Culture = referenceValues[i].Replace("Culture=", "").Trim();
                        }
                        else if (referenceValues[i].Contains("processorArchitecture="))
                        {
                            this.ProcessorArchitecture = referenceValues[i].Replace("processorArchitecture=", "").Trim();
                        }
                    }
                }

            }

        }

        #endregion






    }
}
