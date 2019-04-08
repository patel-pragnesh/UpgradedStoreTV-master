using TaazaTV;
using XLabs.Forms.Mvvm;
using XLabs.Forms.Services;
using IMediaPicker = XLabs.Platform.Services.Media.IMediaPicker;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using TaazaTV.View.Accounts;
using System;
using TaazaTV.View;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections.ObjectModel;

namespace TaazaTV.Model
{


    /// <summary>
    /// Class CameraViewModel.
    /// </summary>
    [ViewType(typeof(RegistrationPage))]
    public class ImageViewModel : XLabs.Forms.Mvvm.ViewModel
    {
        //private ImageSource _ImageSource { get; set; }
        //public ImageSource ImageSource
        //{
        //    get { return _ImageSource; }
        //    set { SetProperty(ref _ImageSource, value); }
        //}

        private byte[] imageData;

        public byte[] ImageData { get { return imageData; } }


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

        private string _ImageLocation;
        public string ImageLocation
        {
            get
            {
                return _ImageLocation;
            }
            set
            {
                SetProperty(ref _ImageLocation, value);

            }
        }

        private string _status;
        private byte[] _ImageArray;

        ////private CancellationTokenSource cancelSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraViewModel" /> class.
        /// </summary>
        public ImageViewModel()
        {
            Setup();
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source.</value>
        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                SetProperty(ref _imageSource, value);

            }
        }

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
        public byte[] ImageArray
        {
            get { return _ImageArray; }
            private set { SetProperty(ref _ImageArray, value); }
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
        public  async Task<MediaFile> TakePicture()
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
                    Status = "A task was canceled.";
                }
                else
                {
                    var mediaFile = t.Result;

                    Uri path = new Uri(mediaFile.Path);
                    //ImageSource = Path.GetFileName(path.AbsoluteUri.ToString());
                    byte[] imgData = ReadStream(mediaFile.Source);
                    Status = Path.GetFileName(path.AbsoluteUri.ToString());
                    ImageArray = imgData;

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
        public async Task<string> SelectPicture()
        {
            
            return Status = await SelectPicturePath();

        }

        public async Task<string> SelectVideo()
        {
            return Status = await SelectVideoPath();
        }
        /// <summary>
        /// Selects the video.
        /// </summary>
        /// <returns>Select Video Task.</returns>
        public async Task<string> SelectVideoPath()
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
                Uri path = new Uri(mediaFile.Path);
                byte[] imgData = ReadStream(mediaFile.Source);
                Status = Path.GetFileName(path.AbsoluteUri.ToString());
                ImageArray = imgData;
                ImageSource = ImageSource.FromStream(() => mediaFile.Source);

                
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
            return Status;
        }

        public byte[] ReadStream(Stream input)
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
                //ImageSource = Path.GetFileName(path.AbsoluteUri.ToString());
                byte[] imgData = ReadStream(mediaFile.Source);
                _ImageLocation = path.AbsoluteUri.ToString();
                Status = Path.GetFileName(_ImageLocation);
                ImageArray = imgData;

            
                //ImageSource = ImageSource.FromStream(() =>
                //{
                //    return mediaFile.Source;
                //});
                //ImageInfo = base64String;
                //ImageSource = base64String;

                ImageSource = ImageSource.FromStream(() => mediaFile.Source);

            }
            catch (System.Exception ex)
            {
                Status = ex.Message;
            }
            return Status;
        }

       

        private static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

       

    }
}
