﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Allocine.cs" company="The YANFOE Project">
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

namespace YANFOE.Scrapers.Movie
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using BitFactory.Logging;

    using YANFOE.InternalApps.DownloadManager;
    using YANFOE.InternalApps.DownloadManager.Model;
    using YANFOE.InternalApps.Logs;
    using YANFOE.Scrapers.Movie.Interfaces;
    using YANFOE.Scrapers.Movie.Models.Search;
    using YANFOE.Tools;
    using YANFOE.Tools.Enums;
    using YANFOE.Tools.Extentions;
    using YANFOE.Tools.Models;

    /// <summary>
    /// The Allocine.fr scraper for YANFOE.v2
    /// </summary>
    public class Allocine : ScraperMovieBase, IMovieScraper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Allocine"/> class.
        /// </summary>
        public Allocine()
        {
            this.ScraperName = ScraperList.Allocine;

            this.Urls = new Dictionary<string, string>
                            {
                                { "main", "http://www.allocine.fr/film/fichefilm_gen_cfilm={0}.html" },
                                { "cast", "http://www.allocine.fr/film/casting_gen_cfilm={0}.html" },
                                { "poster", "http://www.allocine.fr/film/fichefilm-{0}/affiches/" }
                            };

            this.UrlHtmlCache = new Dictionary<string, string>();

            this.AvailableSearchMethod = new BindingList<ScrapeSearchMethod>();

            this.AvailableSearchMethod.AddRange(new[]
                                                    {
                                                        ScrapeSearchMethod.Bing,
                                                    });

            this.AvailableScrapeMethods = new BindingList<ScrapeFields>();

            this.AvailableScrapeMethods.AddRange(new[]
                                               {
                                                   ScrapeFields.Title,
                                                   ScrapeFields.OrigionalTitle,
                                                   ScrapeFields.Year,
                                                   ScrapeFields.Rating,
                                                   ScrapeFields.Director,
                                                   ScrapeFields.Plot,
                                                   ScrapeFields.Country,
                                                   ScrapeFields.Genre,
                                                   ScrapeFields.Cast,
                                                   ScrapeFields.ReleaseDate,
                                                   ScrapeFields.Runtime,
                                               });
        }

        /// <summary>
        /// Searches Bing for Applicable MovieUniqueId's
        /// </summary>
        /// <param name="query">A BingQuery object.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>[true/false] if an error occurred.</returns>
        public new bool SearchViaBing(Query query, int threadID, string logCatagory)
        {
            try
            {
                query.Results = Bing.SearchBing(
                    string.Format(CultureInfo.CurrentCulture, "site:http://www.allocine.fr {0}%20{1}", query.Title, query.Year),
                    "http://www.allocine.fr/film/fichefilm_gen_cfilm=",
                    threadID);

                return query.Results.Count > 0;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Title value
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Title value.</param>
        /// <param name="alternatives">The alternatives.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeTitle(string id, int threadID, out string output, out BindingList<string> alternatives, string logCatagory)
        {
            output = string.Empty;
            alternatives = new BindingList<string>();

            try
            {
                output = YRegex.Match(
                    @"<title>\t(?<title>.*?)\s\(\d\d\d\d\)\s-\sAlloCiné</title>",
                    this.GetHtml("main", threadID, id),
                    "title",
                    true);

                return !string.IsNullOrEmpty(output);
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Origional Title value
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Origional Title value.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeOrigionalTitle(string id, int threadID, out string output, string logCatagory)
        {
            output = string.Empty;

            try
            {
                output = YRegex.Match(
                    @"Titre\soriginal\s:\s<span\sclass=""purehtml""><em>(?<OrigionalTitle>.+?)</em></span>",
                    this.GetHtml("main", threadID, id),
                    "OrigionalTitle",
                    true);

                return !string.IsNullOrEmpty(output);
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Year value
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Year value.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeYear(string id, int threadID, out int output, string logCatagory)
        {
            output = -1;

            try
            {
                var match = Regex.Match(
                    this.GetHtml("main", threadID, id),
                    @"\x28(?<year>\d{4})\x29");

                var success = match.Success;

                if (success)
                {
                    success = int.TryParse(match.Groups["year"].Value, out output);
                }
                else
                {
                    output = -1;
                }

                return success;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Rating value
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Reting value.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeRating(string id, int threadID, out double output, string logCatagory)
        {
            try
            {
                var ratingMatch = Regex.Matches(
                    this.GetHtml("main", threadID, id),
                    @"</a><span\sclass=""moreinfo"">\x28(?<rating>.*?)\x29</span>",
                    RegexOptions.IgnoreCase);

                var rating = "0.0";

                if (ratingMatch.Count > 1)
                {
                    if (ratingMatch[1].Success)
                    {
                        rating = ratingMatch[1].Groups["rating"].Value;
                    }
                }
                else
                {
                    if (ratingMatch.Count > 0)
                    {
                        if (ratingMatch[0].Success)
                        {
                            rating = ratingMatch[0].Groups["rating"].Value;
                        }
                    }
                }

                output = !string.IsNullOrEmpty(rating) ? Maths.ProcessDouble(rating, 2.5).ToDouble() : 0;

                return true;
            }
            catch
            {
                output = 0;
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Director value
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Director value.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeDirector(string id, int threadID, out BindingList<PersonModel> output, string logCatagory)
        {
            output = new BindingList<PersonModel>();

            try
            {
                output = new BindingList<PersonModel>();

                var director = YRegex.Match(
                    @"Un film de (?<director>.*?) avec",
                    this.GetHtml("main", threadID, id),
                    "director",
                    true);

                output.Add(new PersonModel(director));

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Plot value
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Plot value.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <param name="returnShort">if set to <c>true</c> short plot is returned if available.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapePlot(string id, int threadID, out string output, string logCatagory, bool returnShort)
        {
            output = string.Empty;

            try
            {
                output =
                    YRegex.Match(
                        ">Synopsis : </span>(?<plot>.*?).</p>",
                        this.GetHtml("main", threadID, id),
                        "plot",
                        true);

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Country copllection
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Country collection.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeCountry(string id, int threadID, out BindingList<string> output, string logCatagory)
        {
            output = new BindingList<string>();

            try
            {
                var country = YRegex.Match(
                    @"<a\shref=""/film/tous/pays-\d{4}/"">(?<country>.*?)</a>",
                    this.GetHtml("main", threadID, id),
                    "country",
                    true);

                if (!string.IsNullOrEmpty(country))
                {
                    output.Add(country);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Genre collection
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Year collection.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeGenre(string id, int threadID, out BindingList<string> output, string logCatagory)
        {
            output = new BindingList<string>();

            try
            {
                output = YRegex.MatchesToList(
                    @"<a\shref=""/film/tous/genre-\d{5}/"">(?<genre>.*?)</a>", 
                    this.GetHtml("main", threadID, id), 
                    "genre");

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the Cast collection.
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped Cast value.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeCast(string id, int threadID, out BindingList<PersonModel> output, string logCatagory)
        {
            output = new BindingList<PersonModel>();
            
            try
            {
                var html = Downloader.ProcessDownload(this.GetHtml("cast", threadID, id), DownloadType.Html, Section.Movies).RemoveCharacterReturn();

                var castBlock =
                    Regex.Match(html, "<h2>Acteurs(?<body>.*?)Production", RegexOptions.IgnoreCase).Groups[0].Value;

                var m = Regex.Matches(
                    castBlock,
                    @"/personne/fichepersonne_gen_cpersonne=\d*\.html"">\t\t\t*?<img src='(?<image>.*?)'.*?title='(?<actor>.*?)'.*?Rôle : (?<role>.*?)\t",
                    RegexOptions.IgnoreCase);

                const string EmptyImage = "http://images.allocine.fr/r_60_80/commons/emptymedia/AffichetteAllocine.gif";

                foreach (Match match in m)
                {
                    var actor = new PersonModel(match.Groups["actor"].Value, match.Groups["role"].Value);

                    if (match.Groups["image"].Value != EmptyImage)
                    {
                        actor.ImageUrl = match.Groups["image"].Value;
                    }

                    output.Add(actor);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the release date value
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped release date value.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeReleaseDate(string id, int threadID, out DateTime output, string logCatagory)
        {
            output = new DateTime();

            try
            {
                var match = Regex.Match(
                     this.GetHtml("main", threadID, id),
                     @"week=(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})"">(?<releasedate>.*?)</a>");

                if (match.Success)
                {
                    int day;
                    int month;
                    int year;

                    int.TryParse(match.Groups["day"].Value, out day);
                    int.TryParse(match.Groups["month"].Value, out month);
                    int.TryParse(match.Groups["year"].Value, out year);

                    DateTime date;

                    var result = DateTime.TryParse(string.Format("{0}-{1}-{2}", year, month, day), out date);

                    if (result)
                    {
                        output = date;
                    }

                    output = new DateTime(year, month, day);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the runtime value
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped runtime value.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapeRuntime(string id, int threadID, out int output, string logCatagory)
        {
            output = -1;

            try
            {
                int hour;
                int minute;

                var result1 = int.TryParse(
                        YRegex.Match(
                            @"Durée\s:\s\s(?<hour>\d{2})h(?<minute>\d{2})min",
                            this.GetHtml("main", threadID, id),
                            "hour"),
                        out hour);

                var result2 = int.TryParse(
                        YRegex.Match(
                            @"Durée\s:\s\s(?<hour>\d{2})h(?<minute>\d{2})min",
                            this.GetHtml("main", threadID, id),
                            "minute"),
                        out minute);

                if (result1 && result2)
                {
                    var runtime = (hour * 60) + minute;

                    output = runtime;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Scrapes the poster image collection.
        /// </summary>
        /// <param name="id">The MovieUniqueId for the scraper.</param>
        /// <param name="threadID">The thread MovieUniqueId.</param>
        /// <param name="output">The scraped poster image collection.</param>
        /// <param name="logCatagory">The log catagory.</param>
        /// <returns>Scrape succeeded [true/false]</returns>
        public new bool ScrapePoster(string id, int threadID, out BindingList<ImageDetailsModel> output, string logCatagory)
        {
            output = new BindingList<ImageDetailsModel>();

            try
            {
                var downloadHtml = this.GetHtml("poster", threadID, id);

                var regexString = string.Format(@"/film/fichefilm-{0}/affiches/detail/\x3Fcmediafile=(?<imageid>\d*)", id);

                var regexFindImages = Regex.Matches(
                    downloadHtml,
                    regexString,
                    RegexOptions.IgnoreCase);

                foreach (Match match in regexFindImages)
                {
                    var imageWebPage = Downloader.ProcessDownload(string.Format("http://www.allocine.fr/film/fichefilm-{0}/affiches/detail/?cmediafile={1}", id, match.Groups["imageid"].Value), DownloadType.Html, Section.Movies).RemoveCharacterReturn();

                    var regexImageUrl = Regex.Match(
                        imageWebPage,
                        @"<img\ssrc=""(?<imageurl>http://images.allocine.fr/r_760_x.*.jpg?)",
                        RegexOptions.IgnoreCase);

                    if (regexImageUrl.Success)
                    {
                        output.Add(new ImageDetailsModel { UriFull = new Uri(regexImageUrl.Groups["imageurl"].Value) });
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteToLog(LogSeverity.Error, threadID, logCatagory, ex.Message);
                return false;
            }
        }
    }
}
