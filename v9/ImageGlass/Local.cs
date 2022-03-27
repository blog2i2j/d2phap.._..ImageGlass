﻿/*
ImageGlass Project - Image viewer for Windows
Copyright (C) 2010 - 2022 DUONG DIEU PHAP
Project homepage: https://imageglass.org

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using ImageGlass.Base;
using ImageGlass.Base.Photoing.Codecs;
using ImageGlass.Base.Services;
using ImageGlass.Settings;

namespace ImageGlass;

internal class Local
{
    #region Public events

    /// <summary>
    /// Occurs when <see cref="Images"/> is loaded.
    /// </summary>
    public static event EventHandler? OnImageListLoaded;

    /// <summary>
    /// Occurs when the requested image is being loaded.
    /// </summary>
    public static event ImageLoadingHandler? OnImageLoading;
    public delegate void ImageLoadingHandler(ImageLoadingEventArgs e);

    /// <summary>
    /// Occurs when the requested image is loaded.
    /// </summary>
    public static event ImageLoadedHandler? OnImageLoaded;
    public delegate void ImageLoadedHandler(ImageLoadedEventArgs e);


    /// <summary>
    /// Raise <see cref="OnImageListLoaded"/> event.
    /// </summary>
    public static void RaiseImageListLoadedEvent()
    {
        OnImageListLoaded?.Invoke(null, EventArgs.Empty);
    }


    /// <summary>
    /// Raise <see cref="OnImageLoading"/> event.
    /// </summary>
    public static void RaiseImageLoadingEvent(ImageLoadingEventArgs e)
    {
        OnImageLoading?.Invoke(e);
    }


    /// <summary>
    /// Raise <see cref="OnImageLoaded"/> event.
    /// </summary>
    public static void RaiseImageLoadedEvent(ImageLoadedEventArgs e)
    {
        OnImageLoaded?.Invoke(e);
    }


    #endregion


    #region Public properties

    public static IgMetadata? Metadata { get; set; }

    /// <summary>
    /// Gets, sets app state
    /// </summary>
    public static bool IsBusy { get; set; } = false;

    /// <summary>
    /// Gets, sets images list
    /// </summary>
    public static ImageBooster Images { get; set; } = new(Config.Codec);

    /// <summary>
    /// Gets, sets index of the viewing image
    /// </summary>
    public static int CurrentIndex { get; set; } = -1;

    /// <summary>
    /// <para>The current "initial" path (file or dir) we're viewing. Used when the user changes the sort settings: we need to rebuild the image list, but otherwise we don't know what image/folder we started with.</para>
    /// <para>Here's what happened: I opened a folder with subfolders (child folders enabled). I was going through the images, and decided I wanted to change the sort order. Since the _current_ image was in a sub-folder, after a rescan of the image list, only the _sub_-folders images were re-loaded!</para>
    /// <para>But if we reload the list using the original image, then the original folder's images, and the sub-folders, are reloaded.</para>
    /// </summary>
    public static string InitialInputPath { get; set; } = "";

    /// <summary>
    /// The 'current' image sorting order. A reconciliation between the user's Settings selection and the sorting order from Windows Explorer, to be used to sort the active image list.
    /// </summary>
    public static ImageOrderBy ActiveImageLoadingOrder { get; set; }

    /// <summary>
    /// The 'current' image sorting direction. A reconciliation between the user's Settings selection and the sorting direction from Windows Explorer, to be used to sort the active image list.
    /// </summary>
    public static ImageOrderType ActiveImageLoadingOrderType { get; set; }

    /// <summary>
    /// Gets, sets color channel of image
    /// </summary>
    public static ColorChannel ImageChannel { get; set; } = ColorChannel.All;

    /// <summary>
    /// Gets, sets value if image data was modified
    /// </summary>
    public static string ImageModifiedPath { get; set; } = "";

    /// <summary>
    /// Gets, sets value indicating whether the viewing image is memory data (clipboard / screenshot,...) or not
    /// </summary>
    public static bool IsTempMemoryData { get; set; } = false;

    #endregion


    #region Public Functions

    public static void InitImageList(IEnumerable<string>? list = null)
    {
        Images.Dispose();
        Images = new(Config.Codec, list)
        {
            MaxQueue = Config.ImageBoosterCachedCount,
            ImageChannel = ImageChannel,
        };
    }

    #endregion

}