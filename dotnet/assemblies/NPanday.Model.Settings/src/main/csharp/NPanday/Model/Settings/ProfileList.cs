using System;
using System.Collections.Generic;
using System.Text;
using NPanday.Model.Settings;

namespace NPanday.Model.Settings
{
    public class ProfileList : IEnumerable<Profile>
    {
        List<Profile> profiles;
      


        public ProfileList()
        {
            this.profiles = new List<Profile>();
        }

        public int Length
        {
            get
            {
                return profiles.Count;
            }
        }

        public Profile[] ToArray()
        {
            return profiles.ToArray();
        }
        public Profile this[int index]
        {
            get { return profiles[index]; }
            set { profiles[index] = value; }
        }

        /// <summary>
        /// Gets the repository object from the profile
        /// </summary>
        /// <param name="profile">The profile</param>
        /// <param name="url">The repository url</param>
        /// <returns>
        /// The repository object from the profile
        /// </returns>
        public Profile this[string id]
        {
            get
            {
                foreach (Profile pro in profiles)
                {
                    if (id.Equals(pro.id))
                    {
                        return pro;
                    }
                }
                return null;
            }
        }

        #region IEnumerable<Repository> Members

        public IEnumerator<Profile> GetEnumerator()
        {
    
            return profiles.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
       
            return profiles.GetEnumerator();
        }

        #endregion


        public void AddRange(IEnumerable<Profile> repos)
        {
            profiles.AddRange(repos);
        }

        public void Add(Profile repo)
        {
            profiles.Add(repo);
        }

        /// <summary>
        /// The list of profiles with isActive==true
        /// </summary>
        public ProfileList ActiveList
        {
            get
            {
                ProfileList pl = new ProfileList();
                foreach (Profile p in this)
                {
                    if (p.isActive) pl.Add(p);
                }
                return pl;
            }
        }

        /// <summary>
        /// The combined Repositories of the profiles contained in this ProfileList
        /// </summary>
        public RepositoryList Repositories
        {
            get
            {
                RepositoryList rl = new RepositoryList();
                foreach (Profile p in this)
                {
                    rl.AddRange(p.Repositories);
                }
                return rl;
            }
        }

    }
}
