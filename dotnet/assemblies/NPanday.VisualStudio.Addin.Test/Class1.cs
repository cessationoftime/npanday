using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NPanday.Artifact;
using System.IO;
using NPanday.Model.Settings;

namespace NPanday.VisualStudio.Addin.Test
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void testClassifier()
        {
          ArtifactRepository AR = new ArtifactRepository(new DirectoryInfo(SettingsUtil.GetLocalRepositoryPath()));
          Artifact.Artifact art = AR.GetArtifact(new FileInfo(@"C:\Users\cvanvranken\.m2\repository\castle\Castle.Core\2.5.0.0\Castle.Core-2.5.0.0-net4.0.dll"));
          Assert.That(art.Classifier == "net4.0");
        }
    }
}
