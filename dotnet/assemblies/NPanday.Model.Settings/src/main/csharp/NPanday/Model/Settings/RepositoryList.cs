using System;
using System.Collections.Generic;
using System.Text;
using NPanday.Model.Settings;

namespace NPanday.Model.Settings
{
    public class RepositoryList : IEnumerable<Repository>
    {
        List<Repository> repositories;
      
        public RepositoryList()
        {
            this.repositories = new List<Repository>();
        }

        public int Length
        {
            get
            {
                return repositories.Count;
            }
        }

        public Repository[] ToArray()
        {
            return repositories.ToArray();
        }
        public Repository this[int index]
        {
            get { return repositories[index]; }
            set { repositories[index] = value; }
        }

        /// <summary>
        /// Gets the repository object from the profile
        /// </summary>
        /// <param name="profile">The profile</param>
        /// <param name="url">The repository url</param>
        /// <returns>
        /// The repository object from the profile
        /// </returns>
        public Repository this[string url]
        {
            get
            {
                if (repositories != null)
                {
                    foreach (Repository repo in repositories)
                    {
                        if (url.Equals(repo.url))
                        {
                            return repo;
                        }
                    }
                }
                return null;
            }
        }

        public List<Repository> SnapshotRepositoriesOnly
        {
            get
            {
                List<Repository> snapshotList = new List<Repository>();
                foreach (Repository r in this.repositories)
                {
                    if (r.snapshots.enabled == true)
                    {
                        snapshotList.Add(r);
                    }
                }
                return snapshotList;
            }
        }

        public List<Repository> ReleaseRepositoriesOnly
        {
            get
            {
                List<Repository> releaseList = new List<Repository>();
                foreach (Repository r in this.repositories)
                {
                    if (r.releases.enabled == true)
                    {
                        releaseList.Add(r);
                    }
                }
                return releaseList;
            }
        }

        #region IEnumerable<Repository> Members

        public IEnumerator<Repository> GetEnumerator()
        {
            //for (int i = 0; i < repositories.Count; i++)
            //{
            //    yield return repositories[i];
            //}
            return repositories.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            //for (int i = 0; i < repositories.Count; i++)
            //{
            //    yield return repositories[i];
            //}
            return repositories.GetEnumerator();
        }

        #endregion


           
      
      

        
        /// <summary>
        /// Add repository to profile
        /// </summary>
        /// <param name="profile">The profile</param>
        /// <param name="url">The repository url to be added</param>
        /// <param name="isReleaseEnabled">True if release is enabled</param>
        /// <param name="isSnapshotEnabled">True if snapshot is enabled</param>
        /// <param name="settings">The settings</param>
        /// <returns>The repository</returns>
        public Repository Add(string url, bool isReleaseEnabled, bool isSnapshotEnabled)
        {
            Repository repository = this[url];

            if (repository == null)
            {
                repository = new Repository();
                repository.url = url;                    
                repositories.Insert(0, repository);
            }
            UpdateRepository(repository, isReleaseEnabled, isSnapshotEnabled);
            return repository;
        }

        public void AddRange(IEnumerable<Repository> repos)
        {
            repositories.AddRange(repos);
        }

        public void Add(Repository repos)
        {
            repositories.Add(repos);
        }
        
        /// <summary>
        /// Update repository information
        /// </summary>
        /// <param name="repository">The repository</param>
        /// <param name="isReleaseEnabled">True if release is enabled</param>
        /// <param name="isSnapshotEnabled">True if snapshot is enabled</param>
        public void UpdateRepository(Repository repository, bool isReleaseEnabled, bool isSnapshotEnabled)
        {
            RepositoryPolicy releasesPolicy = new RepositoryPolicy();
            RepositoryPolicy snapshotsPolicy = new RepositoryPolicy();
            releasesPolicy.enabled = isReleaseEnabled;
            snapshotsPolicy.enabled = isSnapshotEnabled;
            repository.releases = releasesPolicy;
            repository.snapshots = snapshotsPolicy;
            if (repository.id == null)
            {
                repository.id = generateRepositoryId();
            }
        }

        private string generateRepositoryId()
        {
            int ctr = 0;
            if (repositories != null)
            {
                foreach (Repository repo in repositories)
                {
                    if (repo.id != null && repo.id.StartsWith("npanday.repo."))
                    {
                        int index = int.Parse(repo.id.Substring(13));

                        if (index >= ctr)
                        {
                            ctr = index + 1;
                        }
                    }
                }
            }
            return "npanday.repo." + ctr;
        }

        #region RemoveRepositoryFromProfile
        /// <summary>
        /// Remove repository from profile
        /// </summary>
        /// <param name="profile">The profile</param>
        /// <param name="repository">The repository to remove</param>
        public void RemoveRepositoryFromProfile(Repository repository)
        {
            repositories.Remove(repository);
            //List<Repository> repos = new List<Repository>();
            //repos.AddRange(this.repositories);
            //repos.Remove(repository);
            //this.repositories = repos.ToArray();
        }
        #endregion

    }
}
