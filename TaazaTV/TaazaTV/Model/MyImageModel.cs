using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace TaazaTV.Model
{
    class MyImageModel : INotifyPropertyChanged, XLabs.Forms.Mvvm.ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ImageSource _ImageSource { get; set; }
        public ImageSource ImageSource
        {
            set
            {
                if (_ImageSource != value)
                {
                    _ImageSource = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this,
                            new PropertyChangedEventArgs("ImageSource"));
                    }
                }
            }
            get
            {
                return _ImageSource;
            }
        }

        private byte[] imageData;

        public byte[] ImageData { get { return imageData; } }

        private byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public async Task SelectPicture()
        {
            Setup();

            ImageSource = null;


            try
            {
                var mediaFile = await _Mediapicker.SelectPhotoAsync(new CameraMediaStorageOptions
                {
                    DefaultCamera = CameraDevice.Front,
                    MaxPixelDimension = 400
                });

                VideoInfo = mediaFile.Path;
                ImageSource = ImageSource.FromStream(() => mediaFile.Source);

            }
            catch (System.Exception ex)
            {
                Status = ex.Message;
            }
        }


        private static double ConvertBytesToMegabytes(long bytes)
        {
            double rtn_value = (bytes / 1024f) / 1024f;

            return rtn_value;
        }

        /// <summary>
        /// The _scheduler.
        /// </summary>
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        /// <summary>
        /// The picture chooser.
        /// </summary>
        private IMediaPicker _mediaPicker;

        /// <summary>
        /// The image source.
        /// </summary>
        private ImageSource _imageSource;


        /// <summary>
        /// The video info.
        /// </summary>
        private string _videoInfo;

        /// <summary>
        /// The take picture command.
        /// </summary>
        private Command _takePictureCommand;

        /// <summary>
        /// The select picture command.
        /// </summary>
        private Command _selectPictureCommand;
        private Command _selectPicturePathCommand;
        /// <summary>
        /// The select video command.
        /// </summary>
        private Command _selectVideoCommand;

        private string _status;


        ////private CancellationTokenSource cancelSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraViewModel" /> class.
        /// </summary>
        public MyImageModel()
        {
            Setup();
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        //public ImageSource ImageSource
        //{
        //    get
        //    {
        //        return _imageSource;
        //    }
        //    set
        //    {
        //        SetProperty(ref _imageSource, value);

        //    }
        //}

        /// <summary>
        /// Gets or sets the video info.
        /// </summary>
        /// <value>The video info.</value>
        public string VideoInfo
        {
            get
            {
                return _videoInfo;
            }
            set
            {
                SetProperty(ref _videoInfo, value);
            }
        }

        /// <summary>
        /// Gets the take picture command.
        /// </summary>
        /// <value>The take picture command.</value>
        public Command TakePictureCommand
        {
            get
            {
                return _takePictureCommand ?? (_takePictureCommand = new Command(
                                                                       async () => await TakePicture(),
                                                                       () => true));
            }
        }

        /// <summary>
        /// Gets the select video command.
        /// </summary>
        /// <value>The select picture command.</value>
        public Command SelectVideoCommand
        {
            get
            {
                return _selectVideoCommand ?? (_selectVideoCommand = new Command(
                                                                       async () => await SelectVideo(),
                                                                       () => true));
            }
        }

        /// <summary>
        /// Gets the select picture command.
        /// </summary>
        /// <value>The select picture command.</value>
        public Command SelectPictureCommand
        {
            get
            {
                return _selectPictureCommand ?? (_selectPictureCommand = new Command(
                                                                           async () => await SelectPicture(),
                                                               () => true));


            }
        }
        public Command SelectPicturePathCommand
        {
            get
            {
                //return _selectPicturePathCommand ?? (_selectPicturePathCommand = new Command(
                //                                                           async () =>  await SelectPicturePath(),
                //                                               () => true)) ;
                return _selectPicturePathCommand = new Command(async () => await SelectPicturePath());

            }

        }
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get { return _status; }
            private set { SetProperty(ref _status, value); }
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        private void Setup()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            var device = Resolver.Resolve<IDevice>();

            ////RM: hack for working on windows phone? 
            _mediaPicker = Resolver.Resolve<IMediaPicker>() ?? device.MediaPicker;


        }

        /// <summary>
        /// Takes the picture.
        /// </summary>
        /// <returns>Take Picture Task.</returns>
        private async Task<MediaFile> TakePicture()
        {
            Setup();

            ImageSource = null;

            return await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Status = t.Exception.InnerException.ToString();
                }
                else if (t.IsCanceled)
                {
                    Status = "Canceled";
                }
                else
                {
                    var mediaFile = t.Result;

                    ImageSource = ImageSource.FromStream(() => mediaFile.Source);

                    return mediaFile;
                }

                return null;
            }, _scheduler);
        }

        /// <summary>
        /// Selects the picture.
        /// </summary>
        /// <returns>Select Picture Task.</returns>
        //public async Task<string> SelectPicture()
        //{
        //    //Setup();

        //    //ImageSource = null;

        //    //         try
        //    //{
        //    //	var mediaFile = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
        //    //	{
        //    //		DefaultCamera = CameraDevice.Front,
        //    //		MaxPixelDimension = 400
        //    //	});
        //    //             ImageSource = ImageSource.FromStream(() => mediaFile.Source);



        //    //         }
        //    //catch (System.Exception ex)
        //    //{
        //    //	Status = ex.Message;
        //    //}
        //    return Status = await SelectPicturePath();
        //}
        //public byte[] ReadStream(Stream input)
        //{
        //    byte[] buffer = new byte[16 * 1024];
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        int read;
        //        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            ms.Write(buffer, 0, read);
        //        }
        //        return ms.ToArray();
        //    }

        //}
        private async Task<string> SelectPicturePath()
        {
            Setup();

            ImageSource = null;
            //_videoInfo = null;
            try
            {
                var mediaFile = await _mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                {
                    DefaultCamera = CameraDevice.Front,
                    MaxPixelDimension = 400
                });
                Uri path = new Uri(mediaFile.Path);
                ImageSource = Path.GetFileName(path.AbsoluteUri.ToString());
                byte[] imgData = ReadStream(mediaFile.Source);
                Status = Convert.ToBase64String(imgData);
                //ImageSource = base64String;
                //ImageInfo = base64String;
                //ImageSource = base64String;



            }
            catch (System.Exception ex)
            {
                Status = ex.Message;
            }
            return Status;
        }

        /// <summary>
        /// Selects the video.
        /// </summary>
        /// <returns>Select Video Task.</returns>
        private async Task SelectVideo()
        {
            Setup();

            //TODO Localize
            VideoInfo = "Selecting video";

            try
            {
                var mediaFile = await _mediaPicker.SelectVideoAsync(new VideoMediaStorageOptions());

                //TODO Localize
                VideoInfo = mediaFile != null
                                ? string.Format("Your video size {0} MB", ConvertBytesToMegabytes(mediaFile.Source.Length))
                                : "No video was selected";
            }
            catch (System.Exception ex)
            {
                if (ex is TaskCanceledException)
                {
                    //TODO Localize
                    VideoInfo = "Selecting video canceled";
                }
                else
                {
                    VideoInfo = ex.Message;
                }
            }
        }

        //private static double ConvertBytesToMegabytes(long bytes)
        //{
        //    return (bytes / 1024f) / 1024f;
        //}
    }
}
