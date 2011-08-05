using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EnvDTE;
using VSLangProj80;
using NPanday.Model.Settings;

namespace NPanday.Model
{
    /// <summary>
    /// The overall structure of a maven project, including main and test sources.
    /// </summary>
    public class ProjectStructure
    {
        readonly DirectoryInfo projectFolder;
        string pomFilePath;
        SrcStructure mainSrc;
        SrcStructure testSrc;
        SolutionStructure parent;
        public ProjectStructure(SrcStructure mainSrc, SrcStructure testSrc, SolutionStructure parent)
        {
            this.parent = parent;
            if (testSrc != null) testSrc.ParentStructure = this;
            mainSrc.ParentStructure = this;
            this.mainSrc = mainSrc;
            this.testSrc = testSrc;
            DirectoryInfo findProjectFolder = mainSrc.SrcFolder;
            DirectoryInfo solutionFolder = mainSrc.SolutionFolder;

            //get the actual projectFolder containing the pom.xml
            while (findProjectFolder.Parent.FullName != solutionFolder.FullName)
            {
                findProjectFolder = findProjectFolder.Parent;
            }
            projectFolder = findProjectFolder;

            pomFilePath = Path.Combine(projectFolder.FullName, "pom.xml");
        }

        public SrcStructure SrcMain
        {
            get
            {
                return mainSrc;
            }
        }

        public SolutionStructure ParentStructure
        {
            get
            {
                return parent;
            }
        }

        public SrcStructure SrcTest
        {
            get
            {
                return testSrc;
            }
        }

        /// <summary>
        /// SrcMain[0] and SrcTest[1]
        /// </summary>
        public SrcStructure[] Sources
        {
            get {
                if (SrcMain==null) throw new Exception("SrcMain may not be null");

                if (SrcTest == null)
                {
                    return new SrcStructure[] { SrcMain };
                }
                else
                {
                    return new SrcStructure[] { SrcMain, SrcTest };
                }
            }
        }

        /// <summary>
        /// Path to the ProjectFolder
        /// </summary>
        /// <param name="targetProject"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public DirectoryInfo ProjectFolder
        {
            get
            {
                return projectFolder;
            }
        }

        public FileInfo PomFile
        {
            get {
                return new FileInfo(pomFilePath); 
            }
        }

        public PomXml PomXml
        {
            get
            {
                return new PomXml(pomFilePath);
            }
        }
    }
}

