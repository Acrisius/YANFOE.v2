﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegex.cs" company="The YANFOE Project">
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

namespace YANFOE.Settings.ConstSettings
{
    /// <summary>
    /// Standard regex used within YANFOE
    /// </summary>
    public static class DefaultRegex
    {
        /// <summary>
        /// Extract an hour int and a minute int.
        /// </summary>
        public const string HourMinute = @"(?<Hour>\d*)h\s(?<Minute>\d*)\w\w";

        /// <summary>
        /// Used to detect if a filename is in a TV show format, and extract series and episode numbers.
        /// </summary>
        public const string Tv = @"(?<![0-9])s{0,1}([0-9]{1,2})((?:(?:(e|\se)[0-9]+)+)|(?:(?:x[0-9]+)+))";
    }
}
