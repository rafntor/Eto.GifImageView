
# Eto.GifImageView

[![Build](https://github.com/rafntor/Eto.GifImageView/actions/workflows/build.yml/badge.svg)](https://github.com/rafntor/Eto.GifImageView/actions/workflows/build.yml)
[![NuGet](http://img.shields.io/nuget/v/Eto.GifImageView.svg)](https://www.nuget.org/packages/Eto.GifImageView/)
[![License](https://img.shields.io/github/license/rafntor/Eto.GifImageView)](LICENSE)

Provides an [**Eto.Forms**](https://github.com/picoe/Eto) control for displaying GIF's that
internally uses pure C# GIF decoding and animation.  
Based on https://github.com/AvaloniaUI/Avalonia.GIF;

Demo applications : https://nightly.link/rafntor/Eto.GifImageView/workflows/build/master

## Quickstart

Use NuGet to install [`Eto.GifImageView`](https://www.nuget.org/packages/Eto.GifImageView/), then add the following to your Form or Container:
```cs
   this.Content = new GifImageView { Image = GifImage.FromFile("image.gif") };
```

![](./Animation.gif)
