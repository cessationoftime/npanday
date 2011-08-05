using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE80;
using EnvDTE;
using System.IO;
using System.Linq;
using NPanday.VisualStudioProjectTypes;
namespace NPanday.Model
{
    
    /// <summary>
    /// The overall Structure of a solution
    /// </summary>
    public class SolutionStructure
    {
        Solution solution;
        
        public SolutionStructure(Solution solution)
        {
            this.solution = solution;
        }
        public const string MSG_E_PARENTPOM_NOTFOUND = "parent pom.xml Not Found";//from Parent pom.xml to paren-pom.xml

        /// <summary>
        /// Solution pom.xml
        /// </summary>
        public PomXml PomXml
        {
            get
            {
                try
                {
                    PomXml parentPomXml = new PomXml(Path.GetDirectoryName(solution.FileName) + @"\parent-pom.xml");
                    PomXml pomXml = new PomXml(Path.GetDirectoryName(solution.FileName) + @"\pom.xml");

                    if (!parentPomXml.Exists)
                    {
                        return pomXml;
                    }
                    else if (pomXml.Exists || parentPomXml.Exists)
                    {
                        if (!"pom".Equals(pomXml.Packaging)
                            && parentPomXml.Exists
                            && "pom".Equals(parentPomXml.Packaging))
                        {
                            return parentPomXml;
                        }
                        return pomXml;
                    }
                    else
                    {
                        //MessageBox.Show("Parent pom.xml Not Found! ",
                        //"File Not Found:",
                        //MessageBoxButtons.OK,
                        //MessageBoxIcon.Error);
                        throw new Exception(MSG_E_PARENTPOM_NOTFOUND);
                    }
                }
                catch (Exception)
                {
                    //MessageBox.Show("Locating Parent pom.xml Error: " + e.Message, 
                    //    "Locating Parent pom.xml Error:",
                    //    MessageBoxButtons.OK,
                    //    MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        /// <summary>
        /// Names of the projects (SrcStructures) in the solution
        /// </summary>
        public string[] SrcNames
        {
            get
            {
                List<string> srcNamesList = new List<string>();
                foreach (SrcStructure src in Sources)
                {
                    srcNamesList.Add(src.Name);
                }
                return srcNamesList.ToArray();
            }
        }


        public List<ProjectStructure> Projects
        {
            get
            {
                List<ProjectStructure> projectList = new List<ProjectStructure>();

                foreach (SrcStructure srcMain in sourcesWithoutParent.Where(x=>x.isMainSrc))
                {
                    bool added = false;
                    foreach (SrcStructure srcTest in sourcesWithoutParent.Where(x => x.isTestSrc))
                    {
                        if (srcMain.SrcFolder.FullName == srcTest.SrcFolder.FullName)
                        {
                            projectList.Add(new ProjectStructure(srcMain, srcTest,this));
                            added = true;
                        }
                    }
                    if (!added) projectList.Add(new ProjectStructure(srcMain, null,this));
                }

                return projectList;
            }
        }

        /// <summary>
        /// Test and Main Projects
        /// </summary>
        public List<SrcStructure> Sources
        {
            get
            {
                List<SrcStructure> srcList = new List<SrcStructure>();
                foreach (ProjectStructure p in this.Projects)
                {
                   srcList.AddRange(p.Sources);
                }
                return srcList;
            }
        }

        /// <summary>
        /// get source representing the given project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public SrcStructure getSource(Project project)
        {
            foreach (SrcStructure src in Sources)
            {
                if (src.SrcProject == project) return src;
            }
            throw new Exception("Could not find Source: " + project.Name);
        }

        private List<SrcStructure> sourcesWithoutParent
        {
            get
            {
                List<SrcStructure> srcList = new List<SrcStructure>();
                foreach (Project p in this.Solution.Projects)
                {
                    bool supported = true;
                    try
                    {                       
                        VisualStudioProjectType.GetVisualStudioProjectType(p.Kind);
                    }
                    catch (NotSupportedException) { supported = false; }
                    if (supported) srcList.Add(new SrcStructure(p));
                }
                return srcList;

            }
        }

 
        public List<SrcStructure> MainSources
        {
            get
            {
                List<SrcStructure> srcList = new List<SrcStructure>();
                foreach (ProjectStructure p in this.Projects)
                {
                    srcList.Add(p.SrcMain);
                }
                return srcList;
            }
        }

        public List<SrcStructure> TestSources
        {
            get
            {
                List<SrcStructure> srcList = new List<SrcStructure>();
                foreach (ProjectStructure p in this.Projects)
                {
                    srcList.Add(p.SrcTest);
                }
                return srcList;
            }
        }

        public Solution Solution
        {
            get
            {
                return solution;
            }
        }

        public static DirectoryInfo SolutionFolder(Solution solution)
        {
            return new FileInfo(solution.FileName).Directory;
        }

        public DirectoryInfo Folder
        {
            get
            {
                return SolutionStructure.SolutionFolder(solution);
            }
        }

    }
}