package NPanday.Plugin.Addin;

import org.apache.npanday.plugins.FieldAnnotation;

/**
 * @phase package
 * @goal package
 */
public class AutomationExtensibilityMojo
    extends org.apache.npanday.plugins.AbstractMojo
{
       /**
        * @parameter expression = "${settings.localRepository}"
        */
        @FieldAnnotation()
        public java.lang.String repository;

       /**
        * @parameter expression = "${project}"
        */
        @FieldAnnotation()
        public org.apache.maven.project.MavenProject mavenProject;

       /**
        * @parameter expression = "${project}"
        */
        private org.apache.maven.project.MavenProject project;

       /**
        * @parameter expression = "${settings.localRepository}"
        */
        private String localRepository;

       /**
        * @parameter expression = "${vendor}"
        */
        private String vendor;

       /**
        * @parameter expression = "${vendorVersion}"
        */
        private String vendorVersion;

       /**
        * @parameter expression = "${frameworkVersion}"
        */
        private String frameworkVersion;

       /**
        * @component
        */
        private npanday.executable.NetExecutableFactory netExecutableFactory;

       /**
        * @component
        */
        private org.apache.npanday.plugins.PluginContext pluginContext;

        public String getMojoArtifactId()
        {
            return "NPanday.Plugin.Addin";
        }

        public String getMojoGroupId()
        {
            return "org.apache.npanday.plugins";
        }

        public String getClassName()
        {
            return "NPanday.Plugin.Addin.AutomationExtensibilityMojo";
        }

        public org.apache.npanday.plugins.PluginContext getNetPluginContext()
        {
            return pluginContext;
        }

        public npanday.executable.NetExecutableFactory getNetExecutableFactory()
        {
            return netExecutableFactory;
        }

        public org.apache.maven.project.MavenProject getMavenProject()
        {
            return project;
        }

        public String getLocalRepository()
        {
            return localRepository;
        }

        public String getVendorVersion()
        {
            return vendorVersion;
        }

        public String getVendor()
        {
            return vendor;
        }

        public String getFrameworkVersion()
        {
            return frameworkVersion;
        }

}
