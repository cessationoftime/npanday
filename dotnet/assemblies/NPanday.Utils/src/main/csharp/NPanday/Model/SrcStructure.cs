using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EnvDTE;
using VSLangProj80;
using VsWebSite;
using VSLangProj;
using NPanday.VisualStudioProjectTypes;

namespace NPanday.Model
{
    public enum MainOrTest
    {
        Main,
        Test
    }


    /// <summary>
    /// The overall structure of a maven src folder (main or test source project).
    /// </summary>
    public class SrcStructure : SimpleSrcStructure
    {
        
        public const string FOLDER_KIND_GUID = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";
        //public const string MISC_FILES_KIND_GUID = "{66A2671D-8FB5-11D2-AA7E-00C04F688DDE}";
        //public const string SOLUTION_ITEMS_KIND_GUID = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";
        //public const string UNMODELED_KIND_GUID = "{67294A52-A4F0-11D2-AA88-00C04F688DDE}";
        public const string WEB_APPLICATION_KIND_GUID = "{349C5851-65DF-11DA-9384-00065B846F21}";
        public const string WEB_PROJECT_KIND_GUID = "{E24C65DC-7377-472B-9ABA-BC803B73C61A}";

//        public static string[] NONPROJECT_KINDS = new string[] { FOLDER_KIND_GUID, MISC_FILES_KIND_GUID, SOLUTION_ITEMS_KIND_GUID, UNMODELED_KIND_GUID };

        Project srcProject;
      
        ProjectStructure parent;
        ReferenceManager refManager;
        public SrcStructure(Project srcProject) : base(srcProject.FullName)
        {
            this.srcProject = srcProject;
        }

        public ProjectStructure ParentStructure
        {
            get
            {
                return parent;
            }
            internal set
            {
                parent = value;
            }
        }

        public VSWebSite getWebSiteObject
        {
            get
            {
                return (VSWebSite)srcProject.Object;
            }
        }

        public VSProject2 getProjectObject
        {
            get
            {
                return (VSProject2)srcProject.Object;
            }
        }

        public Project SrcProject
        {
            get
            {
                return srcProject;
            }
        }

        public VisualStudioProjectTypeEnum Kind
        {
            get
            {
                return VisualStudioProjectType.GetVisualStudioProjectType(srcProject.Kind);
            }
        }

        public string createWebReferencesFolder()
        {
                string wsPath = null;
                if (this.isWebProject)
                {
                    wsPath = Path.Combine(getWebSiteObject.Project.FullName, "App_WebReferences");
                    if (!Directory.Exists(wsPath))
                    {
                        Directory.CreateDirectory(wsPath);
                    }
                }
                else
                {                    
                    ProjectItem webReferenceFolder = getProjectObject.WebReferencesFolder;
                    if (webReferenceFolder == null)
                    {
                        webReferenceFolder = getProjectObject.CreateWebReferencesFolder();
                    }

                    wsPath = Path.Combine(Path.GetDirectoryName(srcProject.FullName), webReferenceFolder.Name);
                }
                return wsPath;
        }

        public Solution Solution
        {
            get
            {
                try
                {
                    return getProjectObject.DTE.Solution;
                }
                catch
                {
                    return getWebSiteObject.DTE.Solution;
                }
            }
        }

          public string FullName
        {
            get
            {
                return SrcProject.FullName;

            }
        }

          public string Name
          {
              get
              {
                  return SrcProject.Name;

              }
          }

        public DirectoryInfo SolutionFolder
        {
            get
            {
                return SolutionStructure.SolutionFolder(Solution);
            }
        }

        /// <summary>
        /// Checks the currently selected pom file if there is a vb project in it.
        /// </summary>
        private bool isVbProject
        {
            get
            {
                // make sure there's a project item
                if (srcProject == null) throw new Exception("Unable to determine project type");

                if (srcProject.UniqueName.Contains("vbproj")) return true;
                return false;
            }
        }

        public bool isProject
        {
            get
            {
                return srcProject.Object is VSProject;
            }
        }

        
        public ReferenceManager ReferenceManager
        {
            get
            {
                if (refManager == null)
                {
                    refManager = new ReferenceManager(this.ParentStructure.ParentStructure.SrcNames, this.ParentStructure.PomXml, this.projectFolder.FullName);
                }
                return refManager;
            }
        }

        public bool isWebProject
        {
            get
            {
                // make sure there's a project item
                if (srcProject == null) throw new Exception("Unable to determine project type");



                // compare the project kind to the web project guid
                if (String.Compare(srcProject.Kind, WEB_PROJECT_KIND_GUID, true) == 0)
                {
                    return true;
                }

                // compare the project kind to the web application guid
                if (String.Compare(srcProject.Kind, WEB_APPLICATION_KIND_GUID, true) == 0)
                {
                    return true;
                }

                return false;
            }
        }



        public bool isFolder
        {
            get
            {
                // make sure there's a project item
                if (srcProject == null) throw new Exception("Unable to determine project type");

                // compare the project kind to the folder guid
                return (String.Compare(srcProject.Kind, FOLDER_KIND_GUID, true) == 0);
            }
        }
    }

}
