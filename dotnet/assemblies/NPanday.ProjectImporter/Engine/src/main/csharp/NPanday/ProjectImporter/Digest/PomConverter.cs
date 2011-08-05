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
using System.Windows.Forms;



using NPanday.ProjectImporter.Digest;
using NPanday.ProjectImporter.Digest.Model;
using NPanday.Utils;
using NPanday.Model.Pom;

using NPanday.Artifact;

using NPanday.ProjectImporter.Converter.Algorithms;
using NPanday.VisualStudioProjectTypes;
using NPanday.Model;


/// Author: Leopoldo Lee Agdeppa III

namespace NPanday.ProjectImporter.Converter
{
    public class PomConverter
    {
        // Converter Algorithm Registry
        public static Dictionary<VisualStudioProjectTypeEnum, Type> __converterAlgorithms;
        static PomConverter()
        {
            __converterAlgorithms = new Dictionary<VisualStudioProjectTypeEnum, Type>();
            
            // register converter algorithms
            __converterAlgorithms.Add(VisualStudioProjectTypeEnum.Windows__CSharp, typeof(NormalPomConverter));
            __converterAlgorithms.Add(VisualStudioProjectTypeEnum.Windows__VbDotNet, typeof(NormalPomConverter));
            __converterAlgorithms.Add(VisualStudioProjectTypeEnum.Web_Site, typeof(WebPomConverter));
            __converterAlgorithms.Add(VisualStudioProjectTypeEnum.Web_Application, typeof(WebPomConverter));


            // combination of types
            __converterAlgorithms.Add(
                VisualStudioProjectTypeEnum.Web_Site | VisualStudioProjectTypeEnum.Windows__CSharp,
                typeof(WebWithVbOrCsProjectFilePomConverter)
              );
            __converterAlgorithms.Add(
                VisualStudioProjectTypeEnum.Web_Site | VisualStudioProjectTypeEnum.Windows__VbDotNet,
                typeof(WebWithVbOrCsProjectFilePomConverter)
              );

            __converterAlgorithms.Add(
                VisualStudioProjectTypeEnum.Web_Application | VisualStudioProjectTypeEnum.Windows__CSharp,
                typeof(WebWithVbOrCsProjectFilePomConverter)
              );
            __converterAlgorithms.Add(
                VisualStudioProjectTypeEnum.Web_Application | VisualStudioProjectTypeEnum.Windows__VbDotNet,
                typeof(WebWithVbOrCsProjectFilePomConverter)
              );
            __converterAlgorithms.Add(
                VisualStudioProjectTypeEnum.Windows_Presentation_Foundation__WPF | VisualStudioProjectTypeEnum.Windows__CSharp,
                typeof(NormalPomConverter)
              );
            __converterAlgorithms.Add(
                VisualStudioProjectTypeEnum.Windows_Presentation_Foundation__WPF | VisualStudioProjectTypeEnum.Windows__VbDotNet,
                typeof(NormalPomConverter)
              );
            __converterAlgorithms.Add(
                VisualStudioProjectTypeEnum.Windows_Communication_Foundation__WCF | VisualStudioProjectTypeEnum.Windows__CSharp,
                typeof(NormalPomConverter)
              );
            __converterAlgorithms.Add(
                VisualStudioProjectTypeEnum.Windows_Communication_Foundation__WCF | VisualStudioProjectTypeEnum.Windows__VbDotNet,
                typeof(NormalPomConverter)
              );
			  
			__converterAlgorithms.Add(
				VisualStudioProjectTypeEnum.Model_View_Controller_MVC | VisualStudioProjectTypeEnum.Windows__CSharp | VisualStudioProjectTypeEnum.Web_Application,
				typeof(NormalPomConverter)
			);
        }


