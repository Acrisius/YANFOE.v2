﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Season.cs" company="The YANFOE Project">
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

namespace YANFOE.Models.TvModels.Show
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using YANFOE.Factories;
    using YANFOE.Properties;
    using YANFOE.Tools.Importing;
    using YANFOE.Tools.Models;

    /// <summary>
    /// The TV season model.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class Season : ModelBase
    {
        #region Constants and Fields

        /// <summary>
        /// The banner path.
        /// </summary>
        private string bannerPath;

        /// <summary>
        /// The banner.
        /// </summary>
        private string bannerUrl;

        /// <summary>
        /// The changed banner.
        /// </summary>
        private bool changedBanner;

        /// <summary>
        /// The changed fanart.
        /// </summary>
        private bool changedFanart;

        /// <summary>
        /// The changed poster.
        /// </summary>
        private bool changedPoster;

        /// <summary>
        /// The fanart path.
        /// </summary>
        private string fanartPath;

        /// <summary>
        /// The fanart.
        /// </summary>
        private string fanartUrl;

        /// <summary>
        /// The season guid.
        /// </summary>
        private string guid;

        /// <summary>
        /// The poster path.
        /// </summary>
        private string posterPath;

        /// <summary>
        /// The poster.
        /// </summary>
        private string posterUrl;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Season"/> class.
        /// </summary>
        public Season()
        {
            this.guid = this.guid = System.Guid.NewGuid().ToString();

            this.Episodes = new List<Episode>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets BannerPath.
        /// </summary>
        public string BannerPath
        {
            get
            {
                return this.bannerPath;
            }

            set
            {
                if (this.bannerPath != value)
                {
                    this.bannerPath = value;
                    this.OnPropertyChanged("BannerPath");
                    this.ChangedBanner = true;

                    if (!string.IsNullOrEmpty(this.bannerPath))
                    {
                        this.bannerUrl = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets Banner.
        /// </summary>
        public string BannerUrl
        {
            get
            {
                return this.bannerUrl;
            }

            set
            {
                if (this.bannerUrl != value)
                {
                    this.bannerUrl = value;
                    this.OnPropertyChanged("BannerUrl");
                    this.ChangedBanner = true;

                    if (!string.IsNullOrEmpty(this.bannerUrl))
                    {
                        this.bannerPath = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ChangedBanner.
        /// </summary>
        public bool ChangedBanner
        {
            get
            {
                return this.changedBanner;
            }

            set
            {
                if (this.changedBanner != value)
                {
                    this.changedBanner = value;
                    this.OnPropertyChanged("ChangedBanner");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ChangedFanart.
        /// </summary>
        public bool ChangedFanart
        {
            get
            {
                return this.changedFanart;
            }

            set
            {
                if (this.changedFanart != value)
                {
                    this.changedFanart = value;
                    this.OnPropertyChanged("ChangedFanart");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ChangedPoster.
        /// </summary>
        public bool ChangedPoster
        {
            get
            {
                return this.changedPoster;
            }

            set
            {
                if (this.changedPoster != value)
                {
                    this.changedPoster = value;
                    this.OnPropertyChanged("ChangedPoster");
                }
            }
        }

        /// <summary>
        /// Gets or sets the episodes object
        /// </summary>
        /// <value>
        /// The episodes object
        /// </value>
        public List<Episode> Episodes { get; set; }

        /// <summary>
        /// Gets or sets FanartPath.
        /// </summary>
        public string FanartPath
        {
            get
            {
                return this.fanartPath;
            }

            set
            {
                if (this.fanartPath != value)
                {
                    this.fanartPath = value;
                    this.OnPropertyChanged("FanartPath");
                    this.ChangedFanart = true;

                    if (!string.IsNullOrEmpty(this.fanartPath))
                    {
                        this.fanartUrl = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets Fanart.
        /// </summary>
        public string FanartUrl
        {
            get
            {
                return this.fanartUrl;
            }

            set
            {
                if (this.fanartUrl != value)
                {
                    this.fanartUrl = value;
                    this.OnPropertyChanged("FanartUrl");
                    this.ChangedFanart = true;

                    if (!string.IsNullOrEmpty(this.fanartUrl))
                    {
                        this.fanartPath = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID value
        /// </value>
        public string Guid
        {
            get
            {
                return this.guid;
            }

            set
            {
                this.guid = value;
            }
        }

        private bool isLocked;

        /// <summary>
        /// Gets or sets a value indicating whether IsLocked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is locked; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocked
        {
            get
            {
                return this.isLocked;
            }

            set
            {
                this.isLocked = value;
                this.OnPropertyChanged("IsLocked");
                this.OnPropertyChanged("LockedImage");
            }
        }

        /// <summary>
        /// Gets the locked image.
        /// </summary>
        public Image LockedImage
        {
            get
            {
                return this.IsLocked ? Resources.locked32 : Resources.unlock32;
            }
        }

        /// <summary>
        /// Gets or sets PosterPath.
        /// </summary>
        public string PosterPath
        {
            get
            {
                return this.posterPath;
            }

            set
            {
                if (this.posterPath != value)
                {
                    this.posterPath = value;
                    this.OnPropertyChanged("PosterPath");
                    this.ChangedPoster = true;

                    if (!string.IsNullOrEmpty(this.PosterPath))
                    {
                        this.posterUrl = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets Poster.
        /// </summary>
        public string PosterUrl
        {
            get
            {
                return this.posterUrl;
            }

            set
            {
                if (this.posterUrl != value)
                {
                    this.posterUrl = value;
                    this.OnPropertyChanged("PosterUrl");
                    this.ChangedPoster = true;

                    if (!string.IsNullOrEmpty(this.posterUrl))
                    {
                        this.posterPath = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets SeasonNumber.
        /// </summary>
        public int SeasonNumber { get; set; }

        /// <summary>
        /// Gets Status.
        /// </summary>
        [JsonIgnore]
        public Image Status
        {
            get
            {
                return this.HasMissingEpisodes() ? Resources.promo_red_faded16 : Resources.promo_green_faded16;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether [contains changed episodes].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [contains changed episodes]; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsChangedEpisodes()
        {
            return this.Episodes.Where(episode => episode.ChangedText || episode.ChangedScreenshot).Any();
        }

        /// <summary>
        /// Determines whether [contains episodes with files].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [contains episodes with files]; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsEpisodesWithFiles()
        {
            return this.Episodes.Any(episode => !string.IsNullOrEmpty(episode.FilePath.FileNameAndPath));
        }

        /// <summary>
        /// Counts the missing episodes.
        /// </summary>
        /// <returns>
        /// Total missing episodes
        /// </returns>
        public int CountMissingEpisodes()
        {
            return this.Episodes.Count(e => !File.Exists(e.FilePath.FileNameAndPath));
        }

        /// <summary>
        /// Gets the first episode in the season that contains a filepath.
        /// </summary>
        /// <returns>
        /// The first episode in the season that contains a filepath
        /// </returns>
        public string GetFirstEpisode()
        {
            string path = string.Empty;

            foreach (Episode episode in this.Episodes)
            {
                if (!string.IsNullOrEmpty(episode.FilePath.FileNameAndPath))
                {
                    if (File.Exists(episode.FilePath.FileNameAndPath))
                    {
                        return episode.FilePath.FileNameAndPath;
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Gets the name of the season.
        /// </summary>
        /// <returns>
        /// The name of the season.
        /// </returns>
        public string GetSeasonName()
        {
            foreach (Episode episode in this.Episodes)
            {
                if (!string.IsNullOrEmpty(episode.FilePath.FileNameAndPath))
                {
                    if (File.Exists(episode.FilePath.FileNameAndPath))
                    {
                        string[] segSplit = episode.FilePath.FileNameAndPath.Split(new[] { '\\' });

                        return segSplit[segSplit.Length - 2];
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the season path.
        /// </summary>
        /// <returns>
        /// The season path.
        /// </returns>
        public string GetSeasonPath()
        {
            string path = string.Empty;

            foreach (Episode episode in this.Episodes)
            {
                if (!string.IsNullOrEmpty(episode.FilePath.FileNameAndPath))
                {
                    if (File.Exists(episode.FilePath.FileNameAndPath))
                    {
                        string[] segSplit = episode.FilePath.FileNameAndPath.Split(new[] { '\\' });

                        if (MovieNaming.IsDVD(episode.FilePath.FileNameAndPath))
                        {
                            path = string.Join(@"\", segSplit, 0, segSplit.Length - 3);
                        }
                        else if (MovieNaming.IsBluRay(episode.FilePath.FileNameAndPath))
                        {
                            path = string.Join(@"\", segSplit, 0, segSplit.Length - 4);
                        }
                        else
                        {
                            path = string.Join(@"\", segSplit, 0, segSplit.Length - 1);
                        }

                        return path;
                    }
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                path = string.Format(
                    "{0}{1}Season {2}", 
                    TvDBFactory.CurrentSeries.GetSeriesPath(), 
                    Path.DirectorySeparatorChar, 
                    this.SeasonNumber);
            }

            return path;
        }

        /// <summary>
        /// Gets the series the season belongs to.
        /// </summary>
        /// <returns>
        /// The series the season belongs to.
        /// </returns>
        public Series GetSeries()
        {
            return
                (from series in TvDBFactory.TvDatabase
                 from season in series.Value.Seasons
                 where season.Value == this
                 select series.Value).FirstOrDefault();
        }

        /// <summary>
        /// Determines whether [has episode with path].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [has episode with path]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasEpisodeWithPath()
        {
            return this.Episodes.Any(episode => !string.IsNullOrEmpty(episode.FilePath.FileNameAndPath));
        }

        /// <summary>
        /// Determines whether [has missing episodes].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [has missing episodes]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMissingEpisodes()
        {
            bool missingEpisodes = false;

            foreach (Episode e in this.Episodes)
            {
                if (e.FilePath == null)
                {
                    e.FilePath = new FilePath();
                }

                if (string.IsNullOrEmpty(e.FilePath.FileNameAndPath))
                {
                    missingEpisodes = true;
                }
            }

            return missingEpisodes;
        }

        #endregion
    }
}