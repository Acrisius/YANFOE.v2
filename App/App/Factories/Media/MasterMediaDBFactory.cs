﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MasterMediaDBFactory.cs" company="The YANFOE Project">
//   Copyright 2011 The YANFOE Project
// </copyright>
// <license>
//   This software is licensed under a Creative Commons License
//   Attribution-NonCommercial-ShareAlike 3.0 Unported (CC BY-NC-SA 3.0) 
//   http://creativecommons.org/licenses/by-nc-sa/3.0/
//   See this page: http://www.yanfoe.com/license
//   For any reuse or distribution, you must make clear to others the 
//   license terms of this work.  
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace YANFOE.Factories.Media
{
    using System.ComponentModel;
    using System.Linq;

    using YANFOE.Models.GeneralModels.AssociatedFiles;
    using YANFOE.Models.MovieModels;
    using YANFOE.Models.TvModels.Show;

    /// <summary>
    /// The master media db factory.
    /// </summary>
    public static class MasterMediaDBFactory
    {
        #region Constants and Fields

        /// <summary>
        /// The master movie media database.
        /// </summary>
        private static BindingList<MediaModel> masterMovieMediaDatabase;

        /// <summary>
        /// The master tv media database.
        /// </summary>
        private static BindingList<string> masterTvMediaDatabase;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="MasterMediaDBFactory"/> class.
        /// </summary>
        static MasterMediaDBFactory()
        {
            masterMovieMediaDatabase = new BindingList<MediaModel>();
            masterTvMediaDatabase = new BindingList<string>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets MasterMovieMediaDatabase.
        /// </summary>
        public static BindingList<MediaModel> MasterMovieMediaDatabase
        {
            get
            {
                return masterMovieMediaDatabase;
            }

            set
            {
                masterMovieMediaDatabase = value;
            }
        }

        /// <summary>
        /// Gets or sets MasterTvMediaDatabase.
        /// </summary>
        public static BindingList<string> MasterTvMediaDatabase
        {
            get
            {
                return masterTvMediaDatabase;
            }

            set
            {
                masterTvMediaDatabase = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Movies the database contains.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns>
        /// The movie database contains.
        /// </returns>
        public static bool MovieDatabaseContains(string path)
        {
            MediaModel result = (from m in masterMovieMediaDatabase where m.FilePath == path select m).SingleOrDefault();

            return result != null;
        }

        /// <summary>
        /// The populate master movie media database.
        /// </summary>
        public static void PopulateMasterMovieMediaDatabase()
        {
            var bgw = new BackgroundWorker();

            bgw.DoWork += BuildMasterMovieMediaDatabase;
            bgw.RunWorkerAsync();
        }

        /// <summary>
        /// The populate master tv media database.
        /// </summary>
        public static void PopulateMasterTvMediaDatabase()
        {
            var bgw = new BackgroundWorker();

            bgw.DoWork += BuildMasterTvMediaDatabase;
            bgw.RunWorkerAsync();
        }

        /// <summary>
        /// The tv database contains.
        /// </summary>
        /// <param name="pathAndFileName">The path and file name.</param>
        /// <returns>
        /// True/False - Database contains filename and path.
        /// </returns>
        public static bool TvDatabaseContains(string pathAndFileName)
        {
            return masterTvMediaDatabase.Contains(pathAndFileName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the master movie media database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private static void BuildMasterMovieMediaDatabase(object sender, DoWorkEventArgs e)
        {
            masterMovieMediaDatabase = new BindingList<MediaModel>();

            foreach (MovieModel m in MovieDBFactory.MovieDatabase)
            {
                foreach (MediaModel f in m.AssociatedFiles.Media)
                {
                    masterMovieMediaDatabase.Add(f);
                }
            }
        }

        /// <summary>
        /// Builds the master tv media database.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private static void BuildMasterTvMediaDatabase(object sender, DoWorkEventArgs e)
        {
            masterTvMediaDatabase = new BindingList<string>();

            foreach (var series in TvDBFactory.TvDatabase)
            {
                foreach (var season in series.Value.Seasons)
                {
                    foreach (Episode episode in season.Value.Episodes)
                    {
                        string filePath = episode.CurrentFilenameAndPath;

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            // var check = (from m in masterTvMediaDatabase where m == filePath select m).SingleOrDefault();
                            if (!masterTvMediaDatabase.Contains(filePath))
                            {
                                masterTvMediaDatabase.Add(filePath);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}