        public static NPanday.Model.Pom.Model MakeProjectsParentPomModel(ProjectDigest[] projectDigests, string pomFileName, string groupId, string artifactId, string version, string scmTag, bool writePom)
        {
            string errorPrj = string.Empty;
            try
            {
                NPanday.Model.Pom.Model model = new NPanday.Model.Pom.Model();

                model.modelVersion = "4.0.0";
                model.packaging = "pom";
                model.groupId = groupId;
                model.artifactId = artifactId;
                model.version = version;
                model.name = string.Format("{0} : {1}", groupId, artifactId);
                if (scmTag == null)
                {
                    scmTag = string.Empty;
                }

                if (scmTag.ToUpper().Contains("OPTIONAL"))
                {
                    scmTag = string.Empty;
                }

                if (scmTag.Contains("scm:svn:"))
                {
                    scmTag = scmTag.Remove(scmTag.IndexOf("scm:svn:"), 8);
                }

                Uri repoUri;
                bool isValidUrl = true;
                try
                {
                    if (!scmTag.Equals(string.Empty))
                    {
                        repoUri = new Uri(scmTag);
                    }
                    
                }
                catch(Exception)
                {
                    isValidUrl=false;
                    //MessageBox.Show(string.Format("SCM Tag was not added, because the url {0} was not accessible",scmTag), "NPanday Project Import", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
                
                if (!string.Empty.Equals(scmTag) && scmTag != null && isValidUrl)
                {
                    scmTag = scmTag.Trim();
                    
                    Scm scmHolder = new Scm();
                    scmHolder.connection = string.Format("scm:svn:{0}",scmTag); 
                    scmHolder.developerConnection = string.Format("scm:svn:{0}", scmTag); //why forcibly Subversion? (scm:hg for example). Need to add more fields to configure.
                    scmHolder.url = scmTag;
                    model.scm = scmHolder;
                }
                
                List<string> modules = new List<string>();
                foreach (ProjectDigest projectDigest in projectDigests)
                {
                    DirectoryInfo prjDir = new DirectoryInfo
                        (
                            projectDigest.ProjectType == VisualStudioProjectTypeEnum.Web_Site
                            ? projectDigest.FullFileName
                            : Path.GetDirectoryName(projectDigest.FullFileName)
                        );
                    DirectoryInfo pomDir = new DirectoryInfo(Path.GetDirectoryName(pomFileName));

                    string moduleDir = PomXml.GetRelativePath(pomDir, prjDir);
                    if (string.IsNullOrEmpty(moduleDir))
                    {
                        moduleDir = ".";
                        errorPrj += projectDigest.FullFileName;
                    }
                    modules.Add(moduleDir.Replace(@"\","/"));

                }

                model.modules = modules.ToArray();

                if (writePom)
                {
                    PomXml.WriteModelToPom(new FileInfo(pomFileName), model);
                }
                return model;
            }
            catch (Exception e)
            {
                throw new Exception("Project Importer failed with project "+errorPrj
                                   +" Project directory structure may not be supported. Cause: " + e.Message);
            }

            
        }



        public static NPanday.Model.Pom.Model[] ConvertProjectsToPomModels(ProjectDigest[] projectDigests, string mainPomFile, NPanday.Model.Pom.Model parent, string groupId, bool writePoms, string scmTag)
        {
            try
            {
                string version = parent != null ? parent.version : null;

                List<NPanday.Model.Pom.Model> models = new List<NPanday.Model.Pom.Model>();
                foreach (ProjectDigest projectDigest in projectDigests)
                {
                    NPanday.Model.Pom.Model model = ConvertProjectToPomModel(projectDigest, mainPomFile, parent, groupId, writePoms,scmTag);
                    
                    models.Add(model);
                }
                return models.ToArray();
            }
            catch
            {
                throw;
            }
        }

        public static NPanday.Model.Pom.Model ConvertProjectToPomModel(ProjectDigest projectDigest, string mainPomFile, NPanday.Model.Pom.Model parent, string groupId, bool writePom, string scmTag)
        {
            if (!__converterAlgorithms.ContainsKey(projectDigest.ProjectType))
            {
                throw new NotSupportedException("Not Supported Project Type: " + projectDigest.ProjectType);
            }
            else
           {

               try
                {
                    IPomConverter converter = (IPomConverter)System.Activator.CreateInstance(
                                                    __converterAlgorithms[projectDigest.ProjectType],
                                                    projectDigest,
                                                    mainPomFile,
                                                   parent,
                                                    groupId
                                                    );

                    converter.ConvertProjectToPomModel(scmTag);

                   return converter.Model;
                }
                catch
                {
                    throw;
                }
            }
        }


    }
}
