Kraken.io .Net client
=============

The kraken-net client interacts with the Kraken.io REST API allowing you to utilize Kraken’s features using a .NET interface. 

* [Getting Started](#getting-started)
* [Installation](#installation)
* [Downloading Images](#downloading-images)
* [How To Use](#how-to-use)
  * [Authentication](#authentication)
  * [Providing your images](#providing-your-images)
  * [Image URL](#image-url)
  * [Image Upload](#image-upload)
* [Wait and Callback URL](#wait-and-callback-url)
  * [Wait Option](#wait-option)
  * [Callback URL](#callback-url)
* [Lossy Optimization](#lossy-optimization)
  * [PNG Images](#png-images)
  * [JPEG Images](#jpeg-images)
* [WebP Compression](#webp-compression)
* [Image Type Conversion](#image-type-conversion)
* [Preserving Metadata](#preserving-metadata)
* [External Storage](#external-storage)
  * [Azure Blob](#azure-blob)
  * [Amazon S3](#amazon-s3)
* [Automatic Image Orientation](#automatic-image-orientation)
* [Chroma Subsampling](#chroma-subsampling)
* [Image Resizing](#image-resizing)
* [API Sandbox](#api-sandbox)
* [Account Status](#account-status)

## Getting Started

First you need to sign up for the [Kraken API](http://kraken.io/plans/) and obtain your unique **API Key** and **API Secret**. You will find both under [API Credentials](http://kraken.io/account/api-credentials). Once you have set up your account, you can start using the Kraken.io .Net client.

## Installation

To install kraken-net, run the following command in the Package Manager Console
Install-Package kraken-net

## Downloading Images

Remember - never link to optimized images offered to download. You have to download them first, and then replace them in your websites or applications. Due to security reasons optimized images are available on our servers **for one hour** only.

## How to use

### Authentication

The first step is to authenticate to Kraken API by providing your unique API Key and API Secret while creating a new client connection.

```C#
using Kraken;
using Kraken.Http;

var connection = Connection.Create("key", "secret");
```

### Providing your images

You can optimize your images by providing the URL of the image you want to optimize or upload the image directly instead. Just keep in mind that the image URL must be accessible for Kraken.

The first option (image URL) is great for images that are already in production or any other place on the Internet. The second one (direct upload) is ideal for your deployment process, build script or the on-the-fly processing of your user's uploads where you don't have the images available online yet.

### Image URL

**Request:**

```C#
var response = client.OptimizeWait(
    new Uri("http://image-url.com/file.jpg")
);
```

### Image Upload

Kraken allows you to easily upload your images as can be seen within the examples below. Use the full path option or provide a byte array and a name.

**Using a local file path using default compression settings:**

```C#
var response = client.OptimizeWait("c:\your-image-location-on-disk.png");

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result..Body.KrakedUrl;
}
```

**Using a local file path using custom compression settings:**

```C#
var response = client.OptimizeWait("c:\your-image-location-on-disk.png",
    new OptimizeUploadWaitRequest()
    {
       // your compression settings
    }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

**Callback using a local file path using default compression settings:** 

```C#
var response = client.Optimize("c:\your-image-location-on-disk.png", 
    new Uri("http://awesome-website.com/kraken_results");

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var id = response.Result.Body.Id;
}
```

**Callback using a local file path using custom compression settings:** 

```C#
var response = client.Optimize("c:\your-image-location-on-disk.png",
    new OptimizeUploadRequest(new Uri("http://awesome-website.com/kraken_results"))
    {
        // your compression settings
    }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var id = response.Result.Body.Id;
}
```

## Wait and Callback URL

Kraken-net gives you two options for fetching optimization results. Using the `OptimizeWait` request the results will be returned in the response. Using the `Optimize` request the results will be posted to the URL specified in your request.

### Wait option

Using the `OptimizeWait` request, the connection will be held open until the image has been optimized. Once this is done you will receive a `OptimizeWaitResult` object containing your optimization results. 

**Request:**

```C#
var response = client.OptimizeWait(
    new Uri("http://image-url.com/file.jpg")
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

### Callback URL

Using the `OptimizeWait` request the connection will be terminated immediately and a unique `id` will be returned in the `OptimizeWaitResult` object. After the optimization is over Kraken will POST a message to the `callbackUrl` specified in your request. The ID in the response will reflect the ID in the results posted to your Callback URL.

**Request:**

```C#
var response = client.Optimize(
    new Uri("http://image-url.com/file.jpg"),
    new Uri("http://awesome-website.com/kraken_results")
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var id = response.Result.Body.Id;
}
```

**Results posted to the Callback URL:**

````js
{
    "id": "18fede37617a787649c3f60b9f1f280d"
    "success": true,
    "file_name": "file.jpg",
    "original_size": 324520,
    "kraked_size": 165358,
    "saved_bytes": 159162,
    "kraked_url": "http://dl.kraken.io/18fede37617a787649c3f60b9f1f280d/file.jpg"
}
````

## Lossy Optimization

When you decide to sacrifice just a small amount of image quality (usually unnoticeable to the human eye), you will be able to save up to 90% of the initial file weight. Lossy optimization will give you outstanding results with just a fraction of image quality loss.

To use lossy optimizations simply set `Lossy = true` in your request:

```C#
var response = client.OptimizeWait(
    new OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"))
    {
        Lossy = true,
    });

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

### PNG Images
PNG images will be converted from 24-bit to paletted 8-bit with full alpha channel. This process is called PNG quantization in RGBA format and means the amount of colours used in an image will be reduced to 256 while maintaining all information about alpha transparency.

### JPEG Images
For lossy JPEG optimizations Kraken will generate multiple copies of a input image with a different quality settings. It will then intelligently pick the one with the best quality to filesize ration. This ensures your JPEG image will be at the smallest size with the highest possible quality, without the need for a human to select the optimal image.

## WebP Compression

WebP is a new image format introduced by Google in 2010 which supports both lossy and lossless compression. According to [Google](https://developers.google.com/speed/webp/), WebP lossless images are **26% smaller** in size compared to PNGs and WebP lossy images are **25-34% smaller** in size compared to JPEG images.

To recompress your PNG or JPEG files into WebP format simply set `WebP = true` flag in your optimize request. You can also optionally set `Lossy = true` flag to leverage WebP's lossy compression:

```C#
var response = client.OptimizeWait(new OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"))
    {
        Lossy = true,
        WebP = true
    });

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

## Image Type Conversion

Kraken allows you to easily convert different images from one type/format to another. If, for example, you would like to turn you transparent PNG file into a JPEG with a grey background.

In order to convert between different image types you need to add the `ConvertImage` object to you request. This object takes two properties:

`format` — The image format you wish to convert your image into. This can accept one of the following values: `ImageFormat.jpeg`, `ImageFormat.png` or `ImageFormat.gif`.

`background` — Background image when converting from transparent file formats like PNG or GIF into fully opaque format like JPEG. The background property can be passed in HEX notation "#f60" or "#ff6600". The default background color is white.

```C#
var response = client.OptimizeWait("c:\your-image-location-on-disk.jpg",
    new OptimizeUploadWaitRequest()
    {
        ConvertImage = new ConvertImage(ImageFormat.gif)
        {
            BackgroundColor = "#ffffff"
        }
    }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

## Preserving Metadata

By default, the Kraken API will strip all the metadata found in an image to make the image file as small as it is possible, and in both lossy and lossless modes. Entries like EXIF, XMP and IPTC tags, colour profile information, etc. will be stripped altogether.

However there are situations when you might want to preserve some of the meta information contained in the image, for example, copyright notice or geotags. In order to preserve the most important meta entries add an additional PreserveMeta array to your request with one or more of the following values:
`Date` `Copyright` `Geotag` `Orientation` `Profile`

```C#
var response = client.OptimizeWait(new OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"))
    {
        PreserveMeta = new PreserveMeta[] { PreserveMeta.Geotag }
    });

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

## External Storage
Kraken-net supports the option which allows you to store optimized images directly in your Microsoft Azure Blob Storage or S3 bucket. With just a few additional parameters your optimized images will be pushed to Microsoft Azure or S3.

### Azure Blob

**Azure Blob Storage with custom settings:**

```C#
using OptimizeWaitRequest = Kraken.Model.Azure.OptimizeWaitRequest;

var response = client.OptimizeWait(new OptimizeWaitRequest(
        new Uri("http://image-url.com/file.jpg"), "account", "key", "container")
        {
            ResizeImage = new ResizeImage { Height = 100, Width = 100 },
            WebP = true
        }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

**Azure Blob Storage upload with custom settings options:**

```C#
using OptimizeUploadWaitRequest = Kraken.Model.Azure.OptimizeUploadWaitRequest;

var response = client.OptimizeWait(
    "c:\your-image-location-on-disk.png",
    new OptimizeUploadWaitRequest("account", "key","container")
    {
        ResizeImage = new ResizeImage { Height = 100, Width = 100 },
        WebP = true
    });

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

**Azure Blob Storage with custom headers and metadata:**

```C#
using Kraken.Model.Azure;
using OptimizeWaitRequest = Kraken.Model.Azure.OptimizeWaitRequest;

var dataStore = DataStore("account", "key","container");

dataStore.AddMetadata("x-ms-meta-test1", "value1"); 
dataStore.AddHeaders("Cache-Control", "max-age=2222");

var response = client.OptimizeWait(
    new OptimizeWaitRequest(new Uri(TestData.ImageOne), dataStore)
    {
        WebP = true
    }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

### Amazon S3

**Amazon S3:**

```C#
using Kraken.Model.S3;
using OptimizeWaitRequest = Kraken.Model.S3.OptimizeWaitRequest;

var response = client.OptimizeWait(new OptimizeWaitRequest(
    new Uri("http://image-url.com/file.jpg"), "account", "key", "container", "region")
    {
        ResizeImage = new ResizeImage { Height = 100, Width = 100 },
        WebP = true
    });

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

**Amazon S3 upload with options:**

```C#
using OptimizeUploadWaitRequest = Kraken.Model.S3.OptimizeUploadWaitRequest;

var response = client.OptimizeWait(
    "c:\your-image-location-on-disk.png",
    new OptimizeUploadWaitRequest("account", "key", "container", "region")
    {
        ResizeImage = new ResizeImage {Height = 100, Width = 100},
        WebP = true
    });

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

**Azure Blob Storage with custom headers and metadata:**

```C#
using Kraken.Model.S3;
using OptimizeWaitRequest = Kraken.Model.S3.OptimizeWaitRequest;

var dataStore = new DataStore("account", "key","container");

dataStore.AddMetadata("x-amz-meta-test1", "value1"); 
dataStore.AddHeaders("Cache-Control", "public, max-age=123456");

var response = client.OptimizeWait(
    new OptimizeWaitRequest(new Uri(TestData.ImageOne), dataStore)
    {
        WebP = true
    }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

## Automatic Image Orientation

The EXIF (exchangeable image file format) standard specifies an Orientation tag that can be embedded in images, and is usually set in accordance with the reading from a gravity sensor or accelerometer in digital cameras and smartphones. This enables you to take a picture with your camera sideways or upside-down, and stand a reasonable chance of having it display properly on your computer.

For more information on Automatic Image Orientation, please consult the kraken API documentation. 

Code sample:

```C#
var response = client.OptimizeWait("c:\your-image-location-on-disk.jpg",
    new OptimizeUploadWaitRequest()
    {
        AutoOrient = true
    }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

## Chroma Subsampling 

JPEG is a lossy compression algorithm, meaning that it trades quality to achieve a smaller file size.

The whole point of the JPEG compression format is to reproduce photographs so as to minimize file size while keeping the visual qualities as accurate to the original as possible. For more information on Chroma subsampling, please consult the Kraken API documentation. 

The following options are supported: `Default` (4:2:0) `S422` (4:2:0) or `S444` (4:4:4).

```C#
var response = client.OptimizeWait("c:\your-image-location-on-disk.jpg",
    new OptimizeUploadWaitRequest()
    {
        Lossy = true,
        WebP = true,
        SamplingScheme.S422
    }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

## Image Resizing

Image resizing option is great for creating thumbnails or preview images in your applications. Kraken will first resize the given image and then optimize it with its vast array of optimization algorithms. The `resize` option needs a few parameters to be passed like desired `width` and/or `height` and a mandatory `strategy` property. For example:

```C#
var response = client.OptimizeWait("c:\your-image-location-on-disk.jpg",
    new OptimizeUploadWaitRequest()
    {
        ResizeImage = new ResizeImage
        {
            Width  = 100,
            Height = 100,
            Strategy = Strategy.fit
        }
    }
);

if(response.Result.StatusCode == HttpStatusCode.OK)
{
    var url = response.Result.Body.KrakedUrl;
}
```

The `strategy` property can have one of the following values:

- `exact` - Resize by exact width/height. No aspect ratio will be maintained.
- `portrait` - Exact width will be set, height will be adjusted according to aspect ratio.
- `landscape` - Exact height will be set, width will be adjusted according to aspect ratio.
- `auto` - The best strategy (portrait or landscape) will be selected for a given image according to aspect ratio.
- `fit`  - This option will crop and resize your images to fit the desired width and height
- `crop` - This option will crop your image to the exact size you specify with no distortion.
- `square` - This strategy will first crop the image by its shorter dimension to make it a square, then resize it to the specified size.
- `fill` - This strategy allows you to resize the image to fit the specified bounds while preserving the aspect ratio (just like auto strategy). The optional background property allows you to specify a color which will be used to fill the unused portions of the previously specified bounds.
The background property can be formatted in HEX notation "#f60" or "#ff6600". The default background color is white. 

## API Sandbox

The Kraken.io API Sandbox is designed to allow you to take all the time you need to integrate with the Kraken. The endpoints will parse your request, validate your JSON, process uploads, etc. Everything will reflect production usage on a fully-featured Kraken API plan apart from the actual optimization process itself. Instead Kraken API will return randomized optimization results. 

```C#
 var connection = connection.Create("key", "secret", true);
```

## Account Status

Kraken allows you to programatically query your account status, enabling you to retrieve details such as the name of the plan you have subscribed to, your total quota, used quota, remaining quota, and the "active" status of your account.

```C#
var response = client.UserStatus();
```

## LICENSE - MIT

Copyright (c) 2016 

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